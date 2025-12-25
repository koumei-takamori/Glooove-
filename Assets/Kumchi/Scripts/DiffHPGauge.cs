// -------------------------------------------------------------
//
// DiffHPGauge.cs
// 池田桜輔
// 2025/09/19
// ダメージを受けたときのHPゲージのマスク
// どれくらいダメージを受けたかを表示するための赤いゲージ
// マスク範囲を加速度的に変更することでanimation
//
// -------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Ikeda
{
    public class DiffHPGauge
    {
        // ↓[変数]

        // ゲージのマスク
        RectMask2D m_mask;

        // 目標のHP値
        float m_targetHPvalue = HPGaugeMaskConst.MAX_HP;

        // アニメーション中かどうか
        bool m_isAnimating = false;

        // 現在の速度
        float m_speed = 0f;


        // ---------------------------
        // ↓[関数]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mask">対象のマスク</param>
        public DiffHPGauge(RectMask2D mask)
        {
            m_mask = mask;
        }

        /// <summary>
        /// 赤ゲージを更新する
        /// HPが減少した際に呼び出し、目標値を設定する
        /// </summary>
        /// <param name="newHPvalue">新しいHP量</param>
        public void UpdateDiffGauge(float newHPvalue)
        {
            if (Mathf.Approximately(m_targetHPvalue, newHPvalue)) return;
            m_targetHPvalue = newHPvalue;
            m_isAnimating = true;
        }

        /// <summary>
        /// フレームごとの更新処理
        /// アニメーション中のみ更新を実行する
        /// </summary>
        public void Update()
        {
            if (!m_isAnimating) return;
            UpdateAnimation();
        }

        /// <summary>
        /// アニメーション更新処理
        /// 赤ゲージを加速度的に移動させてHP差分を表現する
        /// </summary>
        private void UpdateAnimation()
        {
            // 目標位置を算出
            float percent = Mathf.Clamp01(m_targetHPvalue / HPGaugeMaskConst.MAX_HP);
            float targetRight = Mathf.Lerp(HPGaugeMaskConst.MASK_MIN, HPGaugeMaskConst.MASK_MAX, percent);

            // 現在位置
            var padding = m_mask.padding;
            float currentRight = padding.z; // RectMask2D の right は z

            // 加速度で速度を増加（上限あり）
            m_speed += HPGaugeMaskConst.SPEED_ACC * Time.deltaTime;
            m_speed = Mathf.Min(m_speed, HPGaugeMaskConst.MAX_DECREASE_SPEED);

            // 移動方向
            float dir = Mathf.Sign(targetRight - currentRight);
            currentRight += dir * m_speed * Time.deltaTime;

            // 目標を超えたら停止
            if ((dir > 0 && currentRight >= targetRight) || (dir < 0 && currentRight <= targetRight))
            {
                currentRight = targetRight;
                m_isAnimating = false;
                m_speed = 0f;
            }

            // 適用
            padding.z = currentRight; // right を更新
            m_mask.padding = padding;
        }
    }
}
