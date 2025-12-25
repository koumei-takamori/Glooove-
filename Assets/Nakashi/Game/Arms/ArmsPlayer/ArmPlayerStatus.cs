//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			NakashiPlayerStatus.h
// <概要>		　　プレイヤーの状態設定
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerStatus : MonoBehaviour
        {
            // 所属コントローラー
            private ArmPlayerController m_controller;

            // 追加：プレイヤーのグローブ情報
            [SerializeField]
            private PlayerGloveData m_gloveData;

            public enum Condition
            {
                ALIVE = 0,  // 生きている
                DEAD = 1,   // 死んだ。
            }

            // プレイヤーの状態
            private Condition m_condition;

            // 現在のHP
            float m_hp;

            // 無敵判定用
            bool m_isInvincible;
       
            public bool GetSetInvincible { get { return m_isInvincible; } set { m_isInvincible = value; } }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller">所属コントローラー</param>
            public ArmPlayerStatus(ArmPlayerController controller)
            {
                m_controller = controller;
                m_hp = m_controller.GetPlayerData().GetMaxHP();
                m_gloveData = m_controller.GloveData;
            }

            /// <summary>
            /// 更新処理
            /// </summary>
            public void Update()
            {
            }

            // コンディションのゲッターセッター
            public Condition GetSetCondition { get { return m_condition; } set { m_condition = value; } }

            public float GetSetHp { get { return m_hp; } set { m_hp = value; } }

            // 追加：グローブの情報
            public PlayerGloveData GetGloveData { get { return m_gloveData; } }
        }

    }

}


