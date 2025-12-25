//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/18
// <file>			ArmPlayerCoolDown.h
// <概要>		　　プレイヤーのクールダウン共通クラス
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerCoolDown
        {
            // クールタイム時間
            private float m_duration;
            // 現在の経過時間
            private float m_timer;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="duration">クールタイム時間</param>
            public ArmPlayerCoolDown(float duration)
            {
                m_duration = duration;
                m_timer = 0;
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            /// <param name="deltatime"></param>
            public void Update(float deltatime)
            {
                if(m_timer > 0)
                {
                    m_timer -= deltatime;
                }
            }
            /// <summary>
            /// 使用可能かどうか
            /// </summary>
            /// <returns></returns>
            public bool IsReady() => m_timer <= 0f;
            /// <summary>
            /// リセット処理
            /// </summary>
            public void Reset() => m_timer = m_duration;
        }
    }
}


