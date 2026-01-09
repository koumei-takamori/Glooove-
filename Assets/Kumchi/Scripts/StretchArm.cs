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

/// <summary>
/// 伸縮する腕を制御するグローブクラス
/// ベジェ曲線を使用して、腕が滑らかに伸び縮みする動作を実現します
/// </summary>
public class StretchArm : GloveBase
{
    // ============================================================
    // シリアライズフィールド（Inspectorで設定可能）
    // ============================================================

    /// <summary>
    /// グローブリスト（読込用）
    /// 利用可能なグローブのプレハブ情報を保持
    /// </summary>
    [SerializeField] private GloveListData gloveListData;

    [Header("Transforms")]
    /// <summary>
    /// 腕のルートボーン（腕の付け根）
    /// 腕全体の回転や伸縮の基準点となる
    /// </summary>
    [SerializeField] private Transform rootBone;

    /// <summary>
    /// 腕の開始位置（フォールバック／回転用）
    /// </summary>
    [SerializeField] private Transform start;

    /// <summary>
    /// ターゲット位置（シリアライズされたフォールバック）
    /// Use()メソッドでプレイヤーのターゲットに差し替えられます
    /// </summary>
    [SerializeField] private Transform target;

    /// <summary>
    /// ベジェ曲線の形状データ
    /// 腕の曲がり方や接線の情報を保持
    /// </summary>
    private BezierCurveData curveData;

    [Header("挙動パラメーター")]
    /// <summary>
    /// 伸縮動作のパラメーター
    /// 速度、待機時間、揺れなどの設定を含む
    /// </summary>
    [SerializeField]
    private StretchArmParams actionParams;

    [Header("グローブのキー")]
    /// <summary>
    /// 現在装着しているグローブの種類
    /// </summary>
    [SerializeField]
    private GloveType gloveType = GloveType.Normal_L;

    [Header("どのプレイヤーに付いているのか")]
    /// <summary>
    /// この腕を所有しているプレイヤーコントローラー
    /// </summary>
    private ArmPlayerController ownerArmPlayerController;

    // ============================================================
    // グローブ関連のプライベートフィールド
    // ============================================================

    /// <summary>
    /// 生成されたグローブのゲームオブジェクト
    /// </summary>
    private GameObject gloveGameObject;

    /// <summary>
    /// グローブのスクリプトコンポーネント
    /// </summary>
    private GloveObject gloveObjectScript;

    /// <summary>
    /// グローブが追従する最奥のボーンのTransform
    /// </summary>
    private Transform endBoneTransform;

    // ============================================================
    // 敵回避関連
    // ============================================================

    /// <summary>
    /// 敵を回避する際の固定座標
    /// </summary>
    public Vector3 enemyDodgePosition = Vector3.zero;

    /// <summary>
    /// 敵回避ポイントが設定されているかどうか
    /// </summary>
    public bool hasEnemyDodgePoint = false;

    // ============================================================
    // 伸ばし中フラグ
    // ============================================================

    /// <summary>
    /// 腕が伸ばし中かどうかを示すフラグ
    /// true: 伸ばし中または戻り中（次の伸ばし動作は不可）
    /// false: 完全に戻った状態（次の伸ばし動作が可能）
    /// </summary>
    private bool isStretching = false;

    /// <summary>
    /// 腕が伸ばし中かどうかを取得するプロパティ
    /// 外部から状態を確認する際に使用
    /// </summary>
    public bool IsStretching => isStretching;

    // ============================================================
    // プロパティ
    // ============================================================

    /// <summary>
    /// 腕に装着されているグローブの種類（取得・設定）
    /// </summary>
    public GloveType ArmGloveType { get { return gloveType; } set { gloveType = value; } }

    /// <summary>
    /// この腕を所有しているプレイヤーコントローラー（取得・設定）
    /// </summary>
    public ArmPlayerController OwnerArmPlayerController { get { return ownerArmPlayerController; } set { ownerArmPlayerController = value; } }

