// -------------------------------------------------------------
//
// HPGaugeMaskConst.cs
// 池田桜輔
// 2024/09/19
// HPゲージに関する定数をまとめたクラス
//
// -------------------------------------------------------------

namespace Ikeda
{
    public static class HPGaugeMaskConst
    {
        // ↓[固定値]

        // 最大HP量
        public const float MAX_HP = 100f;

        // 赤ゲージの最大減少速度（fillAmount / 秒）
        public const float MAX_DECREASE_SPEED = 1.0f;

        // 赤ゲージ用の加速度
        public const float SPEED_ACC = 2.0f;
    }
}
