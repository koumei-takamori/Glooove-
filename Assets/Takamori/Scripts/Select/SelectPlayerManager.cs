/**********************************************************
 *
 *  SelectPlayerManager.cs
 *  セレクトのプレイヤーを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectPlayerManager : SingletonMonoBehaviour<SelectPlayerManager>
{
    // プレイヤー
    private List<SelectPlayer> m_players = new List<SelectPlayer>();

    // 最大人数
    [SerializeField]
    private int m_maxPlayerCount = 2;

    // 各プレイヤーが操作するUIたち
    [SerializeField]
    private List<SelectPlayerUIManager> m_playerUIs = new List<SelectPlayerUIManager>();

    // インゲームのプレイヤーの生成情報
    private PlayerGenerationInfo[] m_playerGenerationInfos = default;


    // プロパティ
    public List<SelectPlayer> Players { get { return m_players; } }
    public PlayerGenerationInfo[] PlayerGenerationInfos { get { return m_playerGenerationInfos; } }

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    override protected void Awake()
    {
        base.Awake();
        m_playerGenerationInfos = new PlayerGenerationInfo[m_maxPlayerCount];
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤー接続処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤー接続処理
    /// </summary>
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // PlayerInputからSelectPlayerを取得
        SelectPlayer selectPlayer = playerInput.GetComponent<SelectPlayer>();

        // nullならエラー
        if (selectPlayer == null)
        {
            Debug.LogError("SelectPlayer が PlayerPrefab に存在しません");
            return;
        }

        // PlayerIDを設定
        selectPlayer.PlayerID = playerInput.playerIndex;

        // 選択したグローブを生成用に構造体に格納
        GloveSet gloves = new GloveSet(GloveType.Normal_L,GloveType.Normal_R);

        // プレイヤーを追加
        m_players.Add(selectPlayer);
        // UIと紐づけ
        m_playerUIs[playerInput.playerIndex].Bind(selectPlayer);

        // インゲームのプレイヤーの生成情報を格納
        m_playerGenerationInfos[playerInput.playerIndex] =
            new PlayerGenerationInfo(
                selectPlayer.PlayerID,
                playerInput.devices[0], 
                CharacterType.Balance,
                gloves
            );
    }
}
