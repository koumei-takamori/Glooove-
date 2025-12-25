//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			EnemyAIController.h
// <概要>		　　敵AIのコントローラー
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace EnemyAI
    {
        public class EnemyAIController : MonoBehaviour
        {
            // リジットボディ、トランスフォーム
            private Rigidbody m_rb;
            private Transform m_transform;

            // 速度
            private Vector3 m_velocity = Vector3.zero;

            // ステートマシーン
            private EnemyAIStateMachine m_stateMachine;



            /// <summary>
            /// 初期化処理
            /// </summary>
            void Start()
            {
                // リジットボディ、トランスフォーム取得
                m_rb = this.GetComponent<Rigidbody>();
                m_transform = this.GetComponent<Transform>();

                // ステートマシンの初期設定
                m_stateMachine = new EnemyAIStateMachine(this);
                m_stateMachine.Initialize(m_stateMachine.GetIdle());
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            void Update()
            {
                // ステートマシンの更新処理
                m_stateMachine.Update();
            }

            public EnemyAIStateMachine GetStateMachine() => m_stateMachine;

            public Rigidbody GetRigidbody() => m_rb;
            public Transform GetTransform() => m_transform;
        }
    }
}

