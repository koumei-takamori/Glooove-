/**********************************************************
 *
 *  GloveSlotUIManager.cs
 *  左右のグローブ選択用スロットUIを管理する
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/03
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectPlayer;

/// <summary>
/// 左右のグローブ選択用スロットUIを管理する
/// </summary>
public class GloveSlotUIManager : MonoBehaviour
{
    // プレイヤー
    [SerializeField]
    private SelectPlayer m_player;

    // 左右のグローブ選択のUI
    [SerializeField] private GloveSlotUI m_leftSlot;
    [SerializeField] private GloveSlotUI m_rightSlot;

    // 操作可能フラグ
    private bool m_canControll = false;

    // プロパティ
    public SelectPlayer Player { get { return m_player; } }
    public bool CanControll { get { return m_canControll; } set { m_canControll = value; } }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // 操作不能なら処理しない
        if (!m_canControll) return;

        m_leftSlot.SetIndex(m_player.GetGloveIndex(GloveSide.Left));
        m_rightSlot.SetIndex(m_player.GetGloveIndex(GloveSide.Right));

        // UI更新処理
        UpdateSlot();
    }

    /*--------------------------------------------------------------------------------
　　|| UI更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// UI更新処理
    /// </summary>
    private void UpdateSlot()
    {
        // 操作不能なら処理しない
        if (!m_canControll) return;

        GloveSide activeSide = m_player.CurrentGloveSide;

        m_leftSlot.SetActive(activeSide == GloveSide.Left);
        m_rightSlot.SetActive(activeSide == GloveSide.Right);
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
        m_canControll = true;
    }
}
