//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/31
// <file>			ArmPlayer_Dash.h
// <概要>		　　ステートマシーン ダッシュ状態
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_Dash : INakashiPlayerState
        {
            private ArmPlayerController m_controller;



            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_Dash(ArmPlayerController controller)
            {
                m_controller = controller;

            }
            /// <summary>
            /// 入出時
            /// </summary>
            public void Enter()
            {
                // ダッシュのトリガーをオンにする
                m_controller.GetAnimator().SetTrigger("Dash");

                Vector3 inputDir = m_controller.GetSetVelocity;
                // 入力がない場合の前ダッシュ
                if(inputDir.sqrMagnitude < 0.01f)
                {
                    inputDir = m_controller.GetTransform().forward;
                }
                Vector3 dashDir = inputDir.normalized;

                m_controller.GetRigidbody().AddForce(dashDir * m_controller.GetPlayerData().GetDashSpeed(),
                    ForceMode.Impulse);
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
                // 1フレームですぐにアイドルへ戻ってもらう
                m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetIdle());
            }

            /// <summary>
            /// 物理演算との更新処理
            /// </summary>
            public void FixedUpdate()
            {

            }
        }
    }
}


