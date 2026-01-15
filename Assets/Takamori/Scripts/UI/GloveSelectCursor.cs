using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveSelectCursor : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private RectTransform cursor;
    private Vector2 leftPosition = new Vector2(-300.0f,0.0f);
    private Vector2 rightPosition = new Vector2(300.0f,0.0f);

    [Header("Tween Settings")]
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private Ease easeType = Ease.OutCubic;

    private int currentSide = 0; // 0:Left, 1:Right
    private Tween moveTween;

    /// <summary>
    /// カーソルを移動させる
    /// 0 = 左, 1 = 右, その他 = 非表示
    /// </summary>
    public void MoveCursor(int side)
    {
        // 無効な値 → 非表示
        if (side != 0 && side != 1)
        {
            cursor.gameObject.SetActive(false);
            return;
        }

        // 表示
        cursor.gameObject.SetActive(true);

        // 同じ位置なら何もしない
        if (currentSide == side)
        {
            return;
        }

        currentSide = side;

        // 既存Tweenを停止
        moveTween?.Kill();

        Vector2 targetPos = (side == 0) ? leftPosition : rightPosition;

        moveTween = cursor
            .DOAnchorPos(targetPos, moveDuration)
            .SetEase(easeType);
    }
}
