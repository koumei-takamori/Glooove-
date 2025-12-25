using UnityEngine;

public class InPlayHandle : MonoBehaviour
{
    // UIコントローラー
    public UIController controller;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        controller = GameObject.Find("UIController")?.GetComponent<UIController>();

        if (controller == null)
        {
            Debug.LogError("UIController が見つかりません。シーンにアサインされているか確認してください。");
        }
    }


    /// <summary>
    /// カウントダウン終了
    /// </summary>
    public void OnTimeUp()
    {
        // ステート変更
        controller.ChangeState(PlayUIType.TimeUp);
    }
}
