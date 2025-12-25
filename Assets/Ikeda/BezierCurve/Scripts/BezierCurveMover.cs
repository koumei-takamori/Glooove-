/**********************************************************
 *  BezierCurveMover.cs
 *  ScriptableObject のベジェ曲線データを元に
 *  任意の Transform を曲線上で制御するクラス（非Mono）
 *
 *  制作者 : 池田桜輔
 *  改良案 : 2025/10/09
 **********************************************************/
using UnityEngine;

namespace Ikeda.Bezier
{
    public class BezierCurveMover
    {
        private readonly BezierCurveData m_curveData;
        private readonly Transform m_origin; // 基準方向（例：プレイヤー）
        private readonly bool m_isLeft;

        private Vector3 p0, p1, p2, p3;

        private float m_t = 0f;
        private float m_speed = 1f;
        private bool m_forward = true;

        public bool IsFinished => (m_forward && m_t >= 1f) || (!m_forward && m_t <= 0f);

        public BezierCurveMover(BezierCurveData data, Transform origin, bool isLeft)
        {
            m_curveData = data;
            m_origin = origin;
            m_isLeft = isLeft;
        }

        /// <summary>
        /// 開始点と終点を設定（実行時にセットアップ）
        /// </summary>
        public void Setup(Vector3 start, Vector3 end)
        {
            p0 = start;
            p3 = end;

            float dist = Vector3.Distance(p0, p3);
            float baseDist = Vector3.Distance(m_curveData.points[0], m_curveData.points[^1]);
            float scale = (baseDist > 0f) ? dist / baseDist : 1f;

            // 回転を反映した制御点設定
            p1 = p0 + (m_origin.rotation * m_curveData.tangents[0] * scale);
            p2 = p3 + (m_origin.rotation * m_curveData.tangents[^1] * scale);

            // 左右反転対応
            if (m_isLeft)
            {
                Vector3 dir = (p3 - p0).normalized;
                Quaternion flip = Quaternion.AngleAxis(180f, dir);
                p1 = p0 + flip * (p1 - p0);
                p2 = p3 + flip * (p2 - p3);
            }

            m_t = 0f;
            m_forward = true;
        }

        /// <summary>
        /// 移動を進めて座標を返す
        /// </summary>
        public Vector3 Update(float deltaTime)
        {
            float dir = m_forward ? 1f : -1f;
            m_t += deltaTime * m_speed * dir;
            m_t = Mathf.Clamp01(m_t);
            return Evaluate(p0, p1, p2, p3, m_t);
        }

        /// <summary>
        /// t指定で曲線上の座標を取得（SplineUpdater用）
        /// </summary>
        public Vector3 Evaluate(float t)
        {
            return Evaluate(p0, p1, p2, p3, Mathf.Clamp01(t));
        }

        /// <summary>
        /// 往復の切り替え
        /// </summary>
        public void Reverse() => m_forward = !m_forward;

        /// <summary>
        /// 移動速度設定
        /// </summary>
        public void SetSpeed(float s) => m_speed = Mathf.Max(0.01f, s);

        private static Vector3 Evaluate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            float u = 1 - t;
            return u * u * u * a + 3 * u * u * t * b + 3 * u * t * t * c + t * t * t * d;
        }
    }
}
