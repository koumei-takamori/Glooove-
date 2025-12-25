/**********************************************************
 *
 *  StretchPunch.cs
 *  伸びるパンチができるグローブクラス
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
/// 射撃グローブクラス
/// </summary>
public class StretchPunchGlove : GloveBase
{
    private ArmPlayerController m_playerController;

    [SerializeField] private float m_maxDistance = 6.0f;
    [SerializeField] private float m_extendSpeed = 18.0f;
    [SerializeField] private float m_retractSpeed = 24.0f;
    [SerializeField] private float m_arriveEps = 0.06f;

    private Vector3 m_targetPos;
    private Vector3 m_hundPosition;

    /// <summary>
    /// 実行前処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// アクションを登録（開始→伸びる→戻る）
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

    /// <summary>
    /// 使用開始
    /// </summary>
    public override bool Use(ArmPlayerController playerController, GloveActionType type)
    {
        // クールダウン等の共通処理
        if (!base.Use(playerController, type)) return false;

        // プレイヤーを取得
        m_playerController = playerController;
        // 必要ならここで forward を aim に合わせて回すなどの拡張可

        return true;
    }

    // --- Phase 1: 開始位置と目標位置の決定
    private bool PhaseStart()
    {
        m_hundPosition = this.GlovePosition;
        m_targetPos = m_playerController.Target.transform.position;

        return true;
    }

    // --- Phase 2: 伸びる（目標まで移動）
    private bool PhaseTravel()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            m_targetPos,
            m_extendSpeed * Time.deltaTime
        );

        float d = (transform.position - m_targetPos).sqrMagnitude;
        return d <= m_arriveEps * m_arriveEps;

        // このままだとのびきるまで次の処理に映らないため
        // なにかに当たったらtrueを返す工夫が必要
    }

    // --- Phase 3: 戻る（開始位置まで戻ったら終了）
    private bool PhaseRetract()
    {
        // 元の位置に戻す
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            this.GlovePosition,
            m_retractSpeed * Time.deltaTime
        );

        float d = (transform.localPosition - this.GlovePosition).sqrMagnitude;
        return d <= m_arriveEps * m_arriveEps;
    }
}
