using UnityEngine;

public class ResultSceneChanger : MonoBehaviour
{
    UIFade fadeSystem;

    void Start()
    {
        fadeSystem = GameObject.Find("UICanvas").transform.Find("Fade").GetComponent<UIFade>();

        if (fadeSystem == null)
        {
            Debug.LogError("Fade System not found!");
        }
    }

    public void ChangeScene()
    {
        fadeSystem.FadeOutWithCallback(() =>
        {
            // 結果シーンへ移動 データも渡す
            PlaySceneWinnerDataSender.Instance.SendPlaySecneWinnerData();
        });
    }

}
