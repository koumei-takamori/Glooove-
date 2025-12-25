//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			EnemyAI_Idle.h
// <概要>		　　敵AIのアイドリング状態
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace EnemyAI
    {
        public class EnemyAI_Idle : IEnemyAIState
        {

            // 所属コントローラー
            private EnemyAIController m_controller;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public EnemyAI_Idle(EnemyAIController controller)
            {
                m_controller = controller;
            }

            /// <summary>
            /// 入出時
            /// </summary>
            public void Enter()
            {

            }
            /// <summary>
            /// 退出時
            /// </summary>
            public void Exit()
            {

            }
            /// <summary>
            /// 更新時
            /// </summary>
            public void Update()
            {

            }
        }
    }
}

