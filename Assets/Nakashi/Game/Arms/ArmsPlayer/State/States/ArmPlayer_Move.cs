//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayer_Move.h
// <概要>		　　プレイヤーの動きのみ　IState継承なし、一生回す
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputReceiver;


namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayer_Move
        {
            // 所属コントローラー
            private ArmPlayerController m_controller;

            // ターゲットの位置格納用
            Transform m_target;

            // 閾値
            float m_threshold = 35f;
            // ポーズの基準点
            private Vector3 rightPoseR = new Vector3(320, 75, 15);
            private Vector3 rightPoseL = new Vector3(320, 65, 180);
            private Vector3 leftPoseR = new Vector3(315, 278, 166);
            private Vector3 leftPoseL = new Vector3(319, 265, 350);
            private Vector3 frontPoseR = new Vector3(320, 0, 80);
            private Vector3 frontPoseL = new Vector3(320, 330, 290);
            private Vector3 backPoseR = new Vector3(310, 190, 260);
            private Vector3 backPoseL = new Vector3(315, 135, 100);

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="controller"></param>
            public ArmPlayer_Move(ArmPlayerController controller)
            {
                m_controller = controller;
                m_target = m_controller.Target;
            }
            /// <summary>
            /// 1フレーム毎の更新
            /// </summary>
            /// <param name="rightEuler">右オイラー角</param>
            /// <param name="leftEuler">左オイラー角</param>
            public void FixedUpdate(Vector3 rightEuler, Vector3 leftEuler)
            {

                if (m_controller.GetPlayerStatus().GetSetControll == true) { return; }

                // 所属コントローラーの速度を0にする
                m_controller.GetSetVelocity = Vector3.zero;

                // プレイヤーの向き
                Transform trans = m_controller.GetTransform();

                // ターゲットの基準ベクトル
                Vector3 forward = trans.forward;
                Vector3 right = trans.right;
                Vector3 back = -forward;
                Vector3 left = -right;

                // 右方向への傾き
                if (IsPoseMatch(rightEuler, rightPoseR) && IsPoseMatch(leftEuler, rightPoseL))
                {
                    m_controller.GetSetVelocity += right;
                }
                // 左方向への傾き
                if (IsPoseMatch(rightEuler, leftPoseR) && IsPoseMatch(leftEuler, leftPoseL))
                {
                    m_controller.GetSetVelocity += left;
                }
                // 前方向への傾き
                if (IsPoseMatch(rightEuler, frontPoseR) && IsPoseMatch(leftEuler, frontPoseL))
                {
                    m_controller.GetSetVelocity += forward;
                }
                // 後ろ方向への傾き
                if (IsPoseMatch(rightEuler, backPoseR) && IsPoseMatch(leftEuler, backPoseL))
                {
                    m_controller.GetSetVelocity += back;

                }

                Rigidbody rb = m_controller.GetRigidbody();
                Vector3 moveDir = m_controller.GetSetVelocity.normalized;
                float speed = m_controller.GetPlayerData().GetWalkSpeed();
                if (!m_controller.IsGround()) { speed *= 0.5f; }

                // XZ方向だけ AddForce
                Vector3 force = moveDir * speed * Time.deltaTime;
                force.y = 0;
                rb.AddForce(force, ForceMode.VelocityChange);

                WalkingAnimation();
            }

            /// <summary>
            /// 現在のポーズがマッチしているかどうかの判定用
            /// </summary>
            /// <param name="current"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            private bool IsPoseMatch(Vector3 current, Vector3 target)
            {
                return
                    AngleDiff(current.x, target.x) < m_threshold &&
                    AngleDiff(current.z, target.z) < m_threshold;

            }


            /// <summary>
            /// Unityのオイラー角は360度のラップされるので、それを越した場合の変化時も対応するための関数
            /// </summary>
            /// <returns></returns>
            private float AngleDiff(float a, float b)
            {
                float diff = Mathf.Abs(a - b) % 360;
                return diff > 180 ? 360 - diff : diff;
            }

            /// <summary>
            /// デバッグ用の移動更新
            /// </summary>
            public void DebugUpdate()
            {
                if (m_controller.GetPlayerStatus().GetSetControll == true) { return; }

                // InputReceiverがnullの場合は処理しない
                if (m_controller.InputReceiver == null)
                {
                    Debug.LogWarning("InputReceiver is null!  デバッグ移動が使用できません。");
                    return;
                }

                // 所属コントローラーのVelocityを0にする
                m_controller.GetSetVelocity = Vector3.zero;

                // プレイヤーの向き取得
                Transform trans = m_controller.GetTransform();

                // 前後左右設定
                Vector3 forward = trans.forward;
                Vector3 right = trans.right;
                Vector3 back = -forward;
                Vector3 left = -right;

                // ★★★ 修正：PlayerInputReceiverから移動入力を取得 ★★★
                Vector2 moveInput = m_controller.InputReceiver.GetInputValue<Vector2>(PlayerInputReceiver.Actions.MOVE);

                // 移動入力に基づいて速度を設定
                if (moveInput.y > 0.1f) // 前方（W or ↑）
                {
                    m_controller.GetSetVelocity += forward * moveInput.y;
                }
                if (moveInput.y < -0.1f) // 後方（S or ↓）
                {
                    m_controller.GetSetVelocity += back * Mathf.Abs(moveInput.y);
                }
                if (moveInput.x < -0.1f) // 左（A or ←）
                {
                    m_controller.GetSetVelocity += left * Mathf.Abs(moveInput.x);
                }
                if (moveInput.x > 0.1f) // 右（D or →）
                {
                    m_controller.GetSetVelocity += right * moveInput.x;
                }

                // 速度の正規化
                if (m_controller.GetSetVelocity.sqrMagnitude > 0.01f)
                {
                    m_controller.GetSetVelocity.Normalize();
                }

                Rigidbody rb = m_controller.GetRigidbody();
                Vector3 moveDir = m_controller.GetSetVelocity.normalized;
                float speed = m_controller.GetPlayerData().GetWalkSpeed();
                if (!m_controller.IsGround()) { speed *= 0.5f; }

                // XZ方向だけ AddForce
                Vector3 force = moveDir * speed * Time.deltaTime;
                force.y = 0;
                rb.AddForce(force, ForceMode.VelocityChange);

                WalkingAnimation();
            }

            /// <summary>
            /// 移動アニメーション
            /// </summary>
            private void WalkingAnimation()
            {
                Rigidbody rb = m_controller.GetRigidbody();
                Vector3 vel = rb.velocity;
                vel.y = 0.0f;

                // 止まっているなら0
                if (vel.sqrMagnitude < 0.001f)
                {
                    m_controller.GetAnimator().SetFloat("WalkSpeedX", 0.0f, 0.1f, Time.deltaTime);
                    m_controller.GetAnimator().SetFloat("WalkSpeedZ", 0.0f, 0.1f, Time.deltaTime);
                    return;
                }

                Vector3 dir = vel.normalized;
                Transform trans = m_controller.transform;

                float moveX = Vector3.Dot(dir, trans.right);
                float moveZ = Vector3.Dot(dir, trans.forward);

                if (m_controller.IsGround())
                {
                    // どっちの成分が強いかでスナップ
                    if (Mathf.Abs(moveX) > Mathf.Abs(moveZ))
                    {
                        moveX = Mathf.Sign(moveX);
                        moveZ = 0.0f;
                    }
                    else
                    {
                        moveZ = Mathf.Sign(moveZ);
                        moveX = 0.0f;
                    }
                }

                m_controller.GetAnimator().SetFloat("WalkSpeedX", moveX, 0.1f, Time.deltaTime);
                m_controller.GetAnimator().SetFloat("WalkSpeedZ", moveZ, 0.1f, Time.deltaTime);
            }

        }
    }


}


