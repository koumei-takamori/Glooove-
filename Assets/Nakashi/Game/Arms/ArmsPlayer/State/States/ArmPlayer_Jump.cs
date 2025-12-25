//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayer_Jump.h
// <概要>		　　ステートマシーン ジャンプさせる
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_Jump : INakashiPlayerState
        {
            // 所属コントローラー
            private ArmPlayerController m_controller;



            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_Jump(ArmPlayerController controller)
            {
                m_controller = controller;

            }
            /// <summary>
            /// 入出時
            /// </summary>
            public void Enter()
            {
     

                // AddForceで上方向に
                m_controller.GetRigidbody().AddForce(
                    new Vector3
                    (0.0f,
                    m_controller.GetPlayerData().GetJumpForce(),
                    0.0f),
                    ForceMode.Impulse);

                // アニメーションをジャンプに切り替える
                m_controller.GetAnimator().SetTrigger("Jump");
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


