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
