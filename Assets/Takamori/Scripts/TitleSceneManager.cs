/**********************************************************
 *
 *  TitleSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using Nakashi.Player;
using UnityEngine;

/// <summary>
/// セレクトシーンを管理
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private UIFade m_fade;

    private bool m_fadeInFlag;

    // 追加：タイトルインプットレシーバー
    private TitleInputReceiver m_inputReceiver;

    /*--------------------------------------------------------------------------------
　　|| 実行前処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前処理
    /// </summary>
    private void Awake()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        m_fadeInFlag = false;
        // 追加：インプットレシーバーの取得
        m_inputReceiver = GetComponent<TitleInputReceiver>();


    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (!m_fadeInFlag)
        {
            m_fade.FadeInWithCallback(() =>
            {
                SoundManager.Instance.PlaySE("First");
                SoundManager.Instance.PlayBGM("TitleBGM", true);
                m_fadeInFlag = true;
            });
        }

        // Enterが呼ばれたらセレクトシーンに移行
        if (m_inputReceiver.GetInputButton(TitleInputReceiver.Actions.ENTER, TitleInputReceiver.InputType.PRESSED))
        {
            SoundManager.Instance.PlaySE("PushButton");
            m_fade.FadeOutWithCallback(() =>
            {
                // セレクトシーンに移行
                SceneLoader.Load("SelectScene");
            });

        }
        // Exitが呼ばれたらアプリケーション終了
        if (m_inputReceiver.GetInputButton(TitleInputReceiver.Actions.EXIT, TitleInputReceiver.InputType.PRESSED))
        {
            SoundManager.Instance.PlaySE("PushButton");
            m_fade.FadeOutWithCallback(() =>
            {
                // アプリケーション終了
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }

}
