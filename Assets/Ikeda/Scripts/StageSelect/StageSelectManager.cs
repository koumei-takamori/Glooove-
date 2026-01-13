// -------------------------------------------------------------
//
// StageSelectManager.cs
// ステージセレクト画面の管理を行うクラス
// 2026/01/10
// 池田
//
// --------------------------------------------------------------

using UnityEngine;

public enum StageID
{
    None = -1,
    Live = 0,
    Junk = 1,
    Street = 2,
    Random = 3
}

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    // *------------------:
    // ll 変数宣言 
    // *------------------:
    [SerializeField]
    private UIFade m_fade;

    // インゲームプレイヤーの生成情報
    private PlayerGenerationInfo[] m_playerGenerationInfos = default;

    // ステージ名
    [SerializeField]
    private string[] m_sceneName;

    // ステージ名配列
    [SerializeField]
    StageID m_selectStageId = StageID.None;

    // シーンロードフラグ
    bool m_isSceneLoad = false;


    // *------------------:
    // ll 関数宣言
    // *------------------:
    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        if (m_fade == null)
        {
            Debug.LogError("StageSelectManager : UIFade がInspectorに設定される");
        }
    }


    // ------------------------------------
    // ※ Todo : コントローラー対応
    // ------------------------------------

    private void Update()
    {
        // シーンロード中は操作を受け付けない
        if (m_isSceneLoad) return;

        // space か 右ボタンでステージ決定
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_isSceneLoad = true;
            LoadInGameScene();
        }


        // スティック左右でステージ選択
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            MoveStageSelect(1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            MoveStageSelect(-1);
        }


    }


    /// <summary>
    /// ステージ移動処理
    /// </summary>
    /// <param name="direction">移動方向</param>
    private void MoveStageSelect(int direction)
    {
        int stageCount = m_sceneName.Length;

        int nextIndex = ((int)m_selectStageId + direction + stageCount) % stageCount;
        m_selectStageId = (StageID)nextIndex;
    }


    /// <summary>
    /// シーン変更
    /// </summary>
    private void LoadInGameScene()
    {

        m_fade.FadeOutWithCallback(() =>
        {
            GameStart();
        });
    }


    /// <summary>
    /// PlaySceneにデータをわたす処理
    /// </summary>
    private async void GameStart()
    {
        // ランダムステージ選択時の処理
        if (m_selectStageId == StageID.Random)
        {
            // ランダム以外のステージをランダムで選択
            m_selectStageId = (StageID)Random.Range(0, m_sceneName.Length - 2);
        }


        var target = await SceneLoader.Load<PlayerGenerator>(m_sceneName[(int)m_selectStageId]);

        if (target == null)
        {
            Debug.LogError("PlayScene に PlayerGenerator が見つかりませんでした。");
            return;
        }

        target.SetGenerationInfo(m_playerGenerationInfos);
    }


    /// <summary>
    /// CharacterSelectManager から プレイヤー生成情報を受け取る
    /// </summary>
    /// <param name="playerData"></param>
    public void SetDataForStageSelect(PlayerGenerationInfo[] playerData)
    {
        m_playerGenerationInfos = playerData;

        Debug.Log("SelectSceneからデータを受け取りました");
    }

    // *------------------:
    // ll アクセサ
    // *------------------:
    public StageID SelectStage { get { return m_selectStageId; } }
}
