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
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
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

            var armPlayer = player.GetComponent<ArmPlayerController>();

            armPlayer.GetStretchArms()[(int)GloveSide.Left].ArmGloveType = m_playerGenertionInfos[i].GloveSet.Left;
            armPlayer.GetStretchArms()[(int)GloveSide.Right].ArmGloveType = m_playerGenertionInfos[i].GloveSet.Right;

            armPlayer.PlayerId = m_playerGenertionInfos[i].PlayerId;

            Debug.Log("プレイヤー" + player.playerIndex + ": デバイス"　 + player.devices[0]);
        }
    }
}
