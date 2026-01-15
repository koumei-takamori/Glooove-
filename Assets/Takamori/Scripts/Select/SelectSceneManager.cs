/**********************************************************
 *
 *  SelectSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static SelectPlayer;

/// <summary>
/// セレクトシーンを管理
/// </summary>
public class SelectSceneManager : SingletonMonoBehaviour<SelectSceneManager>
{
    /// <summary>
    /// セレクトシーンのプレイヤーステート
    /// </summary>
    public enum SelectState
    {
        PlayerSelect = 0,     
        StageSelect = 1,     
        Ready = 2       　　　
    }

    // ステートマシン
    private StateMachine<SelectSceneManager> m_stateMachine;

    // フェード管理
    [SerializeField]
    private UIFade m_fade;

    // GameReadyUI
    [SerializeField]
    private UIElement m_ready;

    // ステージセレクト管理クラス
    [SerializeField]
    private StageSelectManager m_stageSelectManager;

    // インゲームのプレイヤーの生成情報
    private PlayerGenerationInfo[] m_playerGenerationInfos = default;

    // プロパティ
    public UIElement ReadyUI { get { return m_ready; } }
    public StageSelectManager StageSelectManager { get { return m_stageSelectManager; } }   
    public SelectPlayerInputReceiver GetInput(int playerId) { return SelectPlayerManager.Instance.Players[playerId].InputReceiver; }

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
        // ステートマシン定義
        m_stateMachine = new StateMachine<SelectSceneManager>(this);

        // 各ステート追加
        m_stateMachine.Add<PlayerSelectState>((int)SelectState.PlayerSelect);
        m_stateMachine.Add<StageSelectState>((int)SelectState.StageSelect);
        m_stateMachine.Add<ReadySelectState>((int)SelectState.Ready);

        // ステートマシン開始
        m_stateMachine.OnStart((int)SelectPlayerState.CharaSelect);

        // 追加：BGM再生
        StartCoroutine(PlayBGMDelayed());
    }

    // 追加：BGM再生
    IEnumerator PlayBGMDelayed()
    {
        // 1フレーム待つ
        yield return null;

        SoundManager.Instance.PlayBGM("SelectBGM", true);
    }

    /*--------------------------------------------------------------------------------
     || 更新処理
     --------------------------------------------------------------------------------*/
    private void Update()
    {
        // ステート更新
        m_stateMachine.OnUpdate();
    }

    // 追加：PlaySceneへ移動する処理
    public IEnumerator EnterToPlayScene(float duration)
    {
        SoundManager.Instance.PlaySE("GameStart");
        VibrateGamepad(0.05f, 1.0f);

        yield return new WaitForSeconds(duration);
        // フェード処理
        m_fade.FadeOutWithCallback(() =>
        {
            CreateGenerationInfos();
            GameStart();
        });
    }

    /*--------------------------------------------------------------------------------
     || ゲームスタート処理
     --------------------------------------------------------------------------------*/
    public async void GameStart()
    {
        var target = await SceneLoader.Load<PlayerGenerator>("PlayScene" + m_stageSelectManager.GetStageNameByID(m_stageSelectManager.StageID));

        if (target == null)
        {
            Debug.LogError("PlayScene に PlayerGenerator が見つかりませんでした。");
            return;
        }

        target.SetGenerationInfo(m_playerGenerationInfos);
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
    // 振動させるメソッド
    private void VibrateGamepad(float duration = 0.4f, float power = 1.0f)
    {
        foreach (var player in SelectPlayerManager.Instance.Players)
        {
            if (player.InputDevice is Gamepad gamepad)
            {
                // 振動開始
                gamepad.SetMotorSpeeds(power, power);
                // 一定時間後に停止
                StartCoroutine(StopVibration(gamepad, duration));
            }
        }

    }
    private IEnumerator StopVibration(Gamepad gamepad, float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}