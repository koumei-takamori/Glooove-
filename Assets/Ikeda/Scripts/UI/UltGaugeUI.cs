// -----------------------------------------------
//
// UltGaugeUI.cs
// ウルトのゲージ（三角形）を制御するスクリプト
// 作成者: 池田
// 作成日: 2025/10/03
// HPGaugeUI / UltGroup / UltGaugeにアタッチ
// 
// -----------------------------------------------


using UnityEngine;

public class UltGaugeUI : MonoBehaviour
{
    // ウルトゲージの表示サイズ
    [SerializeField, Header("Ultのゲージ(RectTransform)")]
    RectTransform _ultIcon;

    // ウルトゲージイメージ
    [SerializeField, Header("Ultゲージの色(Image)")]
    UnityEngine.UI.Image _ultGauge;

    // ウルトゲージの値(0から1)
    [Range(0f, 1f)]
    [SerializeField] float _ultValue = 0f;

    // ウルトゲージ(0から1)
    public float UltGauge
    {
        get => _ultValue;
        set => _ultValue = Mathf.Clamp01(value);
    }

    // 色：満タン時／通常時（固定値）
    private static readonly Vector4 FULL_COLOR = new Vector4(1f, 1f, 0f, 1f);
    private static readonly Vector4 NOT_FULL_COLOR = new Vector4(1f, 1f, 1f, 1f);



    [SerializeField, Header("ウルト最大時炎エフェクト(GameObject)")]
    GameObject _ultFull;

    [SerializeField, Header("ウルト最大時の強調エフェクト(Animator)")]
    Animator _ultHighlit;




    void Start()
    {
        const string ERROR_LOG_SUFFIX = " がUltGroup / UltGaugeUIの Inspectorで設定されていません。";

        // インスペクターをチェック
        if (_ultIcon == null)
            Debug.LogError("UltのIcon(RectTransform)" + ERROR_LOG_SUFFIX);

        if (_ultFull == null)
            Debug.LogError("ウルト最大時炎エフェクト(GameObject)" + ERROR_LOG_SUFFIX);

        if (_ultHighlit == null)
            Debug.LogError("ウルト最大時の強調エフェクト(Animator)" + ERROR_LOG_SUFFIX);

        if (_ultIcon == null)
            Debug.LogError("UltのIcon(RectTransform)" + ERROR_LOG_SUFFIX);

        if (_ultGauge == null)
            Debug.LogError("Ultゲージの色(Image)" + ERROR_LOG_SUFFIX);

        // 初期状態の設定
        _ultGauge.color = NOT_FULL_COLOR;
        _ultFull.SetActive(false);
    }


    void Update()
    {
        // ウルトゲージのサイズを更新
        _ultIcon.localScale = new Vector3(_ultValue, _ultValue, _ultValue);

        // ウルトゲージが満タンのとき
        if (_ultValue >= 1f)
        {
            _ultFull.SetActive(true);
            _ultGauge.color = FULL_COLOR;

            // 強調エフェクトを再生
            _ultHighlit.SetBool("isFull", true);
        }
        // ウルトゲージが満タンでないとき
        else
        {
            _ultFull.SetActive(false);
            _ultGauge.color = NOT_FULL_COLOR;

            // 強調エフェクトを停止
            _ultHighlit.SetBool("isFull", false);
        }
    }
}
