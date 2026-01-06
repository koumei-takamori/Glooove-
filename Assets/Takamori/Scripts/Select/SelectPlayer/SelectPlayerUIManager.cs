/**********************************************************
 *
 *  SelectPlayerUIManager.cs
 *  プレイヤーのUIを管理する
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/04
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerUIManager : MonoBehaviour
{
    // プレイヤー
    private SelectPlayer m_player;

    [SerializeField]
    private CharacterCursorUI m_selectCursor;

    [SerializeField]
    private GloveSlotUIManager m_selectGlove;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        // 最初はUIを停止
        CanControllUI(false);
    }

    /*--------------------------------------------------------------------------------
　　|| UIとPlayerを結び付ける
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// UIとPlayerを結び付ける
    /// </summary>
    public void Bind(SelectPlayer player)
    {
        m_player = player;
        m_selectCursor.Bind(player);
        m_selectGlove.Bind(player);
    }

    /*--------------------------------------------------------------------------------
　　|| UIの操作可否
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// UIの操作可否
    /// </summary>
    /// <param name="canControll">UIの操作可能フラグ</param>
    private void CanControllUI(bool canControll)
    {
        m_selectCursor.CanControll = canControll;
        m_selectGlove.CanControll = canControll;
    }
}
