/**********************************************************
 *  BoomerangPunch.cs
 *  ベジェ曲線に沿ってブーメラングローブを往復移動させる
 *  （右手・左手で曲がり方向が反転する）
 *
 *  制作者 : 池田桜輔
 *  改良日 : 2025/09/04
 **********************************************************/

using UnityEngine;
using Nakashi.Player;   // ArmPlayerController 用
namespace Ikeda
{
    /// <summary>
    /// ブーメラングローブ
    /// </summary>
    public class BoomerangPunch : GloveBase
    {
        [Header("ベースとなる曲線データ (ScriptableObject)")]
        [SerializeField] private BezierCurveData m_curveData;

        [Header("プレイヤー参照")]
        [SerializeField] private Transform m_player;

        [Header("敵ターゲット")]
        [SerializeField] private Transform m_enemyTarget;

        [Header("左右設定 (true = 左手, false = 右手)")]
        [SerializeField] private bool m_isLeftHand = false;

        [Header("移動速度 (曲線上のtの進み具合)")]
        [SerializeField] private float m_moveSpeed = 1.5f;

        // 内部状態
        private Vector3 p0, p1, p2, p3;
        private float t;
        private bool returning = false;   // 復路かどうか
        private bool isMoving = false;    // 移動中かどうか
        private Transform originalParent; // 元の親を保存しておく

        /// <summary>
        /// アクション登録（GloveBase 抽象メソッド実装）
        /// </summary>
        protected override void RegisterActions()
        {
            // 1. 発射準備
            m_actionsDict[GloveActionType.NORMAL_ATTACK].Add(Action_Prepare);

            // 2. 往路移動
            m_actionsDict[GloveActionType.NORMAL_ATTACK].Add(Action_Forward);

            // 3. 復路移動
            m_actionsDict[GloveActionType.NORMAL_ATTACK].Add(Action_Return);

            // 4. 終了処理
            m_actionsDict[GloveActionType.NORMAL_ATTACK].Add(Action_End);
        }

        /// <summary>
        /// 使用開始
        /// </summary>
        public override bool Use(ArmPlayerController playerController, GloveActionType type)
        {
            // クールダウン等の共通判定
            if (!base.Use(playerController, type)) return false;
            if (m_curveData == null || m_player == null) return false;

            // 元の親を保存し、プレイヤーから外す
            originalParent = transform.parent;
            transform.SetParent(null);

            return true;
        }

        // ----------------------------
        // フェーズごとのアクション
        // ----------------------------

        /// <summary>
        /// 1. 発射準備
        /// </summary>
        private bool Action_Prepare()
        {
            p0 = transform.position;

            // 終点（敵 or 前方固定）
            Vector3 endPos = m_enemyTarget != null
                ? m_enemyTarget.position
                : p0 + m_player.forward * 5f;

            SetupCurve(p0, endPos);

            // 移動開始
            t = 0f;
            isMoving = true;
            returning = false;

            return true; // 次のフェーズへ
        }

        /// <summary>
        /// 2. 往路移動 (0 → 1)
        /// </summary>
        private bool Action_Forward()
        {
            if (!isMoving) return true;

            t += Time.deltaTime * m_moveSpeed;
            UpdatePosition(t);

            if (t >= 1f)
            {
                returning = true;
                t = 1f;
                return true; // 往路完了 → 復路へ
            }

            return false; // 継続
        }

        /// <summary>
        /// 3. 復路移動 (1 → 0)
        /// </summary>
        private bool Action_Return()
        {
            if (!isMoving) return true;

            t -= Time.deltaTime * m_moveSpeed;
            UpdatePosition(t);

            if (t <= 0f)
            {
                return true; // 復路完了 → 終了へ
            }

            return false;
        }

        /// <summary>
        /// 4. 終了処理
        /// </summary>
        private bool Action_End()
        {
            StopBoomerang();
            return true; // アクション完了
        }

        // ----------------------------
        // 補助関数
        // ----------------------------

        /// <summary>
        /// 座標更新
        /// </summary>
        private void UpdatePosition(float t)
        {
            transform.position = EvaluateCubicBezier(p0, p1, p2, p3, Mathf.Clamp01(t));
        }

        /// <summary>
        /// 曲線をセットアップする（p0 → p3）
        /// </summary>
        private void SetupCurve(Vector3 start, Vector3 end)
        {
            p0 = start;
            p3 = end;

            // スケール調整（距離に応じて曲線を拡大）
            float dist = Vector3.Distance(p0, p3);
            float baseDist = Vector3.Distance(m_curveData.points[0], m_curveData.points[1]);
            float scale = (baseDist > 0f) ? dist / baseDist : 1f;

            // プレイヤーの回転を反映
            p1 = p0 + (m_player.rotation * m_curveData.tangents[0] * scale);
            p2 = p3 + (m_player.rotation * m_curveData.tangents[1] * scale);

            // 左手なら曲がり方向を反転
            if (m_isLeftHand)
            {
                Vector3 dir = (p3 - p0).normalized;
                Quaternion flip = Quaternion.AngleAxis(180f, dir);
                p1 = p0 + flip * (p1 - p0);
                p2 = p3 + flip * (p2 - p3);
            }
        }

        /// <summary>
        /// 三次ベジェ曲線の評価
        /// </summary>
        private Vector3 EvaluateCubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            float u = 1 - t;
            return u * u * u * a
                 + 3 * u * u * t * b
                 + 3 * u * t * t * c
                 + t * t * t * d;
        }

        /// <summary>
        /// ブーメラン終了処理
        /// </summary>
        private void StopBoomerang()
        {
            isMoving = false;
            returning = false;
            t = 0f;

            // プレイヤーの子に戻す
            if (originalParent != null)
                transform.SetParent(originalParent);
        }
    }
}
