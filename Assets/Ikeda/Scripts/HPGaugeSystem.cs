// -----------------------------------------------------------
//
// HPGaugeSystem.cs
// 池田桜輔
// 2025/09/18
// HPGaugeGroup.prefab にアタッチ
// HPゲージ全体の管理クラス
//
// -----------------------------------------------------------

using Ikeda;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPGaugeSystem : MonoBehaviour
{
    // ↓[定数]
    // UI要素の名前
    const string FIGHTER_NAME_TEXT_BOX = "NameTextBox";
    const string CURRENT_HP_IMAGE = "HPGauge/CurrentHPBar";
    const string DIFF_HP_IMAGE = "HPGauge/DiffHPBar";

    // ↓[変数]

    // キャラクターのHP
    public int HP { get; private set; } = (int)HPGaugeMaskConst.MAX_HP;

    // キャラクターの名前
    public string FighterName { private get; set; }

    // UI要素
    private TextMeshProUGUI fighterNameText;
    private Image currentHPImage;   // 緑ゲージ
    private Image diffHPImage;      // 赤ゲージ

    // ゲージ制御
    private CurrentHPGauge currentHPGauge;
    private DiffHPGauge diffHPGauge;

    // ---------------------------
    // ↓[関数]

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        FindUIElements();
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    private void Update()
    {
        diffHPGauge.Update();
    }

    /// <summary>
    /// UI要素取得
    /// </summary>
    private void FindUIElements()
    {
        // 名前テキスト
        fighterNameText = transform.Find(FIGHTER_NAME_TEXT_BOX)
                                   .GetComponent<TextMeshProUGUI>();

        // HPゲージImage取得
        currentHPImage = transform.Find(CURRENT_HP_IMAGE)
                                  .GetComponent<Image>();

        diffHPImage = transform.Find(DIFF_HP_IMAGE)
                               .GetComponent<Image>();

        if (fighterNameText == null || currentHPImage == null || diffHPImage == null)
        {
            Debug.LogError("HPGaugeSystem: UI要素の取得に失敗しました。");
            return;
        }

        // ゲージ初期化
        currentHPGauge = new CurrentHPGauge(currentHPImage, HP);
        diffHPGauge = new DiffHPGauge(diffHPImage);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        // HP減少（0未満防止）
        HP = Mathf.Max(0, HP - damage);

        // 緑ゲージ即時反映
        currentHPGauge.UpdateCurrentGauge(HP);

        // 赤ゲージ追従アニメーション
        diffHPGauge.UpdateDiffGauge(HP);
    }
}
