/**********************************************************
 *
 *  SelectPlayerUIManager.cs
 *  プレイヤーのUIを管理する
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/04
 *
 *********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerUIManager : MonoBehaviour
{
    // プレイヤー
    private SelectPlayer m_player;

    // キャラ選択のカーソル
    [SerializeField]
    private CharacterCursorUI m_selectCursor;

    // キャラ選択のモデル管理クラス
    [SerializeField]
    SelectCharaManager m_selectChara;

    [SerializeField]
    private GloveSlotUIManager m_selectGlove;

    [SerializeField]
    private UIElement m_ready;

    private bool m_canControll;

    public bool CanControll { get { return m_canControll; } set { m_canControll = value; } }


    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_ready.Rect.localScale = Vector3.zero;
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (m_player == null) return;

        if (m_player.IsReady) { m_ready.Rect.localScale = Vector3.one; }
        else if (!m_player.IsReady) { m_ready.Rect.localScale = Vector3.zero; }
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
        m_selectGlove.Bind(player);
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーUIを有効化する
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーUIを有効化する
    /// </summary>
    /// <param name="index">最初のキャラのIndex</param>
    public void Active(int index)
    {
        m_selectCursor.Active();
        m_selectChara.ChangeChara(index);
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーの変更
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーの変更
    /// </summary>
    /// <param name="index">キャラのindex</param>
    public void ChangeCharaIndex(int index)
    {
        m_selectCursor.MoveCharaCursor(index);
        m_selectChara.ChangeChara(index);
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーの決定
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーの変更
    /// </summary>
    /// <param name="index">キャラのindex</param>
    public void DecideCharaIndex()
    {
        m_selectCursor.DecideCharaCursor();
        m_selectChara.DecideChara();
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーのキャンセル
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーの変更
    /// </summary>
    /// <param name="index">キャラのindex</param>
    public void CancelCharaIndex()
    {
        m_selectCursor.CancelCharaCursor();
        m_selectChara.CancelChara();
    }
}