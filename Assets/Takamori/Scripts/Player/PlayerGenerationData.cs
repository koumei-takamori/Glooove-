/**********************************************************
 *
 *  PlayerGenerationData.cs
 *  プレイヤーの生成情報
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの生成情報
/// </summary>
[System.Serializable]
public class PlayerGenerationData : MonoBehaviour
{
    // プレイヤーID
    public int playerId = 0;       

    // プレイヤーのプレハブ
    public GameObject playerPrefab;
    // 生成位置
    public Vector3 spawnPosition = new Vector3(0,3,0);
    // 生成角度
    public Quaternion spawnRotation = Quaternion.identity;

    // アームのデータ
    public PlayerGloveData gloveData;
}
