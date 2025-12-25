/**********************************************************
 *
 *  PlayeSceneManager.cs
 *  プレイシーンを管理
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/27
 *
 *********************************************************/
using Nakashi.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイシーンを管理するクラス
/// </summary>
public class PlaySceneManager : SingletonMonoBehaviour<PlaySceneManager>
{
    // プレイヤー１
    [SerializeField]
    private GameObject m_player1;

    [SerializeField]
    // プレイヤー２
    private GameObject m_player2;

    // プロパティ
    public GameObject Player1 { get { return m_player1; } set { m_player1 = value; } }

    public GameObject Player2 { get { return m_player2; } set { m_player2 = value; } }

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
    }
}


