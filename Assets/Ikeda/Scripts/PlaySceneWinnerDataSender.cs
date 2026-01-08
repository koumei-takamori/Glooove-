// ------------------------------------------
//
// WinnerDataSender.cs
// プレイシーンのデータをリザルトシーンに送る
// キャラ/勝利プレイヤー/ステージ/グローブ
// 2026/01/08
// 池田桜輔
// 
// 参考：SelectSceneManager.cs / GameStart()
// 送り先：ResultSceneManager.cs
// ------------------------------------------

using UnityEngine;

public class PlaySceneWinnerDataSender : SingletonMonoBehaviour<PlaySceneWinnerDataSender>
{
    [SerializeField] private PlayerGenerator playerGenerator;
    [SerializeField] private int stageId = -1;
    [SerializeField] private WinnerData winnerData = null;

    [SerializeField] private PlayerGenerationInfo[] playerGenerationInfos = null;

    override protected void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        // ステージIDの確認
        if (stageId == -1)
        {
            Debug.LogError("ステージIDが設定されていません。 PlaySceneWinnerDataSender.cs");
        }
    }

    /// <summary>
    /// 勝利したプレイヤーを判別してデータを保存する
    /// </summary>
    /// <param name="loserId">「idは 0 か 1」</param>
    public void SaveWinnerPlayerData(int loserId)
    {
        int winnerId = 1 - loserId;

        // 勝利したプレイヤーの生成情報を取得
        var winnerGenerationInfo = playerGenerationInfos[winnerId];

        // 勝利データを作成
        winnerData = new WinnerData(
                winnerGenerationInfo.PlayerId,
                winnerGenerationInfo.SelectedCharacter,
                winnerGenerationInfo.GloveSet,
                stageId
             );

        // winnerDataのデータ全てにnullがないかをチェック
        if (winnerData == null)
        {
            Debug.LogError("WinnerDataの作成に失敗しました。 PlaySceneWinnerDataSender.cs");
            return;
        }
    }

    public async void SendPlaySecneWinnerData()
    {
        var resultSceneManager = await SceneLoader.Load<ResultSceneManager>("3DResultScene");

        if (resultSceneManager == null)
        {
            Debug.LogError("ResultSceneManagerがシーン内に見つかりませんでした。");
            return;
        }

        // データを送信
        resultSceneManager.SetWinnerData(winnerData);
    }

    // -------------------------------
    // プロパティ
    // -------------------------------
    public PlayerGenerationInfo[] PlayerGenerationInfos
    {
        get { return playerGenerationInfos; }
        set { playerGenerationInfos = value; }
    }
}


/// <summary>
/// 送るデータクラス
/// </summary>
public class WinnerData
{
    // プレイヤーID
    public int PlayerId { get; private set; } = default;
    // キャラクタータイプ
    public CharacterType CharacterType { get; private set; } = default;
    // 使用グローブ（両手）
    public GloveSet GloveSet { get; private set; } = default;
    // ステージID
    public int StageId { get; private set; } = default;

    public WinnerData(int playerId, CharacterType characterType, GloveSet gloveSet, int stageId)
    {
        PlayerId = playerId;
        CharacterType = characterType;
        GloveSet = gloveSet;
        StageId = stageId;
    }
}
