//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/07/30
// <file>			ArmPlayerController.h
// <概要>		　　プレイヤーコントローラー
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;
using UnityEngine.VFX;

namespace Nakashi
{
    namespace Player
    {
        public class ArmPlayerController : MonoBehaviour
        {
            // プレイヤー 0:1p 1:2ps
            [SerializeField]
            private int m_playerId = -1;

            // プレイヤーのデータ
            [SerializeField] private ArmPlayerData m_playerData;
            // プレイヤーステータス
            [SerializeField] private ArmPlayerStatus m_status;
            // カメラの基準
            [SerializeField] private Transform m_attackPoint;

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

            // 追加：プレイヤーのグローブ情報取得する
            public PlayerGloveData GetPlayerGloveData() => m_gloveData;

            // 追加：前フレームとの接地判定見る用
            private bool m_prevIsGround = true;
            [SerializeField] private VisualEffect landingVFX;


            //// 追加 : 左グローブ
            //private GloveBase m_leftglove;
            //// 追加 : 右グローブ
            //private GloveBase m_rightglove;

            [SerializeField]
            private GloveBase m_leftglove;
            // 追加 : 右グローブ
            [SerializeField]
            private GloveBase m_rightglove;




            // 追加：通常時の腕に着けるグローブオブジェクト
            private GameObject m_LGlove;
            private GameObject m_RGlove;

            // 追加：パリィ開始のキャッシュ
            private bool m_isParryStart = false;




            public StretchArm[] GetStretchArms() => new StretchArm[2]
            {
                m_leftglove.GetComponent<StretchArm>(), m_rightglove.GetComponent<StretchArm>()
            };


            // グローブの固定位置
            [SerializeField] private Transform m_leftglovePosition;
            [SerializeField] private Transform m_rightglovePosition;
            // 追加：通常時の腕にもグローブをつけるための位置（腕の先端のTransform）
            [SerializeField] private Transform m_leftArmPosition;
            [SerializeField] private Transform m_rightArmPosition;

            [SerializeField] Animator m_barrier;

            // 追加 : 回避行動検知
            DodgeChecker dodgeChecker;

            // 追加 : プレイヤーの入力を取得するクラス
            [SerializeField] private PlayerInputReceiver m_playerInputReceiver;



            private void Awake()
            {
                // プレイヤー登録
                PlayerRegistry.Instance.RegisterPlayer(this.gameObject);

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

                // 追加: 入力受け取りクラスの取得
                m_playerInputReceiver = this.GetComponent<PlayerInputReceiver>();
                if (m_playerInputReceiver == null)
                {
                    Debug.LogError("PlayerInputReceiver が見つかりません！同じGameObjectにアタッチしてください。");
                }


                //DebugStringSystem.Instance.AddVariable("Velocity", () => m_velocity);
                //DebugStringSystem.Instance.AddVariable("RightEuler", () => m_rightEuler);
                //DebugStringSystem.Instance.AddVariable("LeftEuler", () => m_leftEuler);

                // かかる重力値の変更のため、Gravityの使用をいったんなくす。
                m_rb.useGravity = false;



                // --- 追加: 回避行動検知用のスクリプト取得 ---

                // DodgeChecker の GameObject を取得
                GameObject dodgeCheckerObj = GameObject.Find("DodgeChecker");

                if (dodgeCheckerObj == null)
                {
                    Debug.LogError("DodgeChecker GameObject が見つかりません。 ikeda/Prefab/DodgeChecker をhierarchyの一番下に入れてください。");
                    return;
                }

                // DodgeChecker コンポーネントを取得
                dodgeChecker = dodgeCheckerObj.GetComponent<DodgeChecker>();

                // 全てが問題なく成功した場合はPlayerRegistryに登録
                PlayerRegistry.Instance.RegisterPlayer(this.gameObject);

                IsInitialized = true;
            }