    /// <summary>
    /// 敵回避開始地点を設定（位置をコピー）
    /// 既に設定されている場合は警告を出力
    /// </summary>
    /// <param name="position">回避地点の座標</param>
    public void SetEnemyDodgePoint(Vector3 position)
    {
        // 既に回避ポイントが設定されている場合は警告
        if (hasEnemyDodgePoint)
        {
            Debug.LogWarning("StretchArm:SetEnemyDodgePoint: すでに回避ポイントが設定されています");
            return;
        }

        // 回避地点を設定
        enemyDodgePosition = position;
        hasEnemyDodgePoint = true;
    }

    [Header("伸縮時の腕のねじれの大きさ")]
    /// <summary>
    /// 伸びるときに腕がねじれる量（度数法）
    /// 値が大きいほど腕がより激しくねじれます
    /// </summary>
    [SerializeField] private float twistAmount = 0f;

    // ============================================================
    // ボーン制御関連のプライベートフィールド
    // ============================================================

    /// <summary>
    /// 腕を構成する全てのボーンのリスト
    /// rootBoneから階層を辿って取得
    /// </summary>
    private List<Transform> bones = new List<Transform>();

    /// <summary>
    /// 伸縮の進行度（0～1）
    /// 0: 完全に縮んだ状態, 1: 完全に伸びた状態
    /// </summary>
    private float t = 0f;

    /// <summary>
    /// 目標到達後の待機タイマー
    /// </summary>
    private float waitTimer = 0f;

    /// <summary>
    /// プレイヤーコントローラーのキャッシュ
    /// </summary>
    private ArmPlayerController m_playerController;

    /// <summary>
    /// rootBoneの初期回転
    /// </summary>
    private Quaternion initialRotation;

    /// <summary>
    /// rootBoneの目標回転
    /// </summary>
    private Quaternion targetRotation;

    // ============================================================
    // ベジェ曲線制御用のキャッシュ
    // ============================================================

    /// <summary>
    /// ベジェ曲線の開始点（P0）
    /// 毎フレーム更新される腕の付け根の位置
    /// </summary>
    private Vector3 bezierP0;

    /// <summary>
    /// ベジェ曲線の制御点（P1）
    /// 腕の曲がり具合を決定する中間点
    /// </summary>
    private Vector3 bezierP1;

    /// <summary>
    /// ベジェ曲線の終点（P2）
    /// 腕の先端が到達する目標位置（PhaseStartで固定）
    /// </summary>
    private Vector3 bezierP2;

    /// <summary>
    /// グローブの初期ローカル座標
    /// </summary>
    private Vector3 m_handPositionLocal;

    /// <summary>
    /// キャッシュされた開始点の接線ベクトル（ワールド空間）
    /// PhaseStartで算出して固定
    /// </summary>
    private Vector3 cachedT0;

    /// <summary>
    /// キャッシュされた終点の接線ベクトル（ワールド空間）
    /// PhaseStartで算出して固定
    /// </summary>
    private Vector3 cachedT1;

    /// <summary>
    /// 曲線形状の基準回転（PhaseStart時点でキャプチャ）
    /// </summary>
    private Quaternion referenceRotation;

    /// <summary>
    /// 基準となる上方向ベクトル
    /// </summary>
    private Vector3 referenceUp;

    /// <summary>
    /// ボーンの初期状態を記録する構造体
    /// ローカル・ワールド両方の位置と回転を保存
    /// </summary>
    private struct BoneState
    {
        public Vector3 localPosition;      // ローカル座標
        public Quaternion localRotation;   // ローカル回転
        public Vector3 worldPosition;      // ワールド座標
        public Quaternion worldRotation;   // ワールド回転
    }

    /// <summary>
    /// 全ボーンの初期状態のスナップショット
    /// リセット時にこの状態に戻す
    /// </summary>
    private List<BoneState> initialBoneStates = new List<BoneState>();

