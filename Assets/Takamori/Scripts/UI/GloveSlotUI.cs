/**********************************************************
 *
 *  GloveSlotUI.cs
 *  グローブ選択用スロットUI
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グローブ選択用スロットUI
/// </summary>
public class GloveSlotUI : MonoBehaviour
{
    // 左右の判別
    [SerializeField]
    private int m_side;

    // アイコンのオブジェクト
    [SerializeField]
    private List<UIElement> m_icons = new List<UIElement>();

    // 間隔
    [SerializeField]
    private float m_slotSpacing = 80.0f;
    // 移動時間
    [SerializeField]
    private float m_moveDuration = 0.1f;
    // 中央のスケール
    [SerializeField]
    private float m_centerScale = 1.2f;
    // サイドのスケール
    [SerializeField]
    private float m_sideScale = 0.8f;
    // 中央の透明度
    [SerializeField]
    private float m_centerAlpha = 1.0f;
    // サイドの透明度
    [SerializeField]
    private float m_sideAlpha = 0.4f;

    // 現在のインデックス
    private int m_currentIndex = 0;
    // 最大数
    private int m_maxCount = 0;

    // 設定位置
    // private static Vector2 m_baseAnchoredPos = new Vector2(160f, -300f);
    private static Vector2 m_baseAnchoredPos = new Vector2(0f, 0f);

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        m_maxCount = m_icons.Count;
        InitSlotIcon();
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

    /*--------------------------------------------------------------------------------
　　|| 選択インデックス設定
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 選択インデックス設定
    /// </summary>
    public void SetIndex(int index)
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
            RectTransform icon = m_icons[i].Rect;

            // 中央から見て何番目にあるかを計算する
            int diff = GetLoopDiff(i, m_currentIndex);

            // 中央のアイコンから１以上離れている場合表示しない
            bool isVisible = Mathf.Abs(diff) <= 1;
            icon.gameObject.SetActive(isVisible);

            // 表示されてなければ処理しない
            if (!isVisible) continue;

            // 目的座標設定
            Vector2 targetPos = m_baseAnchoredPos;
            targetPos.y += -diff * m_slotSpacing;

            // 中央なら中央サイズ、サイドならサイドサイズに変更
            float targetScale = (diff == 0) ? m_centerScale : m_sideScale;
            float targetAlpha = (diff == 0) ? m_centerAlpha : m_sideAlpha;

            // 移動を消す
            icon.DOKill();

            // 移動
            icon.DOAnchorPos(targetPos, m_moveDuration)
                .SetEase(Ease.OutCubic);

            // スケール
            icon.DOScale(Vector3.one * targetScale, m_moveDuration)
                .SetEase(Ease.OutBack);

            m_icons[i].CanvasGroup.DOFade(targetAlpha, m_moveDuration);

            // 中央を最前面
            if (diff == 0)
            {
                icon.SetSiblingIndex(m_icons.Count - 1);
            }
        }
    }

    /*--------------------------------------------------------------------------------
　　|| 各アイコンの初期化
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 各アイコンの初期化
    /// </summary>
    private void InitSlotIcon()
    {
        // すべてのアイコンを順番に処理
        for (int i = 0; i < m_icons.Count; i++)
        {
            // 各アイコンのRectTransformを取得
            RectTransform icon = m_icons[i].Rect;

            // 中央から見て何番目にあるかを計算する
            int diff = GetLoopDiff(i, m_currentIndex);

            // 中央のアイコンから１以上離れている場合表示しない
            bool isVisible = Mathf.Abs(diff) <= 1;
            icon.gameObject.SetActive(isVisible);

            // 表示されてなければ処理しない
            if (!isVisible) continue;

            // 座標設定
            Vector2 pos = m_baseAnchoredPos;
            pos.y += -diff * m_slotSpacing;
            icon.anchoredPosition = pos;

            // 中央なら中央サイズ、サイドならサイドサイズに変更
            float scale = (diff == 0) ? m_centerScale : m_sideScale;
            icon.localScale = Vector3.one * scale;

            m_icons[i].CanvasGroup.alpha = (diff == 0) ? m_centerAlpha : m_sideAlpha;
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


    public void SetActive(bool active)
    {
    }
}