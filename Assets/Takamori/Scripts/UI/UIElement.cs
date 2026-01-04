/**********************************************************
 *
 *  UIElement.cs
 *  UIで使用頻度の高い要素をまとめたクラス
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/03
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIで使用頻度の高い要素をまとめたクラス
/// </summary>

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[System.Serializable]
public class UIElement : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_rect;

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    // プロパティ
    public RectTransform Rect { get { return m_rect; } }
    public CanvasGroup CanvasGroup { get { return m_canvasGroup; } }

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {
        m_rect = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();
    }
}

