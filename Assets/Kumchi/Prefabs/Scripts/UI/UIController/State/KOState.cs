using UnityEngine;

public class KOState : IUIState
{
    // ステートタイプ
    private const PlayUIType stateType = PlayUIType.KO;

    // コントローラー参照
    private readonly UIController controller;

    // KOUI
    private readonly GameObject KOUI;
    // アニメーター
    private Animator animator;


    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">コントローラー</param>
    public KOState(UIController controller)
    {
        this.controller = controller;

        // KOUIの取得
        KOUI = GameObject.Find("UICanvas/KOUI");
        if (KOUI == null)
        {
            Debug.LogError("UICanvas/KOUI が見つかりません。");
            return;
        }

        // アニメーターの取得
        animator = KOUI.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator Component がありません。");
        }

        // KOUIを非アクティブ(非表示)にする
        KOUI.SetActive(false);
    }


    public void Enter()
    {
        // ステート開始時の処理
        Debug.Log("KOState: Enter");

        // KOUIをアクティブ(表示)にする
        KOUI.SetActive(true);

        // アニメーションを再生
        //animator.Play("KO");

    }


    public void Update()
    {
        // ステート更新時の処理
        Debug.Log("KOState: Update");
    }


    public void Exit()
    {
        // ステート終了時の処理
        Debug.Log("KOState: Exit");
    }
}
