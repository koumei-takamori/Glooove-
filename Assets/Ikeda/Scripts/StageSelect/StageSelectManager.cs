// -----------------------------------------------------
// --------
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

public enum MoveStageDirection
{
    Left = -1,
    Right = 1
}

public class StageSelectManager : SingletonMonoBehaviour<StageSelectManager>
{
    // *------------------:
    // ll 変数宣言 
    // *------------------:
    // ステージ名
    [SerializeField]
    private string[] m_sceneName;

    // ステージ名配列
    [SerializeField]
    StageID m_selectStageId = StageID.None;

    // ステージセレクトUI
    [SerializeField]
    private StageSelectUI m_stageSelectUI;

    // アクティブフラグ
    private bool m_isActive;

    // 決定フラグ
    private bool m_isDecide;

    // プロパティ
    public StageID StageID { get { return m_selectStageId; } }
    public bool IsDecide {  get { return m_isDecide; } }

    public string GetStageNameByID(StageID stageID) { return m_sceneName[(int)stageID]; }
    // *------------------:
    // ll 関数宣言
    // *------------------:
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ステージ移動処理
    /// </summary>
    /// <param name="direction">移動方向</param>
    public void MoveStageSelect(int direction)
    {
        int stageCount = m_sceneName.Length;

        int currentIndex =
            m_selectStageId == StageID.None ? 0 : (int)m_selectStageId;

        int nextIndex = (currentIndex + direction + stageCount) % stageCount;
        m_selectStageId = (StageID)nextIndex;

        m_stageSelectUI.ChangeStage(direction);

        Debug.Log("ステージ" + m_selectStageId);
    }

    public void IsActive(bool isActive)
    {
        m_isActive = isActive;
        m_stageSelectUI.gameObject.SetActive(isActive);
    }

    public void Decide()
    {
        m_isDecide = true;
        // ランダムステージ選択時の処理
        if (m_selectStageId == StageID.Random)
        {
            m_selectStageId = (StageID)Random.Range(
       (int)StageID.Live,
       (int)StageID.Random
   );
            //// ランダム以外のステージをランダムで選択
            //m_selectStageId = (StageID)Random.Range(0, m_sceneName.Length - 2);
        }

        Debug.Log("ステージ決定" + m_selectStageId);
    }

    public void Cancel()
    {
        m_isDecide = false;
    }
}
