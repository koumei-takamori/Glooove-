//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayer_Idle.h
// <概要>		　　ステートマシーン アイドル状態
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_Idle : INakashiPlayerState
        {
            // 所属コントローラー
            private ArmPlayerController m_controller;



            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_Idle(ArmPlayerController controller)
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
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Debug.Log("Hおされた");
                }

                // ステート変更関数
                ChangeState();
            }

            /// <summary>
            /// ステート変更用関数
            /// </summary>
            private void ChangeState()
            {
                //if (m_controller.IsGround() && (
                //    //Nakashi.Framework.VibrationSystem.Instance.GetNPad().GetButtonDown(nn.hid.NpadButton.L) || 
                //    Input.GetKeyDown(KeyCode.Space)))
                //{
                //    m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetJump());
                //    return;
                //}
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


