/**********************************************************
 *
 *  PlayerGenerator.cs
 *  プレイヤーの生成
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

/// <summary>
/// プレイヤーの生成
/// </summary>
public class PlayerGenerator : MonoBehaviour
{
    // キャラのPrefab
    [SerializeField] private GameObject m_balance = default;
    [SerializeField] private GameObject m_speed = default;
    [SerializeField] private GameObject m_tank = default;

    // 生成情報
    private PlayerGenerationInfo[] m_playerGenertionInfos = default;

    /// <summary>
    /// 生成情報の取得
    /// Awakeの後、Startの前で設定
    /// </summary>
    /// <param name="playerGenerationInfo"></param>
    public void SetGenerationInfo(PlayerGenerationInfo[] playerGenerationInfo)
    {
        m_playerGenertionInfos = playerGenerationInfo;
        // 例えばここで生成
        CreateCharacter();
    }

    /// <summary>
    /// プレイヤーの生成
    /// </summary>
    private void CreateCharacter() 
    {
        for (int i = 0; i < m_playerGenertionInfos.Length; i++)
        {
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

            // 登録
            PlayerRegistry.Instance.RegisterPlayer(player.gameObject);
        }
    }
}
