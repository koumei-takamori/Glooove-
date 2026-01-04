//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/11/18
// <file>			ArmPlayer_BlowOff.h
// <概要>		　　ステートマシーン 吹っ飛び状態
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_BlowOff : INakashiPlayerState
        {
            private ArmPlayerController m_controller;

            private readonly ArmPlayerCoolDown m_blowOffTime;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_BlowOff(ArmPlayerController controller)
            {
                m_controller = controller;
                m_blowOffTime = new ArmPlayerCoolDown(m_controller.GetPlayerData().GetBlowOffTime());

            }
            /// <summary>
            /// 入出時
            /// </summary>
            public void Enter()
            {
                // 無敵判定をオンにする
                m_controller.GetPlayerStatus().GetSetInvincible = true;
                m_controller.GetPlayerStatus().GetSetControll = true;

                // 敵の位置と、自分の位置をみて、吹っ飛ぶ方向ベクトルを決定
                Vector3 blowVel = m_controller.GetTransform().transform.position - m_controller.Target.transform.position;
                blowVel.y = 0;
                blowVel.Normalize();
                m_controller.GetRigidbody().velocity = Vector3.zero;
                m_controller.GetRigidbody().AddForce(blowVel * m_controller.GetPlayerData().GetBlowOffPower());
            }

            /// <summary>
            /// 退出時
            /// </summary>
            public void Exit()
            {
                // 無敵判定をオフにする
                m_controller.GetPlayerStatus().GetSetInvincible = false;
                m_controller.GetPlayerStatus().GetSetControll = false;
            }
            /// <summary>
            /// 更新時
            /// </summary>
            public void Update()
            {
                m_blowOffTime.Update(Time.deltaTime);
                if(m_blowOffTime.IsReady())
                {
                    m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetIdle());
                }
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
