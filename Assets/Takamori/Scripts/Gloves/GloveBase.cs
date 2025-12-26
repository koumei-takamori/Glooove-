/**********************************************************
 *
 *  GloveBase.cs
 *  グローブの基底クラス（修正版）
 *
 *  制作者 : 髙森 煌明
 *  修正日 : 2025/11/21
 *
 *********************************************************/
using Nakashi.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グローブの基底クラス
/// </summary>
public abstract class GloveBase : MonoBehaviour
{
    // グローブの情報
    private GloveData m_gloveData;

    // 攻撃別アクション関数リスト
    protected Dictionary<GloveActionType, List<Func<bool>>> m_actionsDict;

    // 攻撃別現在のアクションインデックス
    private Dictionary<GloveActionType, int> m_indexDict;

    // 攻撃別攻撃中フラグ
    private Dictionary<GloveActionType, bool> m_isActiveDict;

    // 攻撃別クールタイマー
    private Dictionary<GloveActionType, float> m_coolTimers;

    // グローブのローカル初期位置
    private Vector3 m_glovePosition;

    // プロパティ
    public GloveData GloveParamData { get { return m_gloveData; } set { m_gloveData = value; } }

    public Vector3 GlovePosition { get { return m_glovePosition; } set { m_glovePosition = value; } }

    // 攻撃中フラグを取得
    public bool IsActionActive(GloveActionType type)
    {
        if (m_isActiveDict.ContainsKey(type))
        {
            return m_isActiveDict[type];
        }
        return false;
    }

    /*--------------------------------------------------------------------------------
    || 実行前処理
    --------------------------------------------------------------------------------*/
    protected virtual void Awake()
    {
        // 各 Dictionary を生成
        m_actionsDict = new Dictionary<GloveActionType, List<Func<bool>>>();
        m_indexDict = new Dictionary<GloveActionType, int>();
        m_isActiveDict = new Dictionary<GloveActionType, bool>();
        m_coolTimers = new Dictionary<GloveActionType, float>();

        // Enum のすべての値で最低限のエントリを作成しておく（キー欠損を防ぐ）
        foreach (GloveActionType type in Enum.GetValues(typeof(GloveActionType)))
        {
            EnsureTypeInitialized(type);
        }

        // GloveData があるならデータを辞書に注入（Start の前に値だけセットしておく）
        if (m_gloveData != null)
        {
            FillFromGloveData();
        }
    }

    /*--------------------------------------------------------------------------------
    || 初期化処理
    --------------------------------------------------------------------------------*/
    protected virtual void Start()
    {
        // 派生クラスでのアクション登録は Start で行う（FillFromGloveData は Awake で済ませてある）
        RegisterActions();
    }

    /*--------------------------------------------------------------------------------
    || 更新処理
    --------------------------------------------------------------------------------*/
    protected virtual void Update()
    {
        // 攻撃別クールダウンの更新（コピーされたキー列挙で安全）
        CoolDown();

        // 進行中のアクションは更新する（キーのスナップショットを使う）
        var actionKeys = new List<GloveActionType>(m_actionsDict.Keys);
        foreach (var actionKey in actionKeys)
        {
            // 存在チェック（安全策）
            if (!m_isActiveDict.ContainsKey(actionKey)) continue;

            if (m_isActiveDict[actionKey])
            {
                GloveUpdate(actionKey);
            }
        }
    }

