/**********************************************************
 *
 *  SelectSceneManager.cs
 *  セレクトシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/16
 *
 *********************************************************/
using UnityEngine;




/// <summary>
/// セレクトシーンを管理
/// </summary>
public class SelectSceneManager : SingletonMonoBehaviour<SelectSceneManager> 
{
    // グローブの配列
    [SerializeField]
    private GameObject[] m_gloves;

    // キャラクターの配列
    [SerializeField]
    private GameObject[] m_characters;

    // フェード管理
    [SerializeField]
    private UIFade m_fade;

    // 二人分の選択データ

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_fade.FadeOutWithCallback(() =>
            {
                // ゲームスタート処理
                GameStart();
            });
        }
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
        var target = await SceneLoader.Load<PlaySceneManager>("PlayScene");

        // ターゲットを取得
        if (target == null)
        {
            Debug.LogError("PlayerManager がシーン内に見つかりませんでした。");
            return;
        }
    }

}
