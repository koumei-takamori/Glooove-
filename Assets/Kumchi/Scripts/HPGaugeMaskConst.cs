// -------------------------------------------------------------
//
// HPGaugeMaskConst.cs
// 池田桜輔
// 2024/09/19
// HPゲージのマスクに関する定数をまとめたクラス
//
// -------------------------------------------------------------

namespace Ikeda
{
    public static class HPGaugeMaskConst
    {
        // ↓[固定値]

        // マスクのRightの最小値（HPが0のとき）
        public const float MASK_MIN = 177f;

        // マスクのRightの最大値（HPが満タンのとき）
        public const float MASK_MAX = -105f;

        // マスク範囲の合計値
        public const float TOTAL_MASK_RANGE = MASK_MAX - MASK_MIN;

        // 最大HP量（仮）
        public const float MAX_HP = 100f;

        // 最大減少速度
        public const float MAX_DECREASE_SPEED = 50f;

        // アニメーション用加速度
        public const float SPEED_ACC = 10f;
    }
}