    /// <summary>
    /// グローブオブジェクトを取得するプロパティ
    /// ArmPlayerControllerに渡すために使用
    /// </summary>
    public GameObject GetGloveObject { get { return gloveListData.GetGlove(gloveType); } }

    // ============================================================
    // Unityライフサイクルメソッド
    // ============================================================

    /// <summary>
    /// 初期化処理（Awake）
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 開始処理
    /// ボーンの初期化とグローブの生成を行う
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // ボーンリストをクリアして再取得
        bones.Clear();
        if (rootBone != null) GetAllBones(rootBone);

        // rootBoneの初期回転を設定（下向きにする）
        if (rootBone != null)
        {
            rootBone.rotation = Quaternion.LookRotation(Vector3.down);
            initialRotation = rootBone.rotation;
        }

        // 全ボーンの初期状態を保存
        // ※重要：後でリセットする際に使用するため、確実にコピーを保存
        initialBoneStates.Clear();
        foreach (var bone in bones)
        {
            BoneState st = new BoneState
            {
                localPosition = bone.localPosition,
                localRotation = bone.localRotation,
                worldPosition = bone.position,
                worldRotation = bone.rotation
            };
            initialBoneStates.Add(st);
        }

        // グローブを生成
        GenerateGlove();

        // 初期状態は縮んだ状態にする
        PhaseRetract();

