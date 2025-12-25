//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			ArmPlayerCollisions.h
// <概要>		　　アームプレイヤーの当たり判定
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerCollisions : MonoBehaviour
        {
            [SerializeField] private ArmPlayerController m_controller;

            private void Start()
            {
                // 同じオブジェクトにあるコントローラーを代入しておく
                m_controller = this.GetComponent<ArmPlayerController>();
            }

            private void OnCollisionEnter(Collision collision)
            {
            }
        }
    }
}

