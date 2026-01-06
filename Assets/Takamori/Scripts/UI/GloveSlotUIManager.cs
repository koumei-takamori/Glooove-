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

    // プロパティ
    public SelectPlayer Player { get { return m_player; } set { m_player = value; } }

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
        m_leftSlot.SetIndex(m_player.GetGloveIndex(SelectPlayer.GloveSide.Left));
        m_rightSlot.SetIndex(m_player.GetGloveIndex(SelectPlayer.GloveSide.Right));

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
        GloveSide activeSide = m_player.CurrentGloveSide;

        m_leftSlot.SetActive(activeSide == GloveSide.Left);
        m_rightSlot.SetActive(activeSide == GloveSide.Right);
    }
}
