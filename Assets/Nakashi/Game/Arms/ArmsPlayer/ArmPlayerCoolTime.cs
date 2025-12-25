//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			ArmPlayerCoolTime.h
// <概要>		　　プレイヤーのクールタイム管理
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerCoolTime
        {
            // 所属コントローラー
            ArmPlayerController m_controller;

            /// <summary>
            /// ダッシュのクールタイム管理
            /// </summary>
            private readonly ArmPlayerCoolDown m_dashCoolDown;
            
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller">所属コントローラー</param>
            public ArmPlayerCoolTime(ArmPlayerController controller)
            {
                m_controller = controller;
                m_dashCoolDown = new ArmPlayerCoolDown(m_controller.GetPlayerData().GetDashCoolTime());
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            public void Update()
            {
                m_dashCoolDown.Update(Time.deltaTime);
            }

            /// <summary>
            /// ダッシュ可能かどうか
            /// </summary>
            /// <returns></returns>
            public bool CanDash()
            {
                return m_dashCoolDown.IsReady();
            }

            /// <summary>
            /// ダッシュ開始
            /// </summary>
            public void StartDash()
            {
                m_dashCoolDown.Reset();
            }

           
        }
    }
}

