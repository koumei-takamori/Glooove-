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

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    void Awake()
    {
        //デバック
        m_winnerCharacterId = 2;
        m_stageId = 2;
        m_winnerPlayerId = 1;
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

            //表示キャラクターの設定・変更
            m_character.winnerId = m_winnerCharacterId;
        m_character.ChangeCharacter();

        //ステージの変更
        m_resultStage.ChangeStage(m_stageId);

        //UI変更
        m_winnerPlayer.winnerPlayer = m_winnerPlayerId;
        m_winnerPlayer.ChangeTextUI();

        m_isChangeScene = false;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeOutWithCallback(() =>
            {
                // セレクトシーンに移行
                SceneLoader.Load("TitleScene");
            });

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




    //勝利プレイヤーのゲットセット　１Pか２Pか
    public int WinerPlayerID { get { return m_winnerPlayerId; } set { m_winnerPlayerId = value; } }
    //ステージIDのゲットセット
    public int StageID { get { return m_stageId; } set { m_stageId = value; } }
    //勝利キャラクターのゲットセット
    public int WinnerCharacterID { get { return m_winnerCharacterId; } set { m_winnerCharacterId = value; } }

}
