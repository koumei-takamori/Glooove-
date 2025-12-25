using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [Header("フェードスクリプトがアタッチされたオブジェクト")]
    public UIFade fade;

    [Header("遷移先シーン名")]
    public string nextSceneName = "SelectScene";

    public bool isExitApp = false; // true=アプリ終了, false=シーン遷移

    void Start()
    {
    }

    public void OnClick()
    {
        Debug.Log("押された!");



        fade.FadeOutWithCallback(() =>
        {
            if (isExitApp)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        });

        Debug.Log("FadeOut!");
    }
}