        // 伸ばし中フラグを初期化（開始時は伸びていない）
        isStretching = false;
    }

    /// <summary>
    /// rootBone配下の全てのボーンを再帰的に取得
    /// 最奥のボーンも同時に記録
    /// </summary>
    /// <param name="current">現在処理中のTransform</param>
    private void GetAllBones(Transform current)
    {
        // 現在のボーンをリストに追加
        bones.Add(current);

        // 子ボーンを再帰的に処理
        for (int i = 0; i < current.childCount; i++)
            GetAllBones(current.GetChild(i));

        // 子がない（最奥）ボーンを保存
        if (current.childCount == 0)
            endBoneTransform = current;
    }

    /// <summary>
    /// グローブを生成する処理
    /// Resourcesからグローブデータを読み込み、インスタンス化
    /// </summary>
    void GenerateGlove()
    {
        // グローブリストデータをResourcesから読み込み
        gloveListData = Resources.Load<GloveListData>("DataList/GloveListData");

        // データが取得できない場合はエラー
        if (gloveListData == null)
        {
            Debug.LogError("GloveListDataが取得できませんでした。Resources/DataList/GloveListData" + gameObject);
            return;
        }

        // グローブが登録されていない場合はエラー
        if (gloveListData.GloveCount <= 0)
        {
            Debug.LogError("GloveListDataにグローブが登録されていません。" + gameObject);
            return;
        }

        // 指定された種類のグローブPrefabを取得
        GameObject glovePrefab = gloveListData.GetGlove(gloveType);

        if (glovePrefab == null)
        {
            Debug.LogError("指定されたグローブが見つかりません。" + gameObject);
            return;
        }

        // 最奥のボーンを取得
        Transform deepestChild = GetDeepestChild(this.transform);

        // グローブを生成
        gloveGameObject = Instantiate(glovePrefab);
        gloveGameObject.GetComponent<GloveObject>().Initialize(this.gameObject);

        // このオブジェクトの子に設定
        gloveGameObject.transform.SetParent(this.transform, false);

        // グローブのパラメーターデータを取得
        GloveParamData = gloveGameObject.GetComponent<GloveObject>().ParameterData;

        if (GloveParamData == null)
        {
            Debug.LogError("GloveParamDataが取得できませんでした。" + gameObject);
            return;
        }

        // グローブから曲線挙動データを取得
        curveData = gloveGameObject.GetComponent<GloveObject>().ParameterData.CurveData;

        // グローブスクリプトを取得
        gloveObjectScript = gloveGameObject.GetComponent<GloveObject>();

        if (gloveObjectScript == null)
        {
            Debug.LogError("GloveObjectScriptが取得できませんでした。" + gameObject);
            return;
        }

        // プレイヤーデータをグローブに登録
        ArmPlayerData playerData = ownerArmPlayerController.GetPlayerData();
        if (playerData != null)
        {
            gloveObjectScript.RegisterArmPlayerData(playerData);
        }
    }

    /// <summary>
    /// 指定したTransform配下で一番深い（最奥）のTransformを取得
    /// 再帰的に全ての子を探索して最も深い階層を見つける
    /// </summary>
    /// <param name="root">探索の起点となるTransform</param>
    /// <returns>最も深い階層にあるTransform</returns>
    private Transform GetDeepestChild(Transform root)
    {
        Transform deepest = root;
        int maxDepth = 0;

        // ローカル関数：再帰的に深さを探索
        void Traverse(Transform current, int depth)
        {
            // 現在の深さが最大深さより大きければ更新
            if (depth > maxDepth)
            {
                maxDepth = depth;
                deepest = current;
            }

            // 全ての子を再帰的に探索
            for (int i = 0; i < current.childCount; i++)
            {
                Traverse(current.GetChild(i), depth + 1);
            }
        }

        // 探索開始
        Traverse(root, 0);
        return deepest;
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// グローブを最奥ボーンの位置に追従させる
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // グローブを最奥ボーンに追従させる
        if (gloveGameObject != null && endBoneTransform != null)
        {
            gloveGameObject.transform.position = endBoneTransform.position;
            gloveGameObject.transform.rotation = endBoneTransform.rotation * gloveObjectScript.GloveRotation;
        }
    }

    // ============================================================
    // アクション登録
    // ============================================================

    /// <summary>
    /// アクションの登録
    /// 通常攻撃は「開始→伸びる→戻る」の3フェーズで構成
    /// </summary>
    protected override void RegisterActions()
    {
        m_actionsDict[GloveActionType.NORMAL_ATTACK] = new List<Func<bool>>
        {
            PhaseStart,   // フェーズ1: 初期化
            PhaseTravel,  // フェーズ2: 伸びる
            PhaseRetract  // フェーズ3: 戻る
        };
    }

    /// <summary>
    /// グローブを使用する
    /// プレイヤーから呼び出され、攻撃アクションを開始
    /// 伸ばし中フラグがtrueの場合は使用できない
    /// </summary>
    /// <param name="playerController">使用するプレイヤーコントローラー</param>
    /// <param name="type">アクションの種類</param>
    /// <returns>使用に成功したかどうか</returns>
    public override bool Use(ArmPlayerController playerController, GloveActionType type)
    {
        // 既に伸ばし中の場合は使用不可
        if (isStretching)
        {
            Debug.Log("StretchArm: 腕が伸ばし中のため、次の伸ばし動作はできません");
            return false;
        }

        // 基底クラスのチェック
        if (!base.Use(playerController, type)) return false;

        // プレイヤーコントローラーをキャッシュ
        m_playerController = playerController;

        // アクションタイプに応じたパラメーターを取得
        actionParams = gloveObjectScript.ParameterData.GetStretchArmParamsByType(type);

        // 敵回避ポイントをリセット
        hasEnemyDodgePoint = false;
        enemyDodgePosition = Vector3.zero;

        // プレイヤーのターゲットを取得して設定
        var ptarget = m_playerController.Target;
        if (ptarget != null && ptarget.transform != null)
            target = ptarget.transform;

        // グローブに攻撃開始を通知
        gloveObjectScript.OnPlayerAttack(type);

        return true;
    }

    // ============================================================
    // フェーズ1: 初期化
    // ============================================================

    /// <summary>
    /// フェーズ1: 攻撃開始時の初期化処理
    /// ベジェ曲線の制御点や接線を計算して固定
    /// 伸ばし中フラグをtrueに設定
    /// </summary>
    /// <returns>フェーズ完了でtrue</returns>
    private bool PhaseStart()
    {
        // 攻撃開始SEを再生
        SoundManager.Instance.PlaySE("AttackStart");

        // 伸ばし中フラグを立てる
        // この時点から完全に戻るまで次の伸ばし動作は不可
        isStretching = true;

        // 必要なデータが揃っていない場合は即座に完了
        if (curveData == null || bones.Count < 2 || rootBone == null)
        {
            t = 0f;
            return true;
        }

        // グローブのローカル座標を保存
        m_handPositionLocal = this.GlovePosition;

        // ベジェ曲線の開始点（P0）を計算
        // 親のワールド座標系に変換
        Vector3 handWorld = (transform.parent != null) ? transform.parent.TransformPoint(m_handPositionLocal) : m_handPositionLocal;
        bezierP0 = handWorld;

        // ターゲット座標の決定（優先順位あり）
        Vector3 targetWorld = bezierP0;

        // 優先度1: 敵回避地点（固定座標）
        if (hasEnemyDodgePoint)
        {
            targetWorld = enemyDodgePosition;
        }
        // 優先度2: プレイヤーのターゲット
        else if (target != null)
        {
            targetWorld = target.position;
        }
        // 優先度3: startトランスフォーム
        else if (start != null)
        {
            targetWorld = start.position;
        }

        // ベジェ曲線の終点（P2）を計算
        // 最大距離制限を考慮
        Vector3 dir = (targetWorld - bezierP0);
        float dist = dir.magnitude;
        if (dist > Mathf.Epsilon)
            bezierP2 = (dist > actionParams.MaxDistance) ? bezierP0 + dir.normalized * actionParams.MaxDistance : targetWorld;
        else
            bezierP2 = targetWorld;

        // --- 基準回転と上方向の決定 ---
        // プレイヤーの回転に依存しない曲線形状を実現
        if (actionParams.UseWorldForwardAsReference)
        {
            // ワールド座標のZ軸を基準に（推奨設定）
            referenceRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            referenceUp = Vector3.up;
        }
        else
        {
            // startの回転を基準に（従来動作）
            referenceRotation = (start != null) ? start.rotation : Quaternion.identity;
            referenceUp = referenceRotation * Vector3.up;
        }

        // 接線ベクトルを基準回転を使ってワールド空間に変換
        // この接線がベジェ曲線の形状を決定
        Vector3 baseTangent0 = curveData.tangents[0];
        Vector3 baseTangent1 = curveData.tangents[1];

        cachedT0 = referenceRotation * baseTangent0;
        cachedT1 = referenceRotation * baseTangent1;

        // ベジェ曲線の制御点（P1）を計算
        // 両端の接線から中間点を算出
        Vector3 p1_start = bezierP0 + cachedT0;
        Vector3 p1_end = bezierP2 + cachedT1;
        bezierP1 = Vector3.Lerp(p1_start, p1_end, 0.5f);

        // rootBoneの目標回転を計算
        // 腕の向きを目標方向に設定
        initialRotation = rootBone != null ? rootBone.rotation : Quaternion.identity;
        Vector3 forwardDir = (bezierP2 - bezierP0).normalized;
        if (forwardDir.sqrMagnitude > 0.0001f)
            targetRotation = Quaternion.LookRotation(forwardDir, referenceUp);
        else
            targetRotation = initialRotation;

        // 進行度と待機タイマーをリセット
        t = 0f;
        waitTimer = 0f;

        return true;
    }

    // ============================================================
    // フェーズ2: 伸びる
    // ============================================================

    /// <summary>
    /// フェーズ2: 腕を伸ばす処理
    /// tを0から1まで増加させながらボーンを更新
    /// </summary>
    /// <returns>伸びきって待機時間が経過したらtrue</returns>
    private bool PhaseTravel()
    {
        // 進行度を伸張速度に応じて増加
        t = Mathf.MoveTowards(t, 1f, Time.deltaTime * actionParams.ExtendSpeed);

        // rootBoneを目標回転に向けて補間
        if (rootBone != null)
            rootBone.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

        // 全ボーンの位置を更新
        UpdateBonesByT(t);

        // 完全に伸びきった場合
        if (Mathf.Abs(t - 1f) < 0.01f)
        {
            // 待機タイマーを進める
            waitTimer += Time.deltaTime;

            // 設定された待機時間が経過したら次フェーズへ
            if (waitTimer >= actionParams.HitWaitTime)
            {
                // グローブに攻撃終了を通知
                gloveObjectScript.OnPlayerAttackEnd();
                return true;
            }
        }
        return false;
    }

    // ============================================================
    // フェーズ3: 戻る
    // ============================================================

    /// <summary>
    /// フェーズ3: 腕を元に戻す処理
    /// tを1から0まで減少させながらボーンを更新
    /// 完全に戻ったら伸ばし中フラグをfalseに設定
    /// </summary>
    /// <returns>完全に戻ったらtrue</returns>
    private bool PhaseRetract()
    {
        // 進行度を収縮速度に応じて減少
        t = Mathf.MoveTowards(t, 0f, Time.deltaTime * actionParams.RetractSpeed);

        // rootBoneを初期回転に向けて補間
        if (rootBone != null) rootBone.rotation = Quaternion.Slerp(targetRotation, initialRotation, 1f - t);

        // 全ボーンの位置を更新
        UpdateBonesByT(t);

        // 完全に戻った場合
        if (t <= 0f + Mathf.Epsilon)
        {
            t = 0f;

            // 全ボーンを初期状態に確実に戻す
            for (int i = 0; i < bones.Count; i++)
            {
                bones[i].localPosition = initialBoneStates[i].localPosition;
                bones[i].localRotation = initialBoneStates[i].localRotation;
            }

            // 敵回避ポイントをリセット
            hasEnemyDodgePoint = false;
            enemyDodgePosition = Vector3.zero;

            // 伸ばし中フラグを下ろす
            // この時点で次の伸ばし動作が可能になる
            isStretching = false;

            return true;
        }
        return false;
    }

    // ============================================================
    // ボーン更新処理
    // ============================================================

    /// <summary>
    /// 進行度tに応じて全ボーンの位置・回転を更新
    /// ベジェ曲線に沿って配置し、揺れやコイル効果を適用
    /// cachedT0/cachedT1とreferenceUpを使用して曲線の形状を固定（プレイヤー回転に依存しない）
    /// </summary>
    /// <param name="currentT">現在の進行度（0～1）</param>
    private void UpdateBonesByT(float currentT)
    {
        // 必要なデータが揃っていない場合は処理をスキップ
        if (bones.Count < 2 || curveData == null || rootBone == null) return;

        // ベジェ曲線の開始点（P0）を毎フレーム更新
        bezierP0 = (transform.parent != null) ? transform.parent.TransformPoint(m_handPositionLocal) : m_handPositionLocal;

        // =================================================
        // 追従先座標の決定（優先度順）
        // 優先度: EnemyDodgePoint → target → start
        // =================================================
        Vector3 followPos = Vector3.zero;
        bool hasFollowPos = false;

        if (hasEnemyDodgePoint)
        {
            // 回避開始時の位置（固定）
            followPos = enemyDodgePosition;
            hasFollowPos = true;
        }
        else if (target != null)
        {
            // プレイヤーのターゲット位置
            followPos = target.position;
            hasFollowPos = true;
        }
        else if (start != null)
        {
            // スタート位置
            followPos = start.position;
            hasFollowPos = true;
        }

        // ベジェ曲線の終点（P2）を毎フレーム更新
        if (hasFollowPos)
        {
            Vector3 dir = followPos - bezierP0;
            float dist = dir.magnitude;

            if (dist > Mathf.Epsilon)
            {
                // 最大距離制限を維持
                bezierP2 = (dist > actionParams.MaxDistance)
                    ? bezierP0 + dir.normalized * actionParams.MaxDistance
                    : followPos;
            }
        }

        // --- プレイヤーのforward方向を取得してヨー角を計算 ---
        // XZ平面に投影して符号付きヨー角を取得
        Vector3 playerForward = Vector3.forward;
        if (m_playerController != null && m_playerController.transform != null)
            playerForward = m_playerController.transform.forward;

        // Y成分を0にしてXZ平面に投影
        playerForward.y = 0f;
        if (playerForward.sqrMagnitude < 1e-6f) playerForward = Vector3.forward;
        playerForward.Normalize();

        // atan2で符号付きヨー角を計算（-180～+180度）
        float playerYawDeg = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;

        // プレイヤー回転を打ち消すための角度（負のヨー）
        float appliedYawDeg = -playerYawDeg;
        float appliedYawRad = appliedYawDeg * Mathf.Deg2Rad;
        float c = Mathf.Cos(appliedYawRad);
        float s = Mathf.Sin(appliedYawRad);

        // --- キャッシュされた接線ベクトルのXZ成分を2D回転 ---
        // Quaternionではなく明示的な2D回転で安定性を確保
        Vector3 correctedT0 = cachedT0;
        Vector3 correctedT1 = cachedT1;

        // 接線0のXZ平面での回転
        float x0 = cachedT0.x;
        float z0 = cachedT0.z;
        correctedT0.x = x0 * c - z0 * s;
        correctedT0.z = x0 * s + z0 * c;

        // 接線1のXZ平面での回転
        float x1 = cachedT1.x;
        float z1 = cachedT1.z;
        correctedT1.x = x1 * c - z1 * s;
        correctedT1.z = x1 * s + z1 * c;

        // --- ミラー補正（親やrootに負スケールがある場合） ---
        float mirrorX = 1f;
        if (rootBone != null)
        {
            // 親とrootのlossyScaleからミラー係数を計算
            Vector3 parentLossy = (transform.parent != null) ? transform.parent.lossyScale : Vector3.one;
            Vector3 rootLossy = rootBone.lossyScale;
            mirrorX = Mathf.Sign(parentLossy.x * rootLossy.x);
            if (mirrorX == 0f) mirrorX = 1f;
        }

        // X軸が反転している場合は接線のX成分を反転
        if (mirrorX < 0f)
        {
            correctedT0.x *= -1f;
            correctedT1.x *= -1f;
        }

        // --- デバッグ用：回転前後の接線角度を計算 ---
        // （実際の処理には影響しない）
        float origT0Yaw = Mathf.Atan2(cachedT0.x, cachedT0.z) * Mathf.Rad2Deg;
        float corrT0Yaw = Mathf.Atan2(correctedT0.x, correctedT0.z) * Mathf.Rad2Deg;
        float appliedT0 = Mathf.DeltaAngle(origT0Yaw, corrT0Yaw);

        float origT1Yaw = Mathf.Atan2(cachedT1.x, cachedT1.z) * Mathf.Rad2Deg;
        float corrT1Yaw = Mathf.Atan2(correctedT1.x, correctedT1.z) * Mathf.Rad2Deg;
        float appliedT1 = Mathf.DeltaAngle(origT1Yaw, corrT1Yaw);

        // --- ベジェ曲線の制御点（P1）を補正された接線で再計算 ---
        Vector3 p1_start = bezierP0 + correctedT0;
        Vector3 p1_end = bezierP2 + correctedT1;
        bezierP1 = Vector3.Lerp(p1_start, p1_end, 0.5f);

        // ベジェ曲線の3点を確定
        Vector3 p0 = bezierP0;
        Vector3 p1 = bezierP1;
        Vector3 p2 = bezierP2;

        // 曲線の上方向をワールドY軸で固定（安定性重視）
        Vector3 upForFrame = Vector3.up;

        // 曲線方向と上方向から右方向を計算（コイル効果用）
        Vector3 dirCurve = (p2 - p0).normalized;
        Vector3 right = Vector3.Cross(dirCurve, upForFrame).normalized;
        if (right.sqrMagnitude < 1e-6f) right = Vector3.right;

        // rootBoneの回転を設定
        // 曲線の最初の方向を向くように
        Vector3 forward0 = (GetBezier(p0, p1, p2, Mathf.Clamp01(0.01f)) - p0).normalized;
        if (forward0.sqrMagnitude > 0.0001f)
        {
            Quaternion rot0 = Quaternion.LookRotation(forward0, upForFrame);
            rootBone.rotation = rot0 * Quaternion.Euler(90f, 0f, 0f);
        }

        // --- 全ボーンをベジェ曲線に沿って配置 ---
        for (int i = 0; i < bones.Count; i++)
        {
            // ボーンの位置をベジェ曲線上に配置（0～1の範囲を進行度で調整）
            float u = (float)i / (bones.Count - 1);
            Vector3 pos = GetBezier(p0, p1, p2, u * currentT);

            // 揺れ効果の適用（先端ボーン以外）
            if (i != 0)
            {
                // 中央ほど揺れが大きくなる減衰係数
                float centerFalloff = Mathf.Sin(u * Mathf.PI);
                // 時間と速度に応じた揺れ
                float sway = Mathf.Sin(Time.time * actionParams.SwaySpeed) * actionParams.SwayAmplitude * centerFalloff * currentT;
                pos += Vector3.down * sway;
            }

            // コイル（螺旋）効果の適用
            // 伸びるにつれて減衰
            float wave = Mathf.Sin(u * Mathf.PI * actionParams.CoilFrequency) * actionParams.CoilAmplitude * (1f - currentT);
            pos += right * wave;

            // ボーンの位置を設定
            bones[i].position = pos;

            // 回転の設定（rootBone以外）
            if (i > 0)
            {
                // 前のボーンから現在のボーンへの方向
                Vector3 forward = bones[i].position - bones[i - 1].position;

                if (forward.sqrMagnitude > 0.0001f)
                {
                    // 方向に基づいた回転を計算
                    Quaternion dirRot = Quaternion.LookRotation(forward, upForFrame);
                    dirRot *= Quaternion.Euler(90f, 0f, 0f);

                    Quaternion finalRot = dirRot;

                    // ねじれ効果の適用（子ボーンがある場合）
                    if (bones[i].childCount > 0)
                    {
                        // ねじれの軸（次のボーンへの方向）
                        Vector3 axisWorld = (bones[i + 1].position - bones[i].position).normalized;
                        // ねじれ角度（ボーンの位置に応じて減衰）
                        float angle = (twistAmount - i * twistAmount / 10.0f) * Time.deltaTime;
                        Quaternion twist = Quaternion.AngleAxis(angle, axisWorld);

                        // 現在の回転にねじれを積算
                        finalRot = twist * bones[i].rotation;
                    }

                    // 最終的な回転を反映
                    bones[i].rotation = finalRot;
                }
            }
        }

        // rootBoneのYスケールを進行度に応じて変化
        // 伸びるときは縮み、戻るときは元に戻る
        float minScaleY = 0.25f;
        float maxScaleY = 1.0f;
        Vector3 localScale = rootBone.localScale;
        localScale.y = Mathf.Lerp(minScaleY, maxScaleY, currentT);
        rootBone.localScale = localScale;
    }

    /// <summary>
    /// 2次ベジェ曲線上の点を計算
    /// 3つの制御点（p0, p1, p2）とパラメータtから曲線上の座標を取得
    /// </summary>
    /// <param name="p0">開始点</param>
    /// <param name="p1">制御点</param>
    /// <param name="p2">終点</param>
    /// <param name="t">パラメータ（0～1）</param>
    /// <returns>曲線上の座標</returns>
    private Vector3 GetBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // ベジェ曲線の公式
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }
}