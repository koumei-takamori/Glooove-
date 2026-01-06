/**********************************************************
 *
 *  SelectCharaController.cs
 *  選択キャラ制御
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択キャラ制御
/// </summary>
public class SelectCharaController : MonoBehaviour
{
    // Player接続前の画像
    [SerializeField]
    private Image m_waitingJoinImage;

    // 各キャラクター
    [SerializeField]
    private GameObject m_balance;
    [SerializeField]
    private GameObject m_speed;
    [SerializeField]
    private GameObject m_tank;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_balance.SetActive(false);
        m_speed.SetActive(false);
        m_tank.SetActive(false);
    }
}
