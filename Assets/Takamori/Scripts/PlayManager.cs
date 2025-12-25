/**********************************************************
 *
 *  PlayManager.cs
 *  プレイシーンの管理クラス
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/12
 *
 *********************************************************/
using Nakashi.Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayManager : MonoBehaviour
{
    // プレイヤー配列
    [SerializeField]
    private ArmPlayerController m_players;

    // UI管理クラス
    [SerializeField]
    private UIController m_uiController;

    // 終了フラグ
    private bool m_isFinish;

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
