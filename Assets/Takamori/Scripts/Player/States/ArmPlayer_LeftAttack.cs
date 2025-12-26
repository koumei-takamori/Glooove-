/**********************************************************
 *
 *  ArmPlayer_LeftAttack.cs
 *  プレイヤーの攻撃状態
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/11/12
 *
 *********************************************************/
using UnityEngine;
using Nakashi.Player;


namespace Player
{
    public class ArmPlayer_LeftAttack : INakashiPlayerState
    {
        // 所属コントローラー
        private ArmPlayerController m_controller;

        // 腕を伸ばしたか
        private bool m_isExtend = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controller"></param>
        public ArmPlayer_LeftAttack(ArmPlayerController controller)
        {
            m_controller = controller;

        }
        /// <summary>
        /// 入出時
        /// </summary>
        public void Enter()
        {
            m_controller.LeftGlove.Use(m_controller,GloveActionType.NORMAL_ATTACK);
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
            m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetIdle());
        }

        /// <summary>
        /// ステート変更用関数
        /// </summary>
        private void ChangeState()
        {
            if (m_controller.IsGround() && (
                //Nakashi.Framework.VibrationSystem.Instance.GetNPad().GetButtonDown(nn.hid.NpadButton.L) || 
                Input.GetKeyDown(KeyCode.Space)))
            {
                m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GetJump());
                return;
            }
        }

        /// <summary>
        /// 物理演算との更新処理
        /// </summary>
        public void FixedUpdate()
        {

        }
        /// <summary>
        /// 腕を伸ばしたかを取得
        /// </summary>
        /// <returns></returns>
        public bool IsExtend() => m_isExtend;

    }
}



