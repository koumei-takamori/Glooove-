/**********************************************************
 *
 *  PlayerGenerator.cs
 *  プレイヤーの生成
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using Nakashi.Player;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの生成
/// </summary>
public class PlayerGenerator : MonoBehaviour
{
    // キャラのPrefab
    [SerializeField] private GameObject m_balance = default;
    [SerializeField] private GameObject m_speed = default;
    [SerializeField] private GameObject m_tank = default;
    // 追加：グローブリストデータ
    [SerializeField] private GloveListData m_gloveListData = default;
    // 追加：プレイヤーの生成地点
    [SerializeField] private Transform[] m_playerSpawnPoints = default;

    // 生成情報
    private PlayerGenerationInfo[] m_playerGenertionInfos = default;

    private void Start()
    {
        // 例えばここで生成
        CreateCharacter();
    }

    /// <summary>
    /// 生成情報の取得
    /// Awakeの後、Startの前で設定
    /// </summary>
    /// <param name="playerGenerationInfo"></param>
    public void SetGenerationInfo(PlayerGenerationInfo[] playerGenerationInfo)
    {
        m_playerGenertionInfos = playerGenerationInfo;
    }

    public void SetStageID(int id)
    {
        PlaySceneWinnerDataSender.Instance.StageID = id;
    }

    /// <summary>
    /// プレイヤーの生成
    /// </summary>
    private void CreateCharacter()
    {
        for (int i = 0; i < m_playerGenertionInfos.Length; i++)
        {
            if (m_playerGenertionInfos[i] == null) break;

            GameObject character = default;

            switch (m_playerGenertionInfos[i].SelectedCharacter)
            {
                case CharacterType.Balance:
                    character = m_balance;
                    break;

                case CharacterType.Speed:
                    character = m_speed;
                    break;

                case CharacterType.Tank:
                    character = m_tank;
                    break;
            }

            var player = PlayerInput.Instantiate(
            prefab: character,
            playerIndex: i,
            pairWithDevice: m_playerGenertionInfos[i].PairWithDevice
            );



            // 追加：生成地点に移動
            player.transform.position = m_playerSpawnPoints[i].position;

            var armPlayer = player.GetComponent<ArmPlayerController>();
            // グローブの設定
            // 通常時の腕に着けるグローブを設定
            armPlayer.SelectedLGlove = m_gloveListData.GetGlove(m_playerGenertionInfos[i].GloveSet.Left);
            armPlayer.SelectedRGlove = m_gloveListData.GetGlove(m_playerGenertionInfos[i].GloveSet.Right);

            // のびる腕のグローブタイプを設定
            armPlayer.GetStretchArms()[(int)GloveSide.Left].ArmGloveType = m_playerGenertionInfos[i].GloveSet.Left;
            armPlayer.GetStretchArms()[(int)GloveSide.Right].ArmGloveType = m_playerGenertionInfos[i].GloveSet.Right;

            armPlayer.PlayerId = m_playerGenertionInfos[i].PlayerId;
            // 追加：プレイヤーの操作デバイスをArmPlayerControllerに設定
            armPlayer.PlayerInputDevice = m_playerGenertionInfos[i].PairWithDevice;
            Debug.Log("プレイヤー" + player.playerIndex + ": デバイス" + player.devices[0]);
        }

        PlaySceneManager.Instance.SetTarget();
        // 追加：生成情報を送信
        PlaySceneWinnerDataSender.Instance.PlayerGenerationInfos = m_playerGenertionInfos;


    }
}
