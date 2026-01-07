/**********************************************************
 *
 *  ScrollBackGround.cs
 *  背景をスクロールする
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/01/07
 *
 *********************************************************/
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ScrollBackGround : MonoBehaviour
{
    [SerializeField] private RectTransform[] m_tiles; // 4枚
    [SerializeField] private float m_duration = 6f;

    private float m_tileW;
    private float m_tileH;

    private void Start()
    {
        m_tileW = m_tiles[0].rect.width;
        m_tileH = m_tiles[0].rect.height;

        for (int i = 0; i < m_tiles.Length; i++)
        {
            // 初期配置（斜め）
            m_tiles[i].anchoredPosition =
                new Vector2(m_tileW * i, -m_tileH * i);

            StartMove(m_tiles[i]);
        }
    }

    private void StartMove(RectTransform tile)
    {
        Vector2 startPos = tile.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(m_tileW * 4, -m_tileH * 4);

        tile
            .DOAnchorPos(endPos, m_duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 一番左上へ戻す
                RectTransform top = GetTopLeftTile();

                tile.anchoredPosition =
                    top.anchoredPosition - new Vector2(m_tileW, -m_tileH);

                // 再スタート
                StartMove(tile);
            });
    }

    /// <summary>
    /// 一番左上にあるタイルを取得
    /// </summary>
    private RectTransform GetTopLeftTile()
    {
        RectTransform result = m_tiles[0];
        float min = result.anchoredPosition.x - result.anchoredPosition.y;

        foreach (var t in m_tiles)
        {
            float v = t.anchoredPosition.x - t.anchoredPosition.y;
            if (v < min)
            {
                min = v;
                result = t;
            }
        }
        return result;
    }

    private void OnDestroy()
    {
        foreach (var t in m_tiles)
        {
            t.DOKill();
        }
    }
}
