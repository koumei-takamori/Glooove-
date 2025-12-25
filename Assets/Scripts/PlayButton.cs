using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [Header("フェードスクリプトがアタッチされたオブジェクト")]
    public UIFade fade;

    [Header("遷移先シーン名")]
    public string nextSceneName = "SelectScene";

    void Start()
    {
        // もし非表示にしておきたい場合
    }

    public void OnClick()
    {
        Debug.Log("押された!");



        // フェードアウト開始
        fade.FadeOutWithCallback(() =>
        {
            // フェード完了後にシーン遷移
            SceneManager.LoadScene(nextSceneName);
        });

        Debug.Log("FadeOut!");
    }
}