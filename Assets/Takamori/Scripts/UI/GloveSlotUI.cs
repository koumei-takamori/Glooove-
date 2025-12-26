/**********************************************************
 *
 *  GloveSlotUI.cs
 *  グローブ選択用スロットUI
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グローブ選択用スロットUI
/// </summary>
public class GloveSlotUI : MonoBehaviour
{
    // セレクトのプレイヤー
    [SerializeField]
    private SelectPlayer m_player;

    [SerializeField]
    private List<RectTransform> m_icons = new List<RectTransform>();

    [SerializeField]
    private float m_slotSpacing = 80.0f;

    [SerializeField]
    private float m_moveDuration = 0.25f;

    [SerializeField]
    private float m_centerScale = 1.2f;

    [SerializeField]
    private float m_sideScale = 0.8f;

    [SerializeField]
    private float m_centerAlpha = 1.0f;

    [SerializeField]
    private float m_sideAlpha = 0.4f;

    private int m_currentIndex = 0;
    private int m_maxCount = 0;

    /*--------------------------------------------------------------------------------
　　|| 初期化
　　--------------------------------------------------------------------------------*/
    private void Awake()
    {
        m_maxCount = m_icons.Count;
        UpdateSlotIcon();
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // indexを設定
        SetIndex(m_player.GetGloveIndex());
    }

    /*--------------------------------------------------------------------------------
　　|| 選択インデックス設定
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 選択インデックス設定
    /// </summary>
    private void SetIndex(int index)
    {
        m_currentIndex = LoopIndex(index);
        UpdateSlotTween();
    }

    /*--------------------------------------------------------------------------------
　　|| DOTweenでスロット更新
　　--------------------------------------------------------------------------------*/
    private void UpdateSlotTween()
    {
        for (int i = 0; i < m_icons.Count; i++)
        {
            // RectTransform取得
            RectTransform icon = m_icons[i];

            // 中央から見て何番目にあるかを計算する
            int diff = GetLoopDiff(i, m_currentIndex);

            // ================================
            // 表示制限（中央±1のみ）
            // ================================
            bool isVisible = Mathf.Abs(diff) <= 1;
            icon.gameObject.SetActive(isVisible);

            if (!isVisible) continue;

            Vector2 targetPos = Vector2.zero;
            targetPos.y = -diff * m_slotSpacing;

            float targetScale = (diff == 0) ? m_centerScale : m_sideScale;
            float targetAlpha = (diff == 0) ? m_centerAlpha : m_sideAlpha;

            icon.DOKill();

            // 移動
            icon.DOAnchorPos(targetPos, m_moveDuration)
                .SetEase(Ease.OutCubic);

            // スケール
            icon.DOScale(Vector3.one * targetScale, m_moveDuration)
                .SetEase(Ease.OutBack);

            // 透明度（CanvasGroup）
            CanvasGroup cg = icon.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = icon.gameObject.AddComponent<CanvasGroup>();

            cg.DOFade(targetAlpha, m_moveDuration);

            // 中央を最前面
            if (diff == 0)
            {
                icon.SetSiblingIndex(m_icons.Count - 1);
            }
        }
    }

    /*--------------------------------------------------------------------------------
　　|| 各アイコン更新（初期表示用）
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 各アイコン更新
    /// </summary>
    private void UpdateSlotIcon()
    {
        // すべてのアイコンを順番に処理
        for (int i = 0; i < m_icons.Count; i++)
        {
            // 各アイコンのRectTransformを取得
            RectTransform icon = m_icons[i];

            // 中央との差を作成
            int diff = GetLoopDiff(i, m_currentIndex);

            bool isVisible = Mathf.Abs(diff) <= 1;
            icon.gameObject.SetActive(isVisible);

            if (!isVisible) continue;

            Vector2 pos = Vector2.zero;
            pos.y = -diff * m_slotSpacing;
            icon.anchoredPosition = pos;

            float scale = (diff == 0) ? m_centerScale : m_sideScale;
            icon.localScale = Vector3.one * scale;

            CanvasGroup cg = icon.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = icon.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = (diff == 0) ? m_centerAlpha : m_sideAlpha;
        }
    }

    /*--------------------------------------------------------------------------------
　　|| インデックスの補正
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// インデックスの補正
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <returns></returns>
    private int LoopIndex(int index)
    {
        if (index < 0) return m_maxCount - 1;
        if (index >= m_maxCount) return 0;
        return index;
    }

    /*--------------------------------------------------------------------------------
　　|| 各アイコンが、中央から見て何番目にあるかを計算する
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 各アイコンが、中央から見て何番目にあるかを計算する
    /// </summary>
    /// <param name="iconIndex">アイコンの数</param>
    /// <param name="centerIndex">中央のインデックス</param>
    /// <returns></returns>
    private int GetLoopDiff(int iconIndex, int centerIndex)
    {
        // 中央のindexからほかのアイコンをindexを算出
        int diff = iconIndex - centerIndex;

        // ループ補正
        if (diff > m_maxCount / 2) diff -= m_maxCount;
        if (diff < -m_maxCount / 2) diff += m_maxCount;

        return diff;
    }
}