            private void Start()
            {
                // 追加:通常時の腕にもグローブをつける
                // StretchArmからグローブオブジェクトを受け取る
                // 右腕


                // その子オブジェクトにグローブをセット
                GameObject rGlove = Instantiate(m_RGlove);
                rGlove.GetComponent<GloveObject>().Initialize(m_rightArmPosition.gameObject);
                rGlove.transform.SetParent(m_rightArmPosition, false);
                // オブジェクトを保存
                m_RGlove = rGlove;

                // 左腕


                // その子オブジェクトにグローブをセット
                GameObject lGlove = Instantiate(m_LGlove);
                lGlove.GetComponent<GloveObject>().Initialize(m_leftArmPosition.gameObject);
                lGlove.transform.SetParent(m_leftArmPosition, false);
                // オブジェクトを保存
                m_LGlove = lGlove;
                //// 確認：仮で生成
                //Instantiate(m_RGlove);
                //Instantiate(m_LGlove);

                // このスクリプトを持つオブジェクトの座標をデバッグ表示
                Debug.Log($"Start:Player {m_playerId} Position: {m_transform.position}");
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
                Vector3 gravity = m_playerData.GetGravityScale();


                // ステートマシンの更新
                m_stateMachine.Update();

                // ステート変更キー
                ChangeStateOnKey();
                // クールタイムの更新を行う
                m_coolTime.Update();
                // ステータスの更新
                m_status.Update();

                // グローブの位置を腕の先端に合わせる
                m_leftglove.transform.position = m_leftglovePosition.position;
                m_rightglove.transform.position = m_rightglovePosition.position;
                // このスクリプトを持つオブジェクトの座標をデバッグ表示
                //Debug.Log($"Update:Player {m_playerId} Position: {m_transform.position}");
                //Debug.Log("ジャンプ" + m_status.GetSetJump);
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
                m_playerMove.DebugUpdate();
                //#if UNITY_EDITOR
                //                m_playerMove.DebugUpdate();
                //#endif

                // ターゲットの方向を向く
                LookAtTarget();
                //重力をかける
                SetLocalGravity();
                // 前フレームとの接地判定をみて、着地かどうか判断する
                CheckLanding();
            }

