//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			LookAtPointOfFixation.h
// <概要>		　　一転を見つめる
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPointOfFixation : MonoBehaviour
{
    [Header("注視点"), SerializeField] Transform m_point;
    [Header("回転速度"), SerializeField] float m_rotateSpeed = 30f;

    void Update()
    {
        if (m_point == null) return;

        // 注視点との方向ベクトル
        Vector3 dir = transform.position - m_point.position;

        // 回転角度
        float angle = m_rotateSpeed * Time.deltaTime;

        // 注視点を中心に、自分の位置を回転させる
        dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;

        // 新しい位置に移動
        transform.position = m_point.position + dir;

        // 注視点を向かせる
        transform.LookAt(m_point);
    }
}
