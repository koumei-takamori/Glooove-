/**********************************************************
 *
 *  GloveSlotUIManager.cs
 *  左右のグローブ選択用スロットUIを管理する
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/03
 *
 *********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectPlayer;

/// <summary>
/// 左右のグローブ選択用スロットUIを管理する
/// </summary>
public class GloveSlotUIManager : MonoBehaviour
{
    // カーソル
    [SerializeField]
    private GloveSelectCursor m_coursol;

    // 左右のグローブ選択のUI
    [SerializeField] private GloveSlotUI m_leftSlot;
    [SerializeField] private GloveSlotUI m_rightSlot;

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
       
    }


    /*--------------------------------------------------------------------------------
　　|| グローブの変更
　　--------------------------------------------------------------------------------*/
    public void ChangeGloveIndex(GloveSide side, int index)
    {
        if (side == GloveSide.Left)
        {
            m_leftSlot.SetIndex(index);
        }
        else if (side == GloveSide.Right)
        {
            m_rightSlot.SetIndex(index);
        }
    }

    /*--------------------------------------------------------------------------------
　　|| グローブの左右変更
　　--------------------------------------------------------------------------------*/
    public void ChangeGloveSide(GloveSide side)
    {
       m_coursol.MoveCursor((int)side);
    }

    /*--------------------------------------------------------------------------------
　　|| グローブの決定
　　--------------------------------------------------------------------------------*/
    public void GloveDecide(GloveSide side)
    {
        if (side == GloveSide.Left)
        {
            m_leftSlot.Decide();
        }
        else if (side == GloveSide.Right)
        {
            m_rightSlot.Decide();
        }
    }

    /*--------------------------------------------------------------------------------
　　|| グローブのキャンセル
　　--------------------------------------------------------------------------------*/
    public void GloveCancel(GloveSide side)
    {
        if (side == GloveSide.Left)
        {
            m_leftSlot.Cancel();
        }
        else if (side == GloveSide.Right)
        {
            m_rightSlot.Cancel();
        }
    }
}