    /*--------------------------------------------------------------------------------
    || 使用（攻撃タイプの指定）
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 使用（攻撃タイプの指定）
    /// </summary>
    /// <param name="playerController">プレイヤー</param>
    /// <param name="type">実行する攻撃の種類</param>
    /// <returns>使用できたかどうか</returns>
    public virtual bool Use(ArmPlayerController playerController, GloveActionType type)
    {
        // グローブの情報がない場合は処理しない
        if (m_gloveData == null)
        {
            Debug.LogWarning($"{nameof(GloveBase)}.Use: m_gloveData が null です。");
            return false;
        }

        // type が初期化されているか確認（なければ初期化）
        EnsureTypeInitialized(type);

        // 現在アクティブにする
        m_isActiveDict[type] = true;

        // クールタイムを取得してセット（データが存在するか確認）
        float cooltime = 0f;
        switch (type)
        {
            case GloveActionType.NORMAL_ATTACK:
                cooltime = m_gloveData.NormalAttack != null ? m_gloveData.NormalAttack.CoolTime : 0f;
                break;
            case GloveActionType.SPECIAL_ATTACK:
                cooltime = m_gloveData.SpecialAttack != null ? m_gloveData.SpecialAttack.CoolTime : 0f;
                break;
            case GloveActionType.GRAP:
                cooltime = m_gloveData.Grap != null ? m_gloveData.Grap.CoolTime : 0f;
                break;
            default:
                break;
        }

        m_coolTimers[type] = cooltime;
        // アクションインデックスは開始時にリセットしておく
        m_indexDict[type] = 0;

        return true;
    }

    /// <summary>
    /// 指定の type のエントリが無ければ初期化するヘルパー（再入可能）
    /// </summary>
    protected void EnsureTypeInitialized(GloveActionType type)
    {
        if (!m_actionsDict.ContainsKey(type)) m_actionsDict[type] = new List<Func<bool>>();
        if (!m_indexDict.ContainsKey(type)) m_indexDict[type] = 0;
        if (!m_isActiveDict.ContainsKey(type)) m_isActiveDict[type] = false;
        if (!m_coolTimers.ContainsKey(type)) m_coolTimers[type] = 0f;
    }

    /// <summary>
    /// GloveData の内容を各 Dictionary に注入する
    /// （ここではクールタイム等を注入している）
    /// </summary>
    private void FillFromGloveData()
    {
        if (m_gloveData == null) return;

        // NORMAL_ATTACK
        EnsureTypeInitialized(GloveActionType.NORMAL_ATTACK);
        if (m_gloveData.NormalAttack != null)
            m_coolTimers[GloveActionType.NORMAL_ATTACK] = m_gloveData.NormalAttack.CoolTime;

        // SPECIAL_ATTACK
        EnsureTypeInitialized(GloveActionType.SPECIAL_ATTACK);
        if (m_gloveData.SpecialAttack != null)
            m_coolTimers[GloveActionType.SPECIAL_ATTACK] = m_gloveData.SpecialAttack.CoolTime;

        // GRAP
        EnsureTypeInitialized(GloveActionType.GRAP);
        if (m_gloveData.Grap != null)
            m_coolTimers[GloveActionType.GRAP] = m_gloveData.Grap.CoolTime;
    }

    /*--------------------------------------------------------------------------------
    || 各派生でアクション（フェーズ）を登録
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 各派生でアクション（フェーズ）を登録
    /// </summary>
    protected abstract void RegisterActions();

    /*--------------------------------------------------------------------------------
    || グローブアクション更新処理
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// グローブアクション更新処理
    /// </summary>
    /// <param name="type">グローブアクションの種類</param>
    /// <returns>アクション完了フラグ</returns>
    private bool GloveUpdate(GloveActionType type)
    {
        // アクションがない場合は処理せず完了扱い（かつフラグを下げる）
        if (!m_actionsDict.ContainsKey(type) || m_actionsDict[type] == null || m_actionsDict[type].Count == 0)
        {
            if (m_isActiveDict.ContainsKey(type)) m_isActiveDict[type] = false;
            return true;
        }

        // index の存在と範囲確認
        if (!m_indexDict.ContainsKey(type)) m_indexDict[type] = 0;
        int index = m_indexDict[type];
        if (index < 0 || index >= m_actionsDict[type].Count)
        {
            // 範囲外ならリセットして終了
            m_indexDict[type] = 0;
            m_isActiveDict[type] = false;
            return true;
        }

        // 現在のフェーズを実行（戻り値が true なら次のフェーズへ）
        bool finishedPhase = false;
        try
        {
            finishedPhase = m_actionsDict[type][index].Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"GloveUpdate: アクション実行中に例外が発生しました type={type} index={index} ex={ex}");
            // 安全に終了
            m_isActiveDict[type] = false;
            m_indexDict[type] = 0;
            return true;
        }

        if (finishedPhase)
        {
            if (index < m_actionsDict[type].Count - 1)
            {
                m_indexDict[type]++;
            }
            else
            {
                // 全フェーズ完了
                m_isActiveDict[type] = false;
                m_indexDict[type] = 0; // 次回のためにリセット
                return true;
            }
        }

        return false;
    }

    /*--------------------------------------------------------------------------------
    || 攻撃別クールダウン
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 攻撃別クールダウン
    /// </summary>
    private void CoolDown()
    {
        // Keys のスナップショットを作成してからループ（列挙中のコレクション変更回避）
        var keys = new List<GloveActionType>(m_coolTimers.Keys);

        foreach (var key in keys)
        {
            // 安全チェック: キーが現時点で存在するか
            if (!m_coolTimers.ContainsKey(key) || !m_isActiveDict.ContainsKey(key)) continue;

            // 0より大きく、かつ攻撃中でなければクールダウン
            if (m_coolTimers[key] > 0f && !m_isActiveDict[key])
            {
                m_coolTimers[key] -= Time.deltaTime;
                // 下限クリップ
                if (m_coolTimers[key] < 0f) m_coolTimers[key] = 0f;
            }
        }
    }
}


