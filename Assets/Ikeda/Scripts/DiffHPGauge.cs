// -------------------------------------------------------------
//
// DiffHPGauge.cs
// 池田桜輔
// 2025/09/19
// ダメージを受けたときのHP差分ゲージ（赤）
// Image.fillAmount を使用してサイズ非依存で表現する
//
// -------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Ikeda
{
    public class DiffHPGauge
    {
        // ↓[変数]

        // 赤ゲージの Image
        private Image m_image;

        // 目標のHP割合（0.0 ～ 1.0）
        private float m_targetFill = 1f;

        // 現在のHP割合
        private float m_currentFill = 1f;

        // アニメーション中かどうか
        private bool m_isAnimating = false;

        // 現在の速度
        private float m_speed = 0f;

        // ---------------------------
        // ↓[関数]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="image">赤ゲージのImage</param>
        public DiffHPGauge(Image image)
        {
            m_image = image;

            // 初期状態は満タン
            m_image.type = Image.Type.Filled;
            m_image.fillMethod = Image.FillMethod.Horizontal;
            m_image.fillOrigin = (int)Image.OriginHorizontal.Left;
            m_image.fillAmount = 1f;
        }

        /// <summary>
        /// HPが減少したときに呼び出す
        /// </summary>
        /// <param name="hp">現在HP</param>
        public void UpdateDiffGauge(float hp)
        {
            // HPを割合（0～1）に変換
            float rate = Mathf.Clamp01(hp / HPGaugeMaskConst.MAX_HP);

            // 目標値が同じなら何もしない
            if (Mathf.Approximately(m_targetFill, rate)) return;

            m_targetFill = rate;
            m_isAnimating = true;
        }

        /// <summary>
        /// 毎フレーム更新
        /// </summary>
        public void Update()
        {
            if (!m_isAnimating) return;

            UpdateAnimation();
        }

        /// <summary>
        /// アニメーション処理
        /// </summary>
        private void UpdateAnimation()
        {
            // 加速度的に速度を増加
            m_speed += HPGaugeMaskConst.SPEED_ACC * Time.deltaTime;
            m_speed = Mathf.Min(m_speed, HPGaugeMaskConst.MAX_DECREASE_SPEED);

            // fillAmount を目標値へ近づける
            m_currentFill = Mathf.MoveTowards(
                m_currentFill,
                m_targetFill,
                m_speed * Time.deltaTime
            );

            // Image に反映
            m_image.fillAmount = m_currentFill;

            // 目標到達で停止
            if (Mathf.Approximately(m_currentFill, m_targetFill))
            {
                m_isAnimating = false;
                m_speed = 0f;
            }
        }
    }
}
