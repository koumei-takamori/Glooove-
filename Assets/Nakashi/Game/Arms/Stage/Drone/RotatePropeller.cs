//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			RotatePropeller.h
// <概要>		　　プロペラの回転
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    [Header("回転速度"), SerializeField] private float m_rotateSpeed = 30f;
    
    void Update()
    {
        // １フレームで回転する角度を角速度と経過時間から計算
        var angle = m_rotateSpeed * Time.deltaTime;

        // 既存のrotationに軸回転のクォータニオンを掛ける
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * transform.rotation;
    }
}
