//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			EnemyAI_Move.h
// <概要>		　　敵AIの移動処理
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace EnemyAI
    {
        public class EnemyAI_Move : IEnemyAIState
        {
            // 所属コントローラー
            private EnemyAIController m_controller;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public EnemyAI_Move(EnemyAIController controller)
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