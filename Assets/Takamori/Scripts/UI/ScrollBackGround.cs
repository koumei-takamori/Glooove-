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
/// 背景をスクロールする
/// </summary>
public class ScrollBackGround : MonoBehaviour
{
    // 動かす背景
    [SerializeField]
    private RectTransform[] m_tiles;

    // 移動時間
    [SerializeField]
    private float m_duration = 2.0f;

    // タイルサイズ（斜め配置）
    [SerializeField]
    private float m_tileWidth = 3000.0f;

    [SerializeField]
    private float m_tileHeight = 750.0f;

    /*--------------------------------------------------------------------------------
     || 初期化処理
    --------------------------------------------------------------------------------*/
    private void Start()
    {
        // 全タイル同時に移動開始
        foreach (var tile in m_tiles)
        {
            MoveLoop(tile);
        }
    }

    /// <summary>
    /// タイルを永遠にループ移動させる
    /// </summary>
    private void MoveLoop(RectTransform tile)
    {
        tile.DOKill();

        // 1タイル分だけ移動
        Vector2 endPos =
            tile.anchoredPosition + new Vector2(-m_tileWidth, -m_tileHeight);

        tile.DOAnchorPos(endPos, m_duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 画面外に出たら先頭の次へワープ
                WarpToNext(tile);

                // 次の移動を開始（＝無限ループ）
                MoveLoop(tile);
            });
    }

    /// <summary>
    /// 先頭タイルの「次の位置」へワープ
    /// </summary>
    private void WarpToNext(RectTransform tile)
    {
        RectTransform top = GetTopLeftTile();

        tile.anchoredPosition =
            top.anchoredPosition + new Vector2(m_tileWidth, m_tileHeight);
    }

    /// <summary>
    /// 一番左上（進行方向の先頭）にあるタイルを取得
    /// </summary>
    private RectTransform GetTopLeftTile()
    {
        RectTransform top = m_tiles[0];

        for (int i = 1; i < m_tiles.Length; i++)
        {
            if (m_tiles[i].anchoredPosition.y > top.anchoredPosition.y)
            {
                top = m_tiles[i];
            }
        }

        return top;
    }

    private void OnDestroy()
    {
        foreach (var tile in m_tiles)
        {
            tile.DOKill();
        }
    }
}