// -------------------------------------------------------------
//
// CurrentHPGauge.cs
// 池田桜輔
// 2025/09/19
// 残りHPを表示するためのHPゲージ
// 現在のHPを即座に反映する緑色のゲージ
// Image.fillAmount を使用し、余計なアニメーションは行わない
//
// -------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Ikeda
{
    public class CurrentHPGauge
    {
        // ↓[変数]

        // 緑ゲージの Image
        private Image m_image;

        // 現在のHP割合（0.0 ～ 1.0）
        private float m_currentFill = 1f;

        // ---------------------------
        // ↓[関数]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="image">対象のImage（緑ゲージ）</param>
        /// <param name="maxHP">最大HP</param>
        public CurrentHPGauge(Image image, float maxHP)
        {
            m_image = image;

            // Image を Fill 用に設定
            m_image.type = Image.Type.Filled;
            m_image.fillMethod = Image.FillMethod.Horizontal;
            m_image.fillOrigin = (int)Image.OriginHorizontal.Left;

            // 初期状態は満タン
            m_currentFill = 1f;
            m_image.fillAmount = m_currentFill;
        }

        /// <summary>
        /// 緑ゲージを即時更新する
        /// </summary>
        /// <param name="newHPvalue">現在のHP</param>
        public void UpdateCurrentGauge(float newHPvalue)
        {
            // HPを割合（0～1）へ変換
            float rate = Mathf.Clamp01(newHPvalue / HPGaugeMaskConst.MAX_HP);

            // 変化がなければ何もしない
            if (Mathf.Approximately(m_currentFill, rate)) return;

            m_currentFill = rate;

            // 即時反映（アニメーションなし）
            m_image.fillAmount = m_currentFill;
        }
    }
}
