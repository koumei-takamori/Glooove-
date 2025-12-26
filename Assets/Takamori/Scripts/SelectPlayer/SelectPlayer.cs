/**********************************************************
 *
 *  SelectPlayer.cs
 *  セレクトシーンのプレイヤー
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
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

    // 選択中グローブIndex
    private int m_gloveIndex;
    private const int GLOVE_MAX = 3;

    // 決定フラグ
    private bool m_isReady;


    // 入力クールタイム
    private float m_inputCoolTime = 0.2f;
    private float m_inputTimer = 0.0f;
    // 前フレームの入力
    private float m_prevHorizontal;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_charaIndex = 0;
        m_gloveIndex = 0;
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
        // 入力のインターバルを作成
        m_inputTimer -= Time.deltaTime;
        // 前回入力を保存
        m_prevHorizontal = Input.GetAxisRaw("Horizontal");

        // ステート更新
        m_stateMachine.OnUpdate();
    }

    /*--------------------------------------------------------------------------------
　　|| 入力関連
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 横入力取得（キャラ選択用）
    /// </summary>
    public int GetHorizontalOnce()
    {
        if (m_inputTimer > 0.0f) return 0;

        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0.5f)
        {
            m_inputTimer = m_inputCoolTime;
            return 1;
        }
        else if (h < -0.5f)
        {
            m_inputTimer = m_inputCoolTime;
            return -1;
        }

        return 0;
    }

    /// <summary>
    /// 縦入力（1回のみ）
    /// </summary>
    public int GetVerticalOnce()
    {
        if (m_inputTimer > 0) return 0;

        float v = Input.GetAxisRaw("Vertical");
        if (v > 0.5f)
        {
            m_inputTimer = m_inputCoolTime;
            return 1;
        }
        else if (v < -0.5f)
        {
            m_inputTimer = m_inputCoolTime;
            return -1;
        }
        return 0;
    }


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

    /// <summary>
    /// 選択中キャラIndex取得
    /// </summary>
    public int GetCharaIndex()
    {
        return m_charaIndex;
    }


    /*--------------------------------------------------------------------------------
　　|| グローブ選択関連
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// グローブIndexを変更
    /// </summary>
    public void AddGloveIndex(int value)
    {
        m_gloveIndex += value;
    }

    /// <summary>
    /// 選択中グローブIndex取得
    /// </summary>
    public int GetGloveIndex()
    {
        return m_gloveIndex;
    }

    /// <summary>
    /// 準備完了設定
    /// </summary>
    public void SetReady(bool ready)
    {
        m_isReady = ready;
    }

    /// <summary>
    /// 準備完了か
    /// </summary>
    public bool IsReady()
    {
        return m_isReady;
    }
}