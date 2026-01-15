/**********************************************************
 *
 *  SelectPlayer.cs
 *  セレクトシーンのプレイヤー
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// セレクトシーンのプレイヤー
/// </summary>
[RequireComponent(typeof(SelectPlayerInputReceiver))]
public class SelectPlayer : MonoBehaviour
{
    /// <summary>
    /// セレクトシーンのプレイヤーステート
    /// </summary>
    public enum SelectPlayerState
    {
        CharaSelect = 0,      // キャラ選択状態
        GloveSelect = 1,      // グローブ選択状態
        Ready = 2       　　　// 準備確認状態
    }

    // プレイヤーID
    private int m_playerId;

    // デバイス
    private InputDevice m_inputDevice;

    // ステートマシン
    private StateMachine<SelectPlayer> m_stateMachine;

    // 入力を取得するクラス
    [SerializeField]
    private SelectPlayerInputReceiver m_inputReceiver;

    // プレイヤーが操作するUI
    private SelectPlayerUIManager m_uiManager;

    // 選択中キャラIndex
    private int m_charaIndex;
    private const int CHARA_MAX = 4;

    // 現在操作中のグローブ
    private GloveSide m_currentGloveSide = GloveSide.Left;

    // 選択中グローブIndex
    private Dictionary<GloveSide, int> m_gloveIndex;
    private const int GLOVE_MAX = 3;

    // 決定フラグ
    private bool m_isReady;

    // プロパティ
    public int PlayerId { get { return m_playerId; } set { m_playerId = value; } }
    public InputDevice InputDevice { get { return m_inputDevice; } set { m_inputDevice = value; } }
    public SelectPlayerInputReceiver InputReceiver {  get { return m_inputReceiver; } }
    public SelectPlayerUIManager UI { get { return m_uiManager; } }
    public int CharaIndex {  get { return m_charaIndex; } }
    public GloveSide CurrentGloveSide { get { return m_currentGloveSide; } set { m_currentGloveSide = value; } }
    public int GetGloveIndex(GloveSide side) {  return m_gloveIndex[side]; }
    public bool IsReady { get { return m_isReady; } set { m_isReady = value; } }


    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_charaIndex = 0;

        m_gloveIndex = new Dictionary<GloveSide, int>();

        m_gloveIndex[GloveSide.Left] = 0;
        m_gloveIndex[GloveSide.Right] = 0;

        m_isReady = false;
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // ステートマシン定義
        m_stateMachine = new StateMachine<SelectPlayer>(this);

        // 各ステート追加
        m_stateMachine.Add<CharaSelectState>((int)SelectPlayerState.CharaSelect);
        m_stateMachine.Add<GloveSelectState>((int)SelectPlayerState.GloveSelect);
        m_stateMachine.Add<PlayerReadySelectState>((int)SelectPlayerState.Ready);

        // ステートマシン開始
        m_stateMachine.OnStart((int)SelectPlayerState.CharaSelect);
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // ステート更新
        m_stateMachine.OnUpdate();
    }

    /*--------------------------------------------------------------------------------
　　|| UIと紐づけ
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// UIと紐づけ
    /// </summary>
    public void BindUI(SelectPlayerUIManager uiManager)
    {
        // UIの設定
        m_uiManager = uiManager;
        uiManager.Initialize(m_charaIndex);
    }

    /*--------------------------------------------------------------------------------
　　|| キャラIndexを変更
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラIndexを変更
    /// </summary>
    public void ChangeCharaIndex(int index)
    {
        m_charaIndex += index;
        m_charaIndex = Mathf.Clamp(m_charaIndex, 0, CHARA_MAX - 1);
        UI.ChangeCharaIndex(m_charaIndex);
    }

    /*--------------------------------------------------------------------------------
　　|| グローブIndexを変更
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// グローブIndexを変更
    /// </summary>
    public void ChangeGloveIndex(GloveSide gloveSide, int value)
    {
        m_gloveIndex[gloveSide] =
            (m_gloveIndex[gloveSide] + value + GLOVE_MAX) % GLOVE_MAX;
        UI.ChangeGloveIndex(gloveSide, m_gloveIndex[gloveSide]);
    }

    /*--------------------------------------------------------------------------------
    || キャラ決定処理（ランダム対応）
    --------------------------------------------------------------------------------*/
    public void DecideChara()
    {
        // ランダム（?）の場合
        if (m_charaIndex == CHARA_MAX - 1) // = 3
        {
            m_charaIndex = Random.Range(0, CHARA_MAX - 1); // 0?2
        }

        // UIにも「確定キャラ」を通知（表示差し替え用）
        UI.DecideCharaIndex(m_charaIndex);
    }

    /*--------------------------------------------------------------------------------
　　|| グローブの種類を取得
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// グローブの種類を取得
    /// </summary>
    /// <param name="side">左右</param>
    /// <returns></returns>
    public GloveType GetGloveType(GloveSide side)
    {
        int index = GetGloveIndex(side);
        return (GloveType)(index * 2 + (int)side);
    }
}