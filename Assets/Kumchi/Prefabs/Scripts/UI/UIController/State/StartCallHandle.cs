// -----------------------------------------------------------
// アニメーション終了をコントローラーに渡すためのクラス

// -----------------------------------------------------------

using UnityEngine;

public class StartCallHandle : MonoBehaviour
{
    public UIController controller;

    private void Awake()
    {
        controller = GameObject.Find("UIController")?.GetComponent<UIController>();

        if (controller == null)
        {
            Debug.LogError("UIController が見つかりません。シーンにアサインされているか確認してください。");
        }
    }


    // StartCollUIのアニメーション終了時に呼ばれる
    public void OnStartCollUIAnimationComplete()
    {
        if (controller != null)
        {
            controller.ChangeState(PlayUIType.InPlay);
        }
        else
        {
            Debug.LogWarning("UIController がアサインされていません");
        }
    }
}
