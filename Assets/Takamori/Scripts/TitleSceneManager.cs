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
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if(!m_fadeInFlag) {
            m_fade.FadeInWithCallback(() =>
            {
                SoundManager.Instance.PlaySE("First");
                SoundManager.Instance.PlayBGM("TitleBGM");
                m_fadeInFlag=true;
            });
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.PlaySE("PushButton");
            m_fade.FadeOutWithCallback(() =>
            {
                // セレクトシーンに移行
                SceneLoader.Load("SelectScene");
            });

        }
    }

}
