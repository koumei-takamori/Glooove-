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

    // グローブ選択のUI管理クラス
    [SerializeField]
    private GloveSlotUIManager m_selectGlove;

    // 準備完了
    [SerializeField]
    private UIElement m_ready;

    /*--------------------------------------------------------------------------------
　　|| プレイヤーUIを有効化する
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーUIを有効化する
    /// </summary>
    /// <param name="index">最初のキャラのIndex</param>
    public void Initialize(int index)
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
    public void DecideCharaIndex(int index)
    {
        m_selectCursor.DecideCharaCursor();
        m_selectChara.DecideChara(index);
    }

    /*--------------------------------------------------------------------------------
　　|| プレイヤーのキャンセル
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// プレイヤーのキャンセル
    /// </summary>
    /// <param name="index">キャラのindex</param>
    public void CancelCharaIndex()
    {
        m_selectCursor.CancelCharaCursor();
        m_selectChara.CancelChara();
    }

    /*--------------------------------------------------------------------------------
　　|| グローブの変更
　　--------------------------------------------------------------------------------*/
    public void ChangeGloveIndex(GloveSide side, int index)
    {
        m_selectGlove.ChangeGloveIndex(side,index);
    }
    /*--------------------------------------------------------------------------------
　　|| グローブの左右変更
　　--------------------------------------------------------------------------------*/
    public void ChangeGloveSide(GloveSide side)
    {
        m_selectGlove.ChangeGloveSide(side);
    }
    /*--------------------------------------------------------------------------------
　　|| グローブの決定
　　--------------------------------------------------------------------------------*/
    public void GloveDecide(GloveSide side)
    {
        m_selectGlove.GloveDecide(side);
    }
    /*--------------------------------------------------------------------------------
　　|| グローブのキャンセル
　　--------------------------------------------------------------------------------*/
    public void GloveCancel(GloveSide side)
    {
        m_selectGlove.GloveCancel(side);
    }


    /*--------------------------------------------------------------------------------
　　|| 準備完了UIの変更
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 準備完了の変更
    /// </summary>
    /// <param name="isReady">準備完了かどうか</param>
    public void IsReady(bool isReady)
    {
        // アニメーションを変更する
        m_ready.Animator.SetBool("isSelectOK", isReady);
    }

}