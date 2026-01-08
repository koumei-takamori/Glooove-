/**********************************************************
 *
 *  SelectSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.GridLayoutGroup;


/// <summary>
/// セレクトシーンを管理
/// </summary>
public class SelectSceneManager : SingletonMonoBehaviour<SelectSceneManager>
{
    // インゲームのプレイヤーの生成情報
    private PlayerGenerationInfo[] m_playerGenerationInfos = default;

    // フェード管理
    [SerializeField]
    private UIFade m_fade;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    override protected void Awake()
    {
        base.Awake();
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // 全プレイヤーが準備完了出ないなら処理しない
        if (!IsAllPlayerReady()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeOutWithCallback(() =>
            {
                // 生成情報を生成
                CreateGenerationInfos();
                // ゲームスタート処理
                GameStart();
            });
        }
    }

    /*--------------------------------------------------------------------------------
　　|| ゲームスタート処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ゲームスタート処理
    /// </summary>
    private async void GameStart()
    {
        // awaitしてシーンロード処理とPlayerManagerを取得
        var target = await SceneLoader.Load<PlayerGenerator>("PlaySceneLive");

        // ターゲットを取得
        if (target == null)
        {
            Debug.LogError("TStreetPlayScene がシーン内に見つかりませんでした。");
            return;
        }

        // 生成情報を格納
        target.SetGenerationInfo(m_playerGenerationInfos);
    }

    /*--------------------------------------------------------------------------------
　　|| 全プレイヤーが準備完了か
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 全プレイヤーが準備完了か
    /// </summary>
    private bool IsAllPlayerReady()
    {
        var players = SelectPlayerManager.Instance.Players;

        // まだ誰もいない
        if (players.Count == 0) return false;

        foreach (var player in players)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }
        return true;
    }

    /*--------------------------------------------------------------------------------
  　|| 生成情報を作成する
    --------------------------------------------------------------------------------*/
    /// <summary>
    /// 生成情報を作成する
    /// </summary>
    private void CreateGenerationInfos()
    {
        var players = SelectPlayerManager.Instance.Players;

        m_playerGenerationInfos = new PlayerGenerationInfo[players.Count];

        foreach (var player in players)
        {
            // 選択したグローブを生成用に構造体に格納
            GloveSet gloves = new GloveSet(
                player.GetGloveType(GloveSide.Left),
                player.GetGloveType(GloveSide.Right));

            m_playerGenerationInfos[player.PlayerId] =
                new PlayerGenerationInfo(
                    player.PlayerId,
                    player.InputDevice,
                    (CharacterType)player.CharaIndex,
                    gloves
                );
        }
    }
}
