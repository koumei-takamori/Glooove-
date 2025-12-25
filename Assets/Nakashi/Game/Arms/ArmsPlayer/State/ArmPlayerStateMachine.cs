//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayerStateMachine.h
// <概要>		　　プレイヤーのステートマシーン
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using Player;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerStateMachine 
        {
            // 現在の状態
            INakashiPlayerState m_currentState;

            // 状態一覧
            private ArmPlayer_Idle m_idle;
            private ArmPlayer_Dash m_dash;
            private ArmPlayer_Jump m_jump;
            // 追加
            private ArmPlayer_LeftAttack m_leftattack;
            private ArmPlayer_RightAttack m_rightattack;


            // 現在のステートの状態取得
            public INakashiPlayerState GetCurrentState() => m_currentState;

            // 状態取得
            public ArmPlayer_Idle GetIdle() => m_idle;
            public ArmPlayer_Dash GetDash() => m_dash;
            public ArmPlayer_Jump GetJump() => m_jump;

            // 追加
            public ArmPlayer_LeftAttack GetLeftAttack() => m_leftattack;
            public ArmPlayer_RightAttack GetRightAttack() => m_rightattack;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller">所属するコントローラー</param>
            public ArmPlayerStateMachine(ArmPlayerController controller)
            {
                m_idle = new ArmPlayer_Idle(controller);
                m_dash = new ArmPlayer_Dash(controller);
                m_jump = new ArmPlayer_Jump(controller);
                // 追加
                m_leftattack = new ArmPlayer_LeftAttack(controller);
                m_rightattack = new ArmPlayer_RightAttack(controller);
            }

            /// <summary>
            /// 初期化処理
            /// </summary>
            /// <param name="state"></param>
            public void Initialize(INakashiPlayerState state)
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
            /// 物理演算との同時更新処理
            /// </summary>
            public void FixedUpdate()
            {
                m_currentState.FixedUpdate();
            }

            /// <summary>
            /// ステートの変更
            /// </summary>
            /// <param name="state">次のステート</param>
            public void ChangeState(INakashiPlayerState state)
            {
                m_currentState.Exit();
                m_currentState = state;
                m_currentState.Enter();
            }

            
        }
    }

}

