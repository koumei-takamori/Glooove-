//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			EnemyAIStateMachine.h
// <概要>		　　敵エネミーのAIのステートマシーン本体
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace EnemyAI
    {
        public class EnemyAIStateMachine
        {
            // 現在の状態
            IEnemyAIState m_currentState;

            // 状態一覧
            private EnemyAI_Idle m_idle;

            // 現在のステートの状態取得
            public IEnemyAIState GetCurrentState() => m_currentState;

            public EnemyAI_Idle GetIdle() => m_idle;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller">コントローラー</param>
            public EnemyAIStateMachine(EnemyAIController controller)
            {
                m_idle = new EnemyAI_Idle(controller);
            }

            /// <summary>
            /// 初期化処理
            /// </summary>
            /// <param name="state">ステート</param>
            public void Initialize(IEnemyAIState state)
            {
                m_currentState = state;
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            public void Update()
            {
                // 現在のステートの更新
                m_currentState.Update();
            }

            /// <summary>
            /// ステート変更
            /// </summary>
            /// <param name="state">次のステート</param>
            public void ChangeState(IEnemyAIState state)
            {
                m_currentState.Exit();
                m_currentState = state;
                m_currentState.Enter();
            }


        }
    }
}


