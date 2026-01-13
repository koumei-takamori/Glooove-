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
using System.Collections;


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

    // 追加：Exitが呼ばれたかどうか
    private bool m_isExitCalled = false;

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
        // フェードイン処理
        if (!m_fadeInFlag)
        {
            m_fade.FadeInWithCallback(() =>
            {
                SoundManager.Instance.PlaySE("First");
                SoundManager.Instance.PlayBGM("TitleBGM", true);
                m_fadeInFlag = true;
            });
        }
        // Exitが呼ばれたらアプリケーション終了
        if (m_inputReceiver.GetInputButton(TitleInputReceiver.Actions.EXIT, TitleInputReceiver.InputType.PRESSED))
        {
            // 二重呼び出し防止
            if (m_isExitCalled) return;
            // Exitフラグを立てる
            m_isExitCalled = true;
            // キャンセル音再生
            SoundManager.Instance.PlaySE("Cancel");
            // アプリケーション終了
            StartCoroutine(ExitGame(0.5f));
        }
        // Enterが呼ばれたらセレクトシーンに移行
        if (m_inputReceiver.GetInputButton(TitleInputReceiver.Actions.ENTER, TitleInputReceiver.InputType.PRESSED))
        {
            // 二重呼び出し防止
            if (m_isExitCalled) return;
            // ボタン音再生
            SoundManager.Instance.PlaySE("PushButton");
            // セレクトシーンに移行
            StartCoroutine(EnterToSelectScene(0.5f));
        }

    }
    // 追加：ゲームそのものを終了する（遅延実行）
    private IEnumerator ExitGame(float duration)
    {
        yield return new WaitForSeconds(duration);
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
    // 追加：セレクト画面に移動（遅延実行）
    private IEnumerator EnterToSelectScene(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_fade.FadeOutWithCallback(() =>
        {
            // セレクトシーンに移行
            SceneLoader.Load("SelectScene");
        });
    }
}
