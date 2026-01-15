using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharaCamera : MonoBehaviour
{
    [SerializeField] private float m_moveDuration = 0.3f;
    [SerializeField] private Ease m_ease = Ease.OutCubic;

    /// <summary>
    /// X²‚¾‚¯ƒLƒƒƒ‰‚É‡‚í‚¹‚ÄƒJƒƒ‰‚ğˆÚ“®
    /// </summary>
    public void MoveToTargetX(Transform target)
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x;

        transform.DOMoveX(pos.x, m_moveDuration)
                 .SetEase(m_ease);

        Debug.Log("‚©‚ß‚çˆÚ“®" + transform);
    }
}
