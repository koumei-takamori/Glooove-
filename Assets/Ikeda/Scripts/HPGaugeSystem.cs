// -----------------------------------------------------------
//
// HPGaugeSystem.cs
// 池田桜輔
// 2025/09/18
// HPGaugeGroup.prefabにアタッチ
// HPゲージの名称設定　名前を統括する
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
    const string HP_GAUGE_MASK = "HPGauge/CurrentHPMask";
    const string DIFF_GAUGE_MASK = "HPGauge/DiffHPMask";

    // ↓[変数]
    // キャラクターのHP
    public int HP { private get; set; } = (int)HPGaugeMaskConst.MAX_HP;

    // キャラクターの名前
    public string FighterName { private get; set; }

    // UI要素
    private TextMeshProUGUI fighterNameText;    // 名前テキスト
    private RectMask2D hpGaugeUIMask;           // HPゲージのマスク                        (緑色のバー)
    private RectMask2D diffHPMask;              // ダメージを受けたときのHPゲージのマスク  (赤色のバー)

    // HPゲージ関連
    private CurrentHPGauge currentHPGauge; // 現在のHPゲージ
    private DiffHPGauge diffHPGauge;       // ダメージを受けたときのHPゲージ

    // ↓[関数]

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        // UI要素の取得
        FindUIElesments();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        diffHPGauge.Update();
    }

    /// <summary>
    /// UI要素の取得
    /// </summary>
    private void FindUIElesments()
    {
        // 名前テキストを取得(子オブジェクト)
        fighterNameText = transform.Find(FIGHTER_NAME_TEXT_BOX).GetComponent<TextMeshProUGUI>();

        // HPゲージのマスクを取得(子オブジェクト)
        hpGaugeUIMask = transform.Find(HP_GAUGE_MASK).GetComponent<RectMask2D>();

        // ダメージゲージのマスクを取得(子オブジェクト)
        diffHPMask = transform.Find(DIFF_GAUGE_MASK).GetComponent<RectMask2D>();

        if (fighterNameText == null || hpGaugeUIMask == null || diffHPMask == null)
        {
            Debug.LogError("HPGaugeSystem: UI要素の取得に失敗しました。");
            return;
        }

        // HPゲージ関連の初期化
        currentHPGauge = new CurrentHPGauge(hpGaugeUIMask, HP);
        diffHPGauge = new DiffHPGauge(diffHPMask);
    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// HPを減少させ、ゲージを更新する
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        // HP減少（0未満にならないよう制限）
        HP = Mathf.Max(0, HP - damage);

        // 緑ゲージを即時反映
        currentHPGauge.UpdateCurrentGauge(HP);

        // 赤ゲージをアニメーションさせて追従
        diffHPGauge.UpdateDiffGauge(HP);

        Debug.Log($"HPGaugeSystem: {FighterName}が{damage}のダメージを受けた。残りHP: {HP}");
    }
}
