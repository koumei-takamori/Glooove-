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

    // READY UI
    [SerializeField]
    private Image m_ready;

    // 未接続時のUI
    [SerializeField]
    private Image m_UI;

    // Ready演出用
    private RectTransform m_readyRect;
    private bool m_isReadyShown = false;

    // Ready Tween 管理
    private Tween m_readyTween;

    [Header("Ready Slide Settings")]
    [SerializeField]
    private float m_readySlideOffset = 1920f;
    [SerializeField]
    private float m_readySlideTime = 0.2f;

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
        m_readyRect = m_ready.rectTransform;
    }

    /*--------------------------------------------------------------------------------
     || 更新処理
     --------------------------------------------------------------------------------*/
    private void Update()
    {
        bool allReady = IsAllPlayerReady();

        // -------- Ready 表示 --------
        if (allReady && !m_isReadyShown)
        {
            ShowReadyUI();
            m_isReadyShown = true;
        }
        // -------- Ready 解除（キャンセル）--------
        else if (!allReady && m_isReadyShown)
        {
            HideReadyUI();
            m_isReadyShown = false;
        }

        // 全員準備完了時のみ次へ進める
        if (!allReady) return;



        if (SelectPlayerManager.Instance.Players[0].InputReceiver.GetInputButton(
            SelectPlayerInputReceiver.SelectPlayerActions.Decide, SelectPlayerInputReceiver.InputType.PRESSED) ||
            SelectPlayerManager.Instance.Players[1].InputReceiver.GetInputButton(
            SelectPlayerInputReceiver.SelectPlayerActions.Decide, SelectPlayerInputReceiver.InputType.PRESSED) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeOutWithCallback(() =>
            {
                CreateGenerationInfos();
                GameStart();
            });
        }
    }

    /*--------------------------------------------------------------------------------
     || Ready UI 表示
     --------------------------------------------------------------------------------*/
    private void ShowReadyUI()
    {
        m_ready.gameObject.SetActive(true);

        m_readyTween?.Kill();

        Vector2 targetPos = new Vector2(0, 0);

        m_readyTween = m_readyRect
            .DOAnchorPos(targetPos, m_readySlideTime)
            .SetEase(Ease.InCirc);
    }

    /*--------------------------------------------------------------------------------
     || Ready UI 非表示（キャンセル時）
     --------------------------------------------------------------------------------*/
    private void HideReadyUI()
    {
        m_readyTween?.Kill();

        Vector2 hidePos =
            m_readyRect.anchoredPosition + Vector2.right * m_readySlideOffset;

        m_readyTween = m_readyRect
            .DOAnchorPos(hidePos, m_readySlideTime * 0.8f)
            .SetEase(Ease.InCirc);
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