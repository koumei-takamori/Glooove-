/**********************************************************
 *
 *  ArmBase.cs
 *  曲線を作成を試してみる
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/08/06
 *
 *********************************************************/
using Nakashi.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲線を作成を試してみる
/// </summary>
public class ArmBase : GloveBase
{
    // プレイヤー
    private ArmPlayerController m_playerController;

    [Header("Transforms")]
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform start;  // フォールバック／回転用
    [SerializeField] private Transform target; // シリアライズされたフォールバック。Use() でプレイヤーのターゲットに差し替えられます

    [Header("Bezier")]
    [SerializeField] private BezierCurveData curveData;

    [Header("伸縮速度")]
    [SerializeField] private float extendSpeed = 6f;
    [SerializeField] private float retractSpeed = 3f;

    [Header("挙動パラメータ")]
    [SerializeField] private float coilFrequency = 0f;
    [SerializeField] private float coilAmplitude = 0f;
    [SerializeField] private float hitWaitTime = 0.5f;

    [Header("ゆらぎ")]
    [SerializeField] private float swayAmplitude = 0.3f;
    [SerializeField] private float swaySpeed = 2.0f;

    [Header("共通")]
    [SerializeField] private float maxDistance = 6f;       // 最大伸長距離（開始点から）
    [SerializeField] private float arriveEps = 0.01f;      // 到達許容誤差 (t ベース)

    [Header("動作オプション")]
    [SerializeField] private bool usePlayerForward = true; // true のとき腕の前方をプレイヤーの向きに合わせる
    [SerializeField] private float forwardForceDistance = 6f; // usePlayerForward の場合に優先的に使いたい長さ（0 = target 距離を尊重）

    [Header("曲線基準")]
    [SerializeField] private bool useWorldForwardAsReference = true; // true: 常に世界 +Z を基準に曲がりを固定する（これを推奨）
    // (false にすると start の回転を基準にする従来動作に近くなる)

    private List<Transform> bones = new List<Transform>();
    private float t = 0f;             // 0: たたまれた(戻った)状態, 1: 完全に伸びた状態
    private float waitTimer = 0f;

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

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Start()
    {
        base.Start();

        bones.Clear();
        if (rootBone != null) GetAllBones(rootBone);

        if (rootBone != null)
        {
            this.transform.localScale *= 0.4f;
            rootBone.rotation = Quaternion.LookRotation(Vector3.down);
            initialRotation = rootBone.rotation;
        }

    }

    private void GetAllBones(Transform current)
    {
        bones.Add(current);
        for (int i = 0; i < current.childCount; i++)
            GetAllBones(current.GetChild(i));
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /*--------------------------------------------------------------------------------
　　||  アクションを登録
　　--------------------------------------------------------------------------------*/
    /// <summary>
    ///  アクションを登録
    /// </summary>
    protected override void RegisterActions()
    {
        m_actionsDict[GloveActionType.NORMAL_ATTACK] = new List<Func<bool>>
        {
            PhaseStart,   // 初期化
            PhaseTravel,  // 伸びる
            PhaseRetract  // 戻る
        };
    }

    /*--------------------------------------------------------------------------------
　　|| 使用開始
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 使用開始
    /// </summary>
    public override bool Use(ArmPlayerController playerController, GloveActionType type)
    {
        // クールダウン等の共通処理
        if (!base.Use(playerController, type)) return false;

        // プレイヤーを取得
        m_playerController = playerController;

        return true;
    }

    // --- Phase 1: 初期化 ---
    private bool PhaseStart()
    {

        if (curveData == null || bones.Count < 2 || rootBone == null)
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
            bezierP2 = (dist > maxDistance) ? bezierP0 + dir.normalized * maxDistance : targetWorld;
        else
            bezierP2 = targetWorld;

        // --- referenceRotation / referenceUp の決定（ここが今回の肝） ---
        if (useWorldForwardAsReference)
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
        t = Mathf.MoveTowards(t, 1f, Time.deltaTime * extendSpeed);

        if (rootBone != null)
            rootBone.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

        UpdateBonesByT(t);

        if (Mathf.Abs(t - 1f) < arriveEps)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= hitWaitTime)
            {
                return true;
            }
        }
        return false;
    }

    // --- Phase 3: 戻る ---
    private bool PhaseRetract()
    {
        t = Mathf.MoveTowards(t, 0f, Time.deltaTime * retractSpeed);

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
                float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude * centerFalloff * currentT;
                pos += Vector3.down * sway;
            }

            float wave = Mathf.Sin(u * Mathf.PI * coilFrequency) * coilAmplitude * (1f - currentT);
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
