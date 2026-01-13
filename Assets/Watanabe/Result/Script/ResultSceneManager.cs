/**********************************************************
 *
 *  ResultSceneManager.cs
 *  リザルトシーンの管理クラス
 *
 *  制作者 : 渡邊　翔也
 *  制作日 : 2025/12/26
 *
 *********************************************************/
using UnityEngine;
using System.Collections;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private UIFade m_fade;

    [SerializeField, Header("勝ったキャラクター")]
    private WinnerCharacter m_character;

    [SerializeField, Header("使用したステージ")]
    private ResultStage m_resultStage;

    [SerializeField, Header("勝ったプレイヤー　１Pか２Pか")]
    private WinnerPlayer m_winnerPlayer;


    //勝利キャラクター
    private int m_winnerCharacterId;
    //勝利プレイヤー 1Pか2P
    private int m_winnerPlayerId;
    //ステージID
    private int m_stageId;
    // 追加：グローブ
    private GloveSet m_gloves;
    // 勝利データ
    private WinnerData winnerData;

    //シーン切り替えの有効無効
    private bool m_isChangeScene;

    // 追加：インプットレシーバー
    private ResultInputReceiver m_inputReceiver;

    // 追加：Exitが呼ばれたかどうか
    private bool m_isExitCalled = false;

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    void Awake()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // PlaySceneからデータを受け取れているか
        if (winnerData == null)

            Debug.Log("winner" + winnerData);
        //表示キャラクターの設定・変更
        m_character.winnerId = (int)winnerData.CharacterType;
        m_character.ChangeCharacter();

        //ステージの変更
        m_resultStage.ChangeStage(winnerData.StageId);

        //UI変更
        m_winnerPlayer.winnerPlayer = winnerData.PlayerId + 1;
        m_winnerPlayer.ChangeTextUI();

        m_isChangeScene = false;

        // 追加：インプットレシーバーの取得
        m_inputReceiver = GetComponent<ResultInputReceiver>();

        // 追加：リザルトBGM再生
        StartCoroutine(PlayBGMDelayed());
    }
    // 追加：BGM再生
    IEnumerator PlayBGMDelayed()
    {
        // 1フレーム待つ
        yield return null;

        SoundManager.Instance.PlayBGM("ResultBGM", true);
    }
    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //シーン切り替え
        ChangeScene();
    }

    //シーン切り替え
    private void ChangeScene()
    {
        //if (!m_isChangeScene)
        //{
        //    return;
        //}
        // Exitが呼ばれたらアプリケーション終了
        if (m_inputReceiver.GetInputButton(ResultInputReceiver.Actions.EXIT, ResultInputReceiver.InputType.PRESSED))
        {
            // Exitフラグを立てる
            m_isExitCalled = true;
            // キャンセル音再生
            SoundManager.Instance.PlaySE("Cancel");
            // ゲーム終了
            StartCoroutine(ExitGame(0.5f));
        }
        // Enterが呼ばれたらセレクトシーンに移行
        if (m_inputReceiver.GetInputButton(ResultInputReceiver.Actions.ENTER, ResultInputReceiver.InputType.PRESSED))
        {
            // Exitが呼ばれていなければタイトルに戻る
            if (m_isExitCalled) return;
            // 決定音再生
            SoundManager.Instance.PlaySE("Decide");
            // タイトルに戻る
            StartCoroutine(BackToTitle(0.5f));

        }

    }

    /// <summary>
    /// リザルトアニメーション終了時に呼び出す関数　シグナルで
    /// </summary>
    public void AnimationEnd()
    {

        m_isChangeScene = true;
    }


    /*--------------------------------------------------------------------------------
　　|| PlaySceneからのデータ受け取り処理
　　--------------------------------------------------------------------------------*/
    public void SetWinnerData(WinnerData data)
    {
        winnerData = data;

        m_winnerCharacterId = (int)winnerData.CharacterType;
        m_winnerPlayerId = winnerData.PlayerId;
        m_stageId = winnerData.StageId;
        m_gloves = winnerData.GloveSet;

        Debug.Log("Load完了");
    }

    // タイトルに戻る（遅延実行）
    private IEnumerator BackToTitle(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_fade.FadeOutWithCallback(() =>
        {
            // セレクトシーンに移行
            SceneLoader.Load("TitleScene");
        });
    }

    // 追加：ゲームそのものを終了する（遅延実行）
    private IEnumerator ExitGame(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_fade.FadeOutWithCallback(() =>
        {
            // アプリケーション終了
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        });
    }

    //勝利プレイヤーのゲットセット　１Pか２Pか
    public int WinerPlayerID { get { return m_winnerPlayerId; } set { m_winnerPlayerId = value; } }
    //ステージIDのゲットセット
    public int StageID { get { return m_stageId; } set { m_stageId = value; } }
    //勝利キャラクターのゲットセット
    public int WinnerCharacterID { get { return m_winnerCharacterId; } set { m_winnerCharacterId = value; } }

}
