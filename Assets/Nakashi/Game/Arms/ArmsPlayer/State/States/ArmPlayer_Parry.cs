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
            // パリィ音声再生フラグ
            private bool m_parrySEFlag = false;


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

                // アニメーション変更
                m_controller.GetAnimator().SetBool("Parry", true);
                // パリィ判定をオンにする
                m_controller.GetPlayerStatus().GetSetParry = true;
                // 動けなくする
                m_controller.GetPlayerStatus().GetSetControll = true;
                // Enableの変更
                m_controller.GetBarrier.SetBool("Reflect", true);

            }

            /// <summary>
            /// 退出時
            /// </summary>
            public void Exit()
            {
                // パリィ判定をオフにする
                m_controller.GetPlayerStatus().GetSetParry = false;
                m_controller.GetPlayerStatus().GetSetControll = false;
                // アニメーション変更
                m_controller.GetAnimator().SetBool("Parry", false);
                // Enableの変更
                m_controller.GetBarrier.SetBool("Reflect", false);
            }

            /// <summary>
            /// 更新時
            /// </summary>
            public void Update()
            {
                if (m_parrySEFlag == false)
                {
                    // 追加：パリィ開始音
                    SoundManager.Instance.PlaySE("ParryStart");
                    m_parrySEFlag = true;
                }
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
                bool parryInput = m_controller.InputReceiver.GetInputButton(PlayerInputReceiver.Actions.PARRY, PlayerInputReceiver.InputType.RELEASED);
                // キーが離れたら、アイドリング状態に戻す。
                if (parryInput)
                {
                    m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetIdle());
                    // 追加：パリィ解除音
                    SoundManager.Instance.PlaySE("ParryEnd");
                    // 追加：パリィ音声再生フラグリセット
                    m_parrySEFlag = false;
                }



            }
        }
    }

}
