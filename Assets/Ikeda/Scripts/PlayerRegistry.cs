// --------------------------------------------------------------
//
// PlayerRegistry.cs
// プレイヤーオブジェクトを登録・管理するシングルトンクラス
// 池田桜輔
// 2026/01/05
// 
// --------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry : MonoBehaviour
{
    private static PlayerRegistry instance;

    public static PlayerRegistry Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerRegistry>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerRegistry");
                    instance = obj.AddComponent<PlayerRegistry>();
                }
            }
            return instance;
        }
    }

    [Header("auto : 両プレイヤー"), SerializeField]
    private List<GameObject> players = new List<GameObject>();

    /// <summary>
    /// プレイヤーを登録
    /// </summary>
    /// <param name="player">登録するプレイヤー</param>
    public void RegisterPlayer(GameObject player)
    {
        if (!players.Contains(player))
        {
            Debug.Log("PlayerRegistry: プレイヤー登録 " + player.name);
            players.Add(player);
        }
    }

    /// <summary>
    /// プレイヤーを登録解除
    /// </summary>
    /// <param name="player">登録解除するプレイヤー</param>
    public void UnregisterPlayer(GameObject player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
    }

    /// <summary>
    /// プレイヤーのリストを取得
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetAllPlayers()
    {
        Debug.Log("PlayerRegistry: 登録されているプレイヤー数 " + players.Count);
        return new List<GameObject>(players);
    }

    public GameObject GetPlayer(int index)
    {
        if (index < 0 || index >= players.Count)
        {
            Debug.LogError("PlayerRegistry: 指定されたインデックスが範囲外です " + index);
            return null;
        }
        return players[index];
    }
}
