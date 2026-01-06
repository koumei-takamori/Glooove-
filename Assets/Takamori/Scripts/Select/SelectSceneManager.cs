/**********************************************************
 *
 *  SelectSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// セレクトシーンを管理
/// </summary>
public class SelectSceneManager : SingletonMonoBehaviour<SelectSceneManager> 
{
    // フェード管理
    [SerializeField]
    private UIFade m_fade;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    override protected void Awake()
    {
        base.Awake();
    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    m_fade.FadeOutWithCallback(() =>
        //    {
        //        // ゲームスタート処理
        //        GameStart();
        //    });
        //}
    }

    /*--------------------------------------------------------------------------------
　　|| ゲームスタート処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ゲームスタート処理
    /// </summary>
    private async void GameStart()
    {
        // awaitしてシーンロード処理とPlayerManagerを取得
        var target = await SceneLoader.Load<PlayerGenerator>("TStreetPlayScene");

        // ターゲットを取得
        if (target == null)
        {
            Debug.LogError("PlayerManager がシーン内に見つかりませんでした。");
            return;
        }

        // 生成情報を格納
        target.SetGenerationInfo(SelectPlayerManager.Instance.PlayerGenerationInfos);
    }
}
