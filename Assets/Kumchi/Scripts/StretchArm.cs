/*
 *  StretchArm.cs
 *  のびる腕ができるグローブクラス
 *  制作者：熊澤　圭祐
 *  制作日：2025/11/20
*/
using Nakashi.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StretchArm : GloveBase
{
    // グローブリスト(読込)
    [SerializeField] private GloveListData gloveListData;

    [Header("Transforms")]
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform start;  // フォールバック／回転用
    [SerializeField] private Transform target; // シリアライズされたフォールバック。Use() でプレイヤーのターゲットに差し替えられます

    private BezierCurveData curveData;

    [Header("挙動パラメーター")]
    [SerializeField]
    private StretchArmParams actionParams;

    [Header("12-19 試遊会用グローブ")]
    [SerializeField]
    private string gloveName = "test";


    private List<Transform> bones = new List<Transform>();
    private float t = 0f;             // 0: たたまれた(戻った)状態, 1: 完全に伸びた状態
    private float waitTimer = 0f;

    private ArmPlayerController m_playerController;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    // ベジェ制御用キャッシュ（PhaseStart で決定）
    private Vector3 bezierP0; // ここは毎フレーム更新されるように扱う
    private Vector3 bezierP1;
    private Vector3 bezierP2; // 目標側はPhaseStartで固定

    // 初期 GlovePosition（local）
    private Vector3 m_handPositionLocal;
    // cached tangents（PhaseStartで算出して保持） world-space ベクトルとして固定
    private Vector3 cachedT0;
    private Vector3 cachedT1;

    // 曲線形状の基準回転up（PhaseStart 時点でキャプチャまたは固定）
    private Quaternion referenceRotation;
    private Vector3 referenceUp;

    // グローブ
    private GameObject gloveGameObject;
    // グローブスクリプト
    private GloveObject gloveObjectScript;

    // グローブが追従するTransform
    private Transform endBoneTransform;





    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        // base.Start();

        bones.Clear();
        if (rootBone != null) GetAllBones(rootBone);

        if (rootBone != null)
        {
            this.transform.localScale *= 0.4f;
            rootBone.rotation = Quaternion.LookRotation(Vector3.down);
            initialRotation = rootBone.rotation;
        }

        // グローブ生成
        GenerateGlove();

        base.Start();
    }

    private void GetAllBones(Transform current)
    {
        bones.Add(current);
        for (int i = 0; i < current.childCount; i++)
            GetAllBones(current.GetChild(i));

        // 最奥ボーンを保存
        if (current.childCount == 0)
            endBoneTransform = current;
    }

    protected override void Update()
    {
        base.Update();

        // グローブ追従
        if (gloveGameObject != null && endBoneTransform != null)
        {
            gloveGameObject.transform.position = endBoneTransform.position;
            gloveGameObject.transform.rotation = endBoneTransform.rotation * gloveObjectScript.GloveRotation;
        }
    }

    void GenerateGlove()
    {
        gloveListData = Resources.Load<GloveListData>("DataList/GloveListData");

        if (gloveListData == null)
        {
            Debug.LogError("GloveListDataが取得できませんでした。Resources/DataList/GloveListData" + gameObject);
            return;
        }

        if (gloveListData.GloveCount <= 0)
        {
            Debug.LogError("GloveListDataにグローブが登録されていません。" + gameObject);
            return;
        }

        // Prefab取得
        GameObject glovePrefab = gloveListData.GetGloveByName(gloveName);

        if (glovePrefab == null)
        {
            Debug.LogError("指定されたグローブが見つかりません。" + gameObject);
            return;
        }

        // 最奥ボーン取得
        Transform deepestChild = GetDeepestChild(this.transform);

        // 通常のグローブを生成
        gloveGameObject = Instantiate(glovePrefab);
        gloveGameObject.GetComponent<GloveObject>().Initialize(this.gameObject);

        // このオブジェクトの子にする
        gloveGameObject.transform.SetParent(this.transform, false);

        // グローブデータを取得する
        GloveParamData = gloveGameObject.GetComponent<GloveObject>().ParameterData;

        if (GloveParamData == null)
        {
            Debug.LogError("GloveParamDataが取得できませんでした。" + gameObject);
            return;
        }

        // 生成されたグローブからどのような曲線挙動をするか取得する
        curveData = gloveGameObject.GetComponent<GloveObject>().ParameterData.CurveData;

        // グローブスクリプト取得
        gloveObjectScript = gloveGameObject.GetComponent<GloveObject>();

        if (gloveObjectScript == null)
        {
            Debug.LogError("GloveObjectScriptが取得できませんでした。" + gameObject);
            return;
        }
    }


    /// <summary>
    /// 指定した Transform 配下で、一番深い（最奥）の Transform を取得する
    /// </summary>
    private Transform GetDeepestChild(Transform root)
    {
        Transform deepest = root;
        int maxDepth = 0;

        void Traverse(Transform current, int depth)
        {
            if (depth > maxDepth)
            {
                maxDepth = depth;
                deepest = current;
            }

            for (int i = 0; i < current.childCount; i++)
            {
                Traverse(current.GetChild(i), depth + 1);
            }
        }

        Traverse(root, 0);
        return deepest;
    }


    protected override void RegisterActions()
    {
        m_actionsDict[GloveActionType.NORMAL_ATTACK] = new List<Func<bool>>
        {
            PhaseStart,   // 初期化
            PhaseTravel,  // 伸びる
            PhaseRetract  // 戻る
        };
    }

    public override bool Use(ArmPlayerController playerController, GloveActionType type)
    {
        if (!base.Use(playerController, type)) return false;

        // パラメーター取得
        actionParams = gloveGameObject.GetComponent<GloveObject>().ParameterData.GetStretchArmParamsByType(type);

        m_playerController = playerController;

        var ptarget = m_playerController.Target;
        if (ptarget != null && ptarget.transform != null)
            target = ptarget.transform;

        // グローブに攻撃を行っている　を伝える
        gloveGameObject.GetComponent<GloveObject>().IsAttacking = true;

        return true;
    }

    // --- Phase 1: 初期化 ---
    private bool PhaseStart()
    {

        if (bones.Count < 2 || rootBone == null)
        {
            t = 0f;
            return true;
        }

        // GlovePosition(local) を保存
        m_handPositionLocal = this.GlovePosition;

        // p0/p2 の決定（p2 は target または maxDistance による制限）
        Vector3 handWorld = (transform.parent != null) ? transform.parent.TransformPoint(m_handPositionLocal) : m_handPositionLocal;
        bezierP0 = handWorld;

        Vector3 targetWorld = bezierP0;
        if (target != null) targetWorld = target.position;
        else if (start != null) targetWorld = start.position;

        Vector3 dir = (targetWorld - bezierP0);
        float dist = dir.magnitude;
        if (dist > Mathf.Epsilon)
            bezierP2 = (dist > actionParams.MaxDistance) ? bezierP0 + dir.normalized * actionParams.MaxDistance : targetWorld;
        else
            bezierP2 = targetWorld;

        // --- referenceRotation / referenceUp の決定（ここが今回の肝） ---
        if (actionParams.UseWorldForwardAsReference)
        {
            // 強制的に「世界の +Z 」基準で曲線形を決める（プレイヤー回転に依存しない）
            referenceRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // same as Quaternion.identity with typical axes
            referenceUp = Vector3.up;
        }
        else
        {
            // 従来の start 基準（もし必要ならこちらを使う）
            referenceRotation = (start != null) ? start.rotation : Quaternion.identity;
            referenceUp = referenceRotation * Vector3.up;
        }

        // cached tangents を referenceRotation ベースで world-space に変換して固定する
        Vector3 baseTangent0 = curveData.tangents[0];
        Vector3 baseTangent1 = curveData.tangents[1];

        cachedT0 = referenceRotation * baseTangent0;
        cachedT1 = referenceRotation * baseTangent1;

        // p1（初期）は cached tangents を使って組み立て
        Vector3 p1_start = bezierP0 + cachedT0;
        Vector3 p1_end = bezierP2 + cachedT1;
        bezierP1 = Vector3.Lerp(p1_start, p1_end, 0.5f);

        // rootBone の回転ターゲット（見た目の向き）は referenceUp を使って作る
        initialRotation = rootBone != null ? rootBone.rotation : Quaternion.identity;
        Vector3 forwardDir = (bezierP2 - bezierP0).normalized;
        if (forwardDir.sqrMagnitude > 0.0001f)
            targetRotation = Quaternion.LookRotation(forwardDir, referenceUp);
        else
            targetRotation = initialRotation;

        t = 0f;
        waitTimer = 0f;
        //Debug.Log("PhaseStart");
        //Debug.Log(referenceRotation);
        //Debug.Log(cachedT0);
        //Debug.Log(cachedT1);
        //Debug.Log(bezierP0);
        //Debug.Log(bezierP1);
        //Debug.Log(bezierP2);
        return true;
    }

    // --- Phase 2: 伸びる ---
    private bool PhaseTravel()
    {
        t = Mathf.MoveTowards(t, 1f, Time.deltaTime * actionParams.ExtendSpeed);

        if (rootBone != null)
            rootBone.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

        UpdateBonesByT(t);

        if (Mathf.Abs(t - 1f) < /*arriveEps*/ 0.01f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= actionParams.HitWaitTime)
            {
                return true;
            }
        }
        return false;
    }

    // --- Phase 3: 戻る ---
    private bool PhaseRetract()
    {
        // グローブに攻撃終了　を伝える
        gloveGameObject.GetComponent<GloveObject>().IsAttacking = false;

        t = Mathf.MoveTowards(t, 0f, Time.deltaTime * actionParams.RetractSpeed);

        if (rootBone != null)
            rootBone.rotation = Quaternion.Slerp(targetRotation, initialRotation, 1f - t);

        UpdateBonesByT(t);

        if (t <= 0f + Mathf.Epsilon)
        {
            t = 0f;
            return true;
        }
        return false;
    }

    /// <summary>
    /// t に応じてボーン群の位置・回転を更新
    /// - cachedT0/cachedT1 と referenceUp を使うことで曲線の形を固定（プレイヤー回転に依存しない）
    /// </summary>
    // UpdateBonesByT の修正版（該当メソッド全体を置き換えてください）
    // UpdateBonesByT の該当メソッド（角度ログを追加した版）
    private void UpdateBonesByT(float currentT)
    {
        if (bones.Count < 2 || curveData == null || rootBone == null) return;

        // p0 を毎フレーム計算
        bezierP0 = (transform.parent != null) ? transform.parent.TransformPoint(m_handPositionLocal) : m_handPositionLocal;

        // --- player の forward を XZ 平面に投影して符号付きヨー角を得る ---
        Vector3 playerForward = Vector3.forward;
        if (m_playerController != null && m_playerController.transform != null)
            playerForward = m_playerController.transform.forward;
        playerForward.y = 0f;
        if (playerForward.sqrMagnitude < 1e-6f) playerForward = Vector3.forward;
        playerForward.Normalize();

        // atan2 で -180..+180 の符号付きヨーを得る（+Z 基準）
        float playerYawDeg = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;

        // 打ち消す角度（負のヨー）
        float appliedYawDeg = -playerYawDeg;
        float appliedYawRad = appliedYawDeg * Mathf.Deg2Rad;
        float c = Mathf.Cos(appliedYawRad);
        float s = Mathf.Sin(appliedYawRad);

        // --- cachedT0/cachedT1 の XZ を 2D 回転で回す（Quaternion ではなく明示的に回す） ---
        Vector3 correctedT0 = cachedT0;
        Vector3 correctedT1 = cachedT1;

        // XZ の 2D 回転を適用（安定的）
        float x0 = cachedT0.x;
        float z0 = cachedT0.z;
        correctedT0.x = x0 * c - z0 * s;
        correctedT0.z = x0 * s + z0 * c;

        float x1 = cachedT1.x;
        float z1 = cachedT1.z;
        correctedT1.x = x1 * c - z1 * s;
        correctedT1.z = x1 * s + z1 * c;

        // --- ミラー補正（親や root に負スケールがある場合） ---
        float mirrorX = 1f;
        if (rootBone != null)
        {
            Vector3 parentLossy = (transform.parent != null) ? transform.parent.lossyScale : Vector3.one;
            Vector3 rootLossy = rootBone.lossyScale;
            mirrorX = Mathf.Sign(parentLossy.x * rootLossy.x);
            if (mirrorX == 0f) mirrorX = 1f;
        }
        if (mirrorX < 0f)
        {
            correctedT0.x *= -1f;
            correctedT1.x *= -1f;
        }

        // --- デバッグログ：角度と回転後の角度（XZ で atan2）を出す ---
        float origT0Yaw = Mathf.Atan2(cachedT0.x, cachedT0.z) * Mathf.Rad2Deg;
        float corrT0Yaw = Mathf.Atan2(correctedT0.x, correctedT0.z) * Mathf.Rad2Deg;
        float appliedT0 = Mathf.DeltaAngle(origT0Yaw, corrT0Yaw);

        float origT1Yaw = Mathf.Atan2(cachedT1.x, cachedT1.z) * Mathf.Rad2Deg;
        float corrT1Yaw = Mathf.Atan2(correctedT1.x, correctedT1.z) * Mathf.Rad2Deg;
        float appliedT1 = Mathf.DeltaAngle(origT1Yaw, corrT1Yaw);

        Debug.Log($"[角度ログ] playerYawDeg={playerYawDeg:F1} appliedYawDeg={appliedYawDeg:F1} (deg)");
        Debug.Log($"[角度ログ] cachedT0 yaw={origT0Yaw:F1} -> correctedT0 yaw={corrT0Yaw:F1}, applied={appliedT0:F1}");
        Debug.Log($"[角度ログ] cachedT1 yaw={origT1Yaw:F1} -> correctedT1 yaw={corrT1Yaw:F1}, applied={appliedT1:F1}");
        Debug.Log($"[角度ログ] mirrorX={mirrorX:F1}");

        // ここで correctedT0/1 を使って p1 を作る（以降は既存処理）
        Vector3 p1_start = bezierP0 + correctedT0;
        Vector3 p1_end = bezierP2 + correctedT1;
        bezierP1 = Vector3.Lerp(p1_start, p1_end, 0.5f);

        Vector3 p0 = bezierP0;
        Vector3 p1 = bezierP1;
        Vector3 p2 = bezierP2;

        // up はワールド上方向（+Y）で固定（安定性重視）
        Vector3 upForFrame = Vector3.up;

        // 以降は既存処理（right、rootBone 回転、ボーン配置）
        Vector3 dirCurve = (p2 - p0).normalized;
        Vector3 right = Vector3.Cross(dirCurve, upForFrame).normalized;
        if (right.sqrMagnitude < 1e-6f) right = Vector3.right;

        Vector3 forward0 = (GetBezier(p0, p1, p2, Mathf.Clamp01(0.01f)) - p0).normalized;
        if (forward0.sqrMagnitude > 0.0001f)
        {
            Quaternion rot0 = Quaternion.LookRotation(forward0, upForFrame);
            rootBone.rotation = rot0 * Quaternion.Euler(90f, 0f, 0f);
        }

        for (int i = 0; i < bones.Count; i++)
        {
            float u = (float)i / (bones.Count - 1);
            Vector3 pos = GetBezier(p0, p1, p2, u * currentT);

            if (i != 0)
            {
                float centerFalloff = Mathf.Sin(u * Mathf.PI);
                float sway = Mathf.Sin(Time.time * actionParams.SwaySpeed) * actionParams.SwayAmplitude * centerFalloff * currentT;
                pos += Vector3.down * sway;
            }

            float wave =
                Mathf.Sin(u * Mathf.PI * actionParams.CoilFrequency)
                * actionParams.CoilAmplitude
                * (1f - currentT);

            pos += right * wave;

            bones[i].position = pos;

            if (i > 0)
            {
                Vector3 forward = bones[i].position - bones[i - 1].position;
                if (forward.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(forward, upForFrame);
                    targetRot *= Quaternion.Euler(90f, 0f, 0f);
                    bones[i].rotation = targetRot;
                }
            }
        }

        // rootBone の Y スケール補間
        float minScaleY = 0.01f;
        float maxScaleY = 1.0f;
        Vector3 localScale = rootBone.localScale;
        localScale.y = Mathf.Lerp(minScaleY, maxScaleY, currentT);
        rootBone.localScale = localScale;
    }
    private Vector3 GetBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }
}