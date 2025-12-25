using UnityEngine;

public class InPlayState : IUIState
{
    // ステートタイプ
    private const PlayUIType stateType = PlayUIType.InPlay;
    // コントローラー参照
    private readonly UIController controller;
    // InPlayUI
    private readonly GameObject inPlayUI;
    // アニメーター
    private Animator animator;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">コントローラー</param>
    public InPlayState(UIController controller)
    {
        this.controller = controller;

        // InPlayUIの取得
        inPlayUI = GameObject.Find("UICanvas/InPlayUI");
        if (inPlayUI == null)
        {
            Debug.LogError("UICanvas/InPlayUI が見つかりません。");
            return;
        }

        // アニメーターの取得
        animator = inPlayUI.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator Component がありません。");
        }

        // InPlayUIを非アクティブ(非表示)にする
        inPlayUI.SetActive(false);
    }


    public void Enter()
    {
        // ステート開始時の処理
        Debug.Log("InPlayState: Enter");

        // InPlayUIをアクティブ(表示)にする
        inPlayUI.SetActive(true);

        // アニメーションを再生
        animator.Play("StartCountDown");
    }


    public void Update()
    {
        // ステート更新時の処理
        Debug.Log("InPlayState: Update");
    }


    public void Exit()
    {
        // ステート終了時の処理
        Debug.Log("InPlayState: Exit");

        // InPlayUIを非アクティブ(非表示)にする
        inPlayUI.SetActive(false);
    }
}
