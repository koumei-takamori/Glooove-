/**********************************************************
 *
 *  SelectPlayer.cs
 *  セレクトシーンのプレイヤー
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using Nakashi.Player;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セレクトシーンのプレイヤー
/// </summary>
public class SelectPlayer : MonoBehaviour
{
    /// <summary>
    /// セレクトシーンのプレイヤーステート
    /// </summary>
    public enum SelectPlayerState
    {
        CharaSelect = 0,      // キャラ選択状態
        GloveSelect = 1,      // グローブ選択状態
        Ready = 2       // 準備確認状態
    }

    // ステートマシン
    private StateMachine<SelectPlayer> m_stateMachine;

    // プレイヤーID
    [SerializeField]
    private int m_playerID;

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
    public int PlayerID {  get { return m_playerID; } set { m_playerID = value; } }
    public int CharaIndex {  get { return m_charaIndex; } }
    public GloveSide CurrentGloveSide { get { return m_currentGloveSide; } set { m_currentGloveSide = value; } }
    public int GetGloveIndex(GloveSide side) {  return m_gloveIndex[side]; }
    public bool IsReady { get { return m_isReady; } }


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
        m_stateMachine.Add<ReadySelectState>((int)SelectPlayerState.Ready);

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
　　|| 入力関連
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 決定入力
    /// </summary>
    public bool IsDecide()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    /// <summary>
    /// キャンセル入力
    /// </summary>
    public bool IsCancel()
    {
        return Input.GetKeyDown(KeyCode.Backspace);
    }

    /*--------------------------------------------------------------------------------
　　|| キャラ選択関連
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// キャラIndexを変更
    /// </summary>
    public void AddCharaIndex(int value)
    {
        m_charaIndex += value;
        m_charaIndex = Mathf.Clamp(m_charaIndex, 0, CHARA_MAX - 1);
    }

    /*--------------------------------------------------------------------------------
　　|| グローブ選択関連
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// グローブIndexを変更
    /// </summary>
    public void AddGloveIndex(GloveSide gloveSide, int value)
    {
        m_gloveIndex[gloveSide] =
            (m_gloveIndex[gloveSide] + value + GLOVE_MAX) % GLOVE_MAX;
    }  

    /// <summary>
    /// 準備完了設定
    /// </summary>
    public void SetReady(bool ready)
    {
        m_isReady = ready;
    }
}