using Nakashi.Player;
using System.Collections.Generic;
using UnityEngine;

public class TimeUpState : IUIState
{
    // ステートタイプ
    private const PlayUIType stateType = PlayUIType.TimeUp;

    // コントローラー参照
    private readonly UIController controller;

    // TimeUpUI
    private readonly GameObject TimeUpUI;
    // アニメーター
    private Animator animator;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">コントローラー</param>
    public TimeUpState(UIController controller)
    {
        this.controller = controller;

        // TimeUpUIの取得
        TimeUpUI = GameObject.Find("UICanvas/TimeUpUI");
        if (TimeUpUI == null)
        {
            Debug.LogError("UICanvas/TimeUpUI が見つかりません。");
            return;
        }

        // アニメーターの取得
        animator = TimeUpUI.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator Component がありません。");
        }

        // TimeUpUIを非アクティブ(非表示)にする
        TimeUpUI.SetActive(false);
    }


    public void Enter()
    {
        // ステート開始時の処理
        Debug.Log("TimeUpState: Enter");

        // TimeUpUIをアクティブ(表示)にする
        TimeUpUI.SetActive(true);

        // アニメーションを再生
        animator.Play("TimeUp");

        // プレイヤーを操作不能にする
        List<GameObject> players = PlayerRegistry.Instance.GetAllPlayers();
        foreach (GameObject player in players)
        {
            ArmPlayerStatus status = player.GetComponent<ArmPlayerController>().GetPlayerStatus();
            status.CanStart = false;
            status.GetSetControll = true;

            Debug.Log("KOState: プレイヤーの操作を無効化 " + player.name);
        }


        // HPの残量で勝者を判定
        PlayerHP p1HP = players[0].GetComponent<PlayerHP>();
        PlayerHP p2HP = players[1].GetComponent<PlayerHP>();

        // 敗北者を取得（データ登録先でIndexを修正しているため）
        int loserIndex = -1;
        loserIndex = (p1HP.GetCurrentHP() <= p2HP.GetCurrentHP()) ? 0 : 1;

        PlaySceneWinnerDataSender.Instance.SaveWinnerPlayerData(loserIndex);
    }


    public void Update()
    {
        // ステート更新時の処理
        Debug.Log("TimeUpState: Update");
    }


    public void Exit()
    {
        // ステート終了時の処理
        Debug.Log("TimeUpState: Exit");
    }
}
