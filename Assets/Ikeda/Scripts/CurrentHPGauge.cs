// -------------------------------------------------------------
//
// CurrentHPGauge.cs
// 池田桜輔
// 2025/09/19
// 残りHP表示するためのHPゲージ
// 現在のHPを表示するための緑色のゲージ
// マスク範囲を常に残りHPに併せて変更　余計なanimationはしない
//
// -------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Ikeda
{
    public class CurrentHPGauge
    {
        // ↓[変数]

        // ゲージのマスク
        RectMask2D m_mask;
        // 現在のHP値
        float m_currentHPvalue = HPGaugeMaskConst.MAX_HP;

        // ↓[関数]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mask">対象のマスク</param>
        /// <param name="maxHP">最大HP</param>
        public CurrentHPGauge(RectMask2D mask, float maxHP)
        {
            m_mask = mask;
            m_currentHPvalue = maxHP;
        }

        /// <summary>
        /// 緑ゲージを更新する
        /// アニメーションせず即座に現在のHPを反映する
        /// </summary>
        /// <param name="newHPvalue">新しいHP量</param>
        public void UpdateCurrentGauge(float newHPvalue)
        {
            if (Mathf.Approximately(m_currentHPvalue, newHPvalue)) return;
            m_currentHPvalue = newHPvalue;

            float percent = Mathf.Clamp01(m_currentHPvalue / HPGaugeMaskConst.MAX_HP);
            float newRight = Mathf.Lerp(HPGaugeMaskConst.MASK_MIN, HPGaugeMaskConst.MASK_MAX, percent);

            var padding = m_mask.padding;
            padding.z = newRight; // RectMask2D の right は z
            m_mask.padding = padding;
        }
    }
}
