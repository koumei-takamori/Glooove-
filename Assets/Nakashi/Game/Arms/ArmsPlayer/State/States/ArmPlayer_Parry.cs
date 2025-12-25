//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/11/18
// <file>			ArmPlayer_Parry.h
// <概要>		　　ステートマシーン パリィ状態
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_Parry : INakashiPlayerState
        {
            private ArmPlayerController m_controller;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_Parry(ArmPlayerController controller)
            {
                m_controller = controller;

            }
            /// <summary>
            /// 入出時
            /// </summary>
            public void Enter()
            {
                // パリィ判定をオンにする
                m_controller.GetPlayerStatus().GetSetParry = true;
                // 動けなくする
                m_controller.GetPlayerStatus().GetSetControll = true;
            }

            /// <summary>
            /// 退出時
            /// </summary>
            public void Exit()
            {
                // パリィ判定をオフにする
                m_controller.GetPlayerStatus().GetSetParry = false;
                m_controller.GetPlayerStatus().GetSetControll = false;
            }

            /// <summary>
            /// 更新時
            /// </summary>
            public void Update()
            {
                ReleaseButton();
                Debug.Log(m_controller.GetPlayerStatus().GetSetParry);
            }

            /// <summary>
            /// 物理演算との更新処理
            /// </summary>
            public void FixedUpdate()
            {

            }

            /// <summary>
            /// ボタンを離した時の処理用
            /// </summary>
            private void ReleaseButton()
            {
                // キーが離れたら、アイドリング状態に戻す。
                if(Input.GetKeyUp(KeyCode.P)) { m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetIdle()); }
            }
        }
    }

}
