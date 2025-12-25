//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayerController.h
// <概要>		　　プレイヤーコントローラー
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerController : MonoBehaviour
        {
            // プレイヤーのデータ
            [SerializeField] private ArmPlayerData m_playerData;
            // プレイヤーステータス
            [SerializeField] private ArmPlayerStatus m_status;

            // ステートマシン
            private ArmPlayerStateMachine m_stateMachine;

            // リジットボディ、トランスフォーム、当たり判定
            private Rigidbody m_rb;
            private Transform m_transform;
            private Collider m_collider;
            // アニメーター
            private Animator m_animator;

            // 速度
            private Vector3 m_velocity = Vector3.zero;

            // 現在のゲージ量
            private float m_currentGauge;

            // ターゲットの位置
            [SerializeField] private Transform m_target;

            // プレイヤーの動きのみ
            private ArmPlayer_Move m_playerMove;

            // プレイヤーのクールタイム計算用
            private ArmPlayerCoolTime m_coolTime;

            // 右左オイラー角
            //Vector3 m_rightEuler;
            //Vector3 m_leftEuler;


            // 追加：プレイヤーのグローブ情報
            [SerializeField]
            private PlayerGloveData m_gloveData;

            // 追加 : 左グローブ
            private GloveBase m_leftglove;
            // 追加 : 右グローブ
            private GloveBase m_rightglove;

            // 追加：キャラクターの操作可能フラグ
            private bool m_canControll = true;

            // グローブの固定位置
            private Vector3 m_leftglovePosition = new Vector3(-1, 0, 0);
            private Vector3 m_rightglovePosition = new Vector3(1, 0, 0);

            private void Start() 
            {
                // リジットボディ、トランスフォーム取得
                m_rb = this.GetComponent<Rigidbody>();
                m_transform = this.GetComponent<Transform>();
                m_collider = this.GetComponent<Collider>();
                m_animator = this.GetComponent<Animator>();

                // プレイヤーのステータスクラス
                m_status = new ArmPlayerStatus(this);

                // ステートマシーンの初期設定
                m_stateMachine = new ArmPlayerStateMachine(this);
                m_stateMachine.Initialize(m_stateMachine.GetIdle());

                // プレイヤーの動きクラス
                m_playerMove = new ArmPlayer_Move(this);
                // プレイヤーのクールタイム管理クラス
                m_coolTime = new ArmPlayerCoolTime(this);

                // 追加: グローブの設定
                GloveSetUp();

                //DebugStringSystem.Instance.AddVariable("Velocity", () => m_velocity);
                //DebugStringSystem.Instance.AddVariable("RightEuler", () => m_rightEuler);
                //DebugStringSystem.Instance.AddVariable("LeftEuler", () => m_leftEuler);

                // かかる重力値の変更のため、Gravityの使用をいったんなくす。
                m_rb.useGravity = false; 
            }

           
            /// <summary>
            /// 通常更新
            /// </summary>

            private void Update()
            {
                //m_rightEuler = Nakashi.Framework.AxisSystem.Instance.GetRightQuaternion().eulerAngles;
                //m_leftEuler = Nakashi.Framework.AxisSystem.Instance.GetLeftQuaternion().eulerAngles;
                // ジャンプ判定の測定
                CheckJumpNow();

                // ステートマシンの更新
                m_stateMachine.Update();

                // ステート変更キー
                ChangeStateOnKey();
                // クールタイムの更新を行う
                m_coolTime.Update();
                // ステータスの更新
                m_status.Update();

                Debug.Log("ジャンプ" + m_status.GetSetJump);
            }

            /// <summary>
            /// 当たり判定などの更新と合わせるための更新処理
            /// </summary>
            private void FixedUpdate()
            {
                // ステートマシンの、FixedUpdateを行う
                m_stateMachine.FixedUpdate();
                // プレイヤーの移動更新を行う
                //m_playerMove.FixedUpdate(m_rightEuler, m_leftEuler);
                // UnityのEditor上のみのデバッグ処理
                #if UNITY_EDITOR
                m_playerMove.DebugUpdate();
                #endif

                // ターゲットの方向を向く
                LookAtTarget();
                //重力をかける
                SetLocalGravity();
            }

            /// <summary>
            /// ステート変更キー
            /// </summary>
            private void ChangeStateOnKey()
            {
                if(IsGround() && (
                    //Nakashi.Framework.VibrationSystem.Instance.GetNPad().GetButtonDown(nn.hid.NpadButton.L) || 
                    Input.GetKeyDown(KeyCode.Space) &&
                    m_coolTime.CanJump()))
                {
                    m_stateMachine.ChangeState(m_stateMachine.GetJump());
                    m_coolTime.StartJump();
                    return;
                }
                if((/*Nakashi.Framework.VibrationSystem.Instance.GetNPad().GetButtonDown(nn.hid.NpadButton.R) || */
                    Input.GetKeyDown(KeyCode.LeftShift)) &&
                    m_coolTime.CanDash())
                {
                    m_stateMachine.ChangeState(m_stateMachine.GetDash());
                    m_coolTime.StartDash();
                    return;
                }
                
                // 追加: 攻撃状態に変更
                if (Input.GetKeyDown(KeyCode.H))
                {
                    
                    m_stateMachine.ChangeState(m_stateMachine.GetRightAttack());
                    Debug.Log("Hおされた");
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                   
                    m_stateMachine.ChangeState(m_stateMachine.GetLeftAttack());
                    Debug.Log("Gおされた");
                }

                
             
            }

            /// <summary>
            /// 重力値の変更をセットするための関数
            /// </summary>
            private void SetLocalGravity()
            {
                m_rb.AddForce(m_playerData.GetGravityScale(), ForceMode.Acceleration);
            }

            /// <summary>
            /// 地面接地判定
            /// </summary>
            /// <returns></returns>
            public bool IsGround()
            {
                // Rayを下に伸ばして、地面と当たったかどうかの判定を返すようにする
                Vector3 rayPosition = this.m_transform.position;
                Ray ray = new Ray(rayPosition, Vector3.down);
                Debug.DrawRay(ray.origin, ray.direction * GetPlayerData().GetRayDistance(), Color.red);
                return Physics.Raycast(ray, GetPlayerData().GetRayDistance());
            }

            /// <summary>
            /// ターゲットの方向を向く処理
            /// </summary>
            private void LookAtTarget()
            {
                // ターゲットの位置を向くようにTransformの変更
                Vector3 targetPos = m_target.position;
                targetPos.y = m_transform.position.y;
                m_transform.LookAt(targetPos);
            }

            /// <summary>
            /// ゲージ量アップ
            /// </summary>
            /// <param name="upGauge"></param>
            public void SpecialGaugeUp(float upGauge)
            {
                if(m_currentGauge >= m_playerData.GetMaxSpecialGauge()) { return; }
                m_currentGauge += upGauge;
            }

            /// <summary>
            /// ゲージの使用
            /// </summary>
            /// <param name="downGauge"></param>
            public void UseSpecialGauge(float downGauge)
            {
                if(m_currentGauge - downGauge < 0) { return; }
                m_currentGauge -= downGauge;
            }


            // 追加:グローブのセットアップ
            private void GloveSetUp()
            {
                // 親の実際のスケール（ワールド空間上の大きさ）を取得
                Vector3 parentScale = transform.lossyScale;

                // 0除算を防止（安全策）
                parentScale.x = Mathf.Approximately(parentScale.x, 0f) ? 1f : parentScale.x;
                parentScale.y = Mathf.Approximately(parentScale.y, 0f) ? 1f : parentScale.y;
                parentScale.z = Mathf.Approximately(parentScale.z, 0f) ? 1f : parentScale.z;

                // 親スケールの逆数を計算
                Vector3 inverseParentScale = new Vector3(
                    1f / parentScale.x,
                    1f / parentScale.y,
                    1f / parentScale.z
                );

                // 左グローブ生成
                GameObject leftglove = Instantiate(m_status.GetGloveData.LeftGlove);
                leftglove.transform.SetParent(transform, false);
                leftglove.transform.localPosition = m_leftglovePosition;
                leftglove.transform.localRotation = Quaternion.identity;

                // 親のスケールに応じて補正
                leftglove.transform.localScale = inverseParentScale;

                // Script取得
                m_leftglove = leftglove.GetComponent<GloveBase>();
                m_leftglove.GlovePosition = m_leftglovePosition;

                // 右グローブ生成
                GameObject rightglove = Instantiate(m_status.GetGloveData.RightGlove);
                rightglove.transform.SetParent(transform, false);
                rightglove.transform.localPosition = m_rightglovePosition;
                rightglove.transform.localRotation = Quaternion.identity;

                // 同じく補正
                rightglove.transform.localScale = inverseParentScale;

                // Script取得
                m_rightglove = rightglove.GetComponent<GloveBase>();
                m_rightglove.GlovePosition = m_rightglovePosition;
            }


            // 追加：グローブ情報設定する
            public void SetGolveData(PlayerGloveData gloveData)
            {
                m_gloveData = gloveData;
            }

            /// <summary>
            /// ジャンプ中か調べる
            /// </summary>
            private void CheckJumpNow()
            {
                if (IsGround()) { m_status.GetSetJump = false; m_animator.SetBool("Is_JumpEnd", true); }
                else { m_status.GetSetJump = true; m_animator.SetBool("Is_JumpEnd", false); }
            }

            public Rigidbody GetRigidbody() => m_rb;
            public Transform GetTransform() => m_transform;
            public Collider GetCollider() => m_collider;
            public Animator GetAnimator() => m_animator;
            public ArmPlayerStateMachine GetStateMachine() => m_stateMachine;
            public ArmPlayerData GetPlayerData() => m_playerData;
            public ArmPlayerStatus GetPlayerStatus() => m_status;
            //public Vector3 GetRightEuler() => m_rightEuler;
            //public Vector3 GetLeftEuler() => m_leftEuler;
            public Vector3 GetSetVelocity { get { return m_velocity; } set { m_velocity = value; } }
            public Transform Target { get { return m_target; } set { m_target = value; } }

            public ArmPlayerCoolTime PlayerCoolTime { get { return m_coolTime; } set { m_coolTime = value; } }

            // 追加：グローブのプロパティ
            public PlayerGloveData GloveData { get { return m_gloveData; } }
            public GloveBase LeftGlove { get { return m_leftglove; } }
            public GloveBase RigthGlove { get { return m_rightglove; } }
            public bool CanContoroll { get { return m_canControll; } set { m_canControll = value; } }

        }
    }

}


