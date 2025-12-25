using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackButton : MonoBehaviour
{
    [Header("フェードスクリプトがアタッチされたオブジェクト")]
    public UIFade fade;

    [Header("遷移先シーン名")]
    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {

    }


    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        // フェードアウト開始
        fade.FadeOutWithCallback(() =>
        {
            // フェード完了後にシーン遷移
            SceneManager.LoadScene(nextSceneName);
        });

    }

}