            /// <summary>
            /// ステート変更キー
            /// </summary>
            private void ChangeStateOnKey()
            {
                // スタートフラグ中は処理しない
                if (!m_status.CanStart) { return; }

                // 追加:
                // ジャンプ入力を取得
                bool jumpInput = InputReceiver.GetInputButton(PlayerInputReceiver.Actions.JUMP, PlayerInputReceiver.InputType.PRESSED);
                // ダッシュ入力を取得
                bool dashInput = InputReceiver.GetInputButton(PlayerInputReceiver.Actions.DASH, PlayerInputReceiver.InputType.PRESSED);


                if (IsGround() && (jumpInput && m_coolTime.CanJump()))
                {
                    m_stateMachine.ChangeState(m_stateMachine.GetJump());
                    m_coolTime.StartJump();
                    SoundManager.Instance.PlaySE("JumpStart");

                    return;
                }
                if (dashInput && m_coolTime.CanDash())
                {
                    // 相手のグローブに回避行動を行ったことを通知
                    dodgeChecker.IsDodgeCheckerAction(this, m_transform.position);
                    SoundManager.Instance.PlaySE("Dash");

                    m_stateMachine.ChangeState(m_stateMachine.GetDash());
                    m_coolTime.StartDash();
                    return;
                }
                // 追加:
                // 左攻撃入力を取得
                bool leftAttackInput = InputReceiver.GetInputButton(PlayerInputReceiver.Actions.L_ATTACK, PlayerInputReceiver.InputType.PRESSED);
                // 右攻撃入力を取得
                bool rightAttackInput = InputReceiver.GetInputButton(PlayerInputReceiver.Actions.R_ATTACK, PlayerInputReceiver.InputType.PRESSED);

                // 追加：グローブデータ（腕）からStretchArmを取得
                StretchArm rightStretchArm = m_rightglove.GetComponent<StretchArm>();
                StretchArm leftStretchArm = m_leftglove.GetComponent<StretchArm>();


                // 追加: 攻撃状態に変更
                if (rightAttackInput && !rightStretchArm.IsStretching)
                {

                    m_stateMachine.ChangeState(m_stateMachine.GetRightAttack());
                    Debug.Log("Hおされた");
                }
                if (leftAttackInput && !leftStretchArm.IsStretching)
                {

                    m_stateMachine.ChangeState(m_stateMachine.GetLeftAttack());
                    Debug.Log("Gおされた");
                }

                // 追加:
                // パリィ入力を取得
                bool parryInput = InputReceiver.GetInputButton(PlayerInputReceiver.Actions.PARRY, PlayerInputReceiver.InputType.PRESSED);


                // パリィ状態        ↓↓この、Pかえるだけだと無理です。ごめ。ArmPlayer_ParryのほうのRelaseButtonも変えてね。
                if (parryInput)
                {

                    m_stateMachine.ChangeState(m_stateMachine.GetParry());
                    return;
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
                if (m_currentGauge >= m_playerData.GetMaxSpecialGauge()) { return; }
                m_currentGauge += upGauge;
            }

            /// <summary>
            /// ゲージの使用
            /// </summary>
            /// <param name="downGauge"></param>
            public void UseSpecialGauge(float downGauge)
            {
                if (m_currentGauge - downGauge < 0) { return; }
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
                leftglove.transform.SetParent(m_leftglovePosition, false);
                leftglove.transform.localPosition = m_leftglovePosition.localPosition;
                leftglove.transform.localRotation = Quaternion.identity;

                // 親のスケールに応じて補正
                //leftglove.transform.localScale = inverseParentScale;

                // Script取得
                m_leftglove = leftglove.GetComponent<GloveBase>();
                m_leftglove.GlovePosition = m_leftglovePosition.localPosition;

                // 右グローブ生成
                GameObject rightglove = Instantiate(m_status.GetGloveData.RightGlove);
                rightglove.transform.SetParent(m_rightglovePosition, false);
                rightglove.transform.localPosition = m_rightglovePosition.localPosition;
                rightglove.transform.localRotation = Quaternion.identity;

                // 同じく補正
                //rightglove.transform.localScale = inverseParentScale;

                // Script取得
                m_rightglove = rightglove.GetComponent<GloveBase>();
                m_rightglove.GlovePosition = m_rightglovePosition.localPosition;

                // StretchArmにArmPlayerControllerをセット
                m_leftglove.GetComponent<StretchArm>().OwnerArmPlayerController = this;
                m_rightglove.GetComponent<StretchArm>().OwnerArmPlayerController = this;
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
                if (IsGround())
                {
                    if (m_status.GetSetJump == true) SoundManager.Instance.PlaySE("JumpEnd");
                    m_status.GetSetJump = false;
                    m_animator.SetBool("Is_JumpEnd", true);
                }
                else { m_status.GetSetJump = true; m_animator.SetBool("Is_JumpEnd", false); }
            }
            /// <summary>
            /// 指定した Transform 配下で、一番深い（最奥）の Transform を取得する
            /// </summary>
            private Transform GetDeepestChild(Transform root)
            {
                Transform deepest = root;
                int maxDepth = 0;

                void Traverse(Transform current, int depth)
                {
                    if (depth > maxDepth)
                    {
                        maxDepth = depth;
                        deepest = current;
                    }

                    for (int i = 0; i < current.childCount; i++)
                    {
                        Traverse(current.GetChild(i), depth + 1);
                    }
                }

                Traverse(root, 0);
                return deepest;
            }

            private void CheckLanding()
            {
                bool isGroundNow = IsGround();

                if (!m_prevIsGround && isGroundNow)
                {
                    OnLanding();
                }
                m_prevIsGround = isGroundNow;
            }

            private void OnLanding()
            {
                Debug.Log("着地");

                if (landingVFX == null) { Debug.Log("接地VFXが入ってません"); return; }

                landingVFX.Stop();
                landingVFX.Play();
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

            public Animator GetBarrier { get { return m_barrier; } }

            // 追加：　生成完了通知用のプロパティ
            public bool IsInitialized { get; private set; } = false;

            // 攻撃する位置
            public Transform AttackPoint { get { return m_attackPoint; } }

            public int PlayerId { get { return m_playerId; } set { m_playerId = value; } }

            // 追加：プレイヤーの入力を取得するクラス
            public PlayerInputReceiver InputReceiver { get { return m_playerInputReceiver; } }
            //// 追加：ArmChangerから通常時の腕についているグローブオブジェクトを参照するための関数
            public GameObject SelectedLGlove { get { return m_LGlove; } set { m_LGlove = value; } }
            public GameObject SelectedRGlove { get { return m_RGlove; } set { m_RGlove = value; } }
        }

    }

}
