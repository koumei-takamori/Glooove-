using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    UIFade fadeSystem;

    [SerializeField] string nextSceneName = "SelectScene";

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
            SceneLoader.Load(nextSceneName);
        });
    }

}
