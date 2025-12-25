/**********************************************
 * 
 *  SceneLoader.cs 
 *  シーンをロードするクラス
 *  ロード先のシーンのコンポーネントを取得し値を渡すことができる
 *  指定コンポ―ネントはAwakeの後、Startの前の取得可能
 * 
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/10/31
 * 
 **********************************************/
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンをロードするクラス
/// ロード先のシーンのコンポーネントを取得し値を渡すことができる
/// 指定コンポ―ネントはAwakeの後、Startの前の取得可能
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /*--------------------------------------------------------------------------------
　　|| シーンのロード処理（同期処理）
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// シーンのロード処理（同期処理）
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="mode">シーンロードモード</param>
    public static void Load(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName, mode);
    }

    /*--------------------------------------------------------------------------------
　　|| シーンのロード処理（コンポーネントを取得可能な同期処理）
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// シーンのロード処理（コンポーネントを取得可能な同期処理）
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="mode">シーンロードモード</param>
    /// <returns>ロード先シーンのコンポーネント</returns>
    public static UniTask<TComponent> Load<TComponent>(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        where TComponent : Component
    {
        // 返り値を持たないUniTaskを生成するUniTaskCompletionSourceを生成
        var tcs = new UniTaskCompletionSource<TComponent>();
        // シーンロード後に呼び出すイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
        // シーンをロードする
        SceneManager.LoadScene(sceneName, mode);

        // タスクを返す
        return tcs.Task;

        void OnSceneLoaded(Scene scene, LoadSceneMode _mode)
        {
            // 一度イベントを受けたら不要なので解除
            SceneManager.sceneLoaded -= OnSceneLoaded;
            // ロードしたシーンのルート階層のGameObjectから指定コンポーネントを1つ取得する
            var target = GetFirstComponent<TComponent>(scene.GetRootGameObjects());
            // UniTaskを完了させる
            tcs.TrySetResult(target);
        }
    }

    /*--------------------------------------------------------------------------------
　　|| シーンのロード処理（コンポーネントを取得可能な非同期処理）
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// シーンのロード処理（コンポーネントを取得可能な非同期処理）
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="mode">シーンロードモード</param>
    /// <returns>ロード先シーンのコンポーネント</returns>
    public static async UniTask<TComponent> LoadAsync<TComponent>(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        where TComponent : Component
    {
        // シーンのロードを待機する
        await SceneManager.LoadSceneAsync(sceneName, mode);
        // ロードするシーンを保存する
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // コンポーネントを取得して返す
        return GetFirstComponent<TComponent>(scene.GetRootGameObjects());
    }


    /*--------------------------------------------------------------------------------
　　|| GameObject配列から指定のコンポーネントを一つ取得する
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// GameObject配列から指定のコンポーネントを一つ取得する
    /// </summary>
    /// <typeparam name="TComponent">取得対象コンポーネント</typeparam>
    /// <param name="gameObjects">GameObject配列</param>
    /// <returns>対象コンポーネント</returns>
    private static TComponent GetFirstComponent<TComponent>(GameObject[] gameObjects)
        where TComponent : Component
    {
        // 空のコンポーネントを作成
        TComponent target = null;

        // 配列の中に対象コンポーネントがあれば取得する
        foreach (GameObject go in gameObjects)
        {
            target = go.GetComponent<TComponent>();
            if (target != null) break;
        }

        // 取得したコンポーネントを返す
        return target;
    }
}

