/**********************************************************
 *
 *  SelectSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

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

    // GameReady
    [SerializeField]
    private UIElement m_ready;

    bool m_isSceneLoad = false;

    /*--------------------------------------------------------------------------------
     || 実行前初期化処理
     --------------------------------------------------------------------------------*/
    protected override void Awake()
    {
        base.Awake();
    }

    /*--------------------------------------------------------------------------------
     || 初期化処理
     --------------------------------------------------------------------------------*/
    private void Start()
    {
    }

    /*--------------------------------------------------------------------------------
     || 更新処理
     --------------------------------------------------------------------------------*/
    private void Update()
    {
        bool allReady = IsAllPlayerReady();
        m_ready.Animator.SetBool("GameReady", IsAllPlayerReady());

        if (!allReady || m_isSceneLoad) return;

        if (SelectPlayerManager.Instance.Players[0].InputReceiver.GetInputButton(
            SelectPlayerInputReceiver.SelectPlayerActions.Decide, SelectPlayerInputReceiver.InputType.PRESSED) ||
            SelectPlayerManager.Instance.Players[1].InputReceiver.GetInputButton(
            SelectPlayerInputReceiver.SelectPlayerActions.Decide, SelectPlayerInputReceiver.InputType.PRESSED) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            // シーンをロードし始める
            m_isSceneLoad = true;
            // フェード処理
            m_fade.FadeOutWithCallback(() =>
            {
                CreateGenerationInfos();
                GameStart();
            });
        }
    }

    /*--------------------------------------------------------------------------------
     || ゲームスタート処理
     --------------------------------------------------------------------------------*/
    private async void GameStart()
    {
        var target = await SceneLoader.Load<PlayerGenerator>("PlaySceneLive");

        if (target == null)
        {
            Debug.LogError("PlaySceneLive に PlayerGenerator が見つかりませんでした。");
            return;
        }

        target.SetGenerationInfo(m_playerGenerationInfos);
    }

    /*--------------------------------------------------------------------------------
     || 全プレイヤーが準備完了か
     --------------------------------------------------------------------------------*/
    private bool IsAllPlayerReady()
    {
        var players = SelectPlayerManager.Instance.Players;

        if (players.Count < 2) return false;

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
    private void CreateGenerationInfos()
    {
        var players = SelectPlayerManager.Instance.Players;

        m_playerGenerationInfos = new PlayerGenerationInfo[players.Count];

        foreach (var player in players)
        {
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