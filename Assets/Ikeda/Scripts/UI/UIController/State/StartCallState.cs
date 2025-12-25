using UnityEngine;

public class StartCallState : IUIState
{
    // ステートタイプ
    private const PlayUIType stateType = PlayUIType.StartCall;

    // コントローラー参照
    private readonly UIController controller;

    // startCallUI
    private readonly GameObject startCallUI;

    // アニメーター
    private Animator animator;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">コントローラー</param>
    public StartCallState(UIController controller)
    {
        // コントローラーの参照を保存
        this.controller = controller;

        // startCallUIの取得
        startCallUI = GameObject.Find("UICanvas/StartCallUI");
        if (startCallUI == null)
        {
            Debug.LogError("UICanvas/StartCallUI が見つかりません。");
            return;
        }

        // アニメーターの取得
        animator = startCallUI.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator Component がありません。");
        }
    }


    public void Enter()
    {
        // ステート開始時の処理
        Debug.Log("StartCallState: Enter");

        // startCallUIをアクティブ(表示)にする
        startCallUI.SetActive(true);

        // アニメーションを再生
        animator.Play("GameStartMotion");
    }


    public void Update()
    {
        // ステート更新時の処理
        Debug.Log("StartCallState: Update");
        // ここにステート更新時の処理を追加
    }


    public void Exit()
    {
        // ステート終了時の処理
        Debug.Log("StartCallState: Exit");

        // startCallUIをアクティブ(表示)にする
        startCallUI.SetActive(false);
    }
}
