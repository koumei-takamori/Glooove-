// ------------------------------------------------------------------------------
// * 
// * InGameCountDown.cs
// * プレイ中のカウントダウンを管理するクラス
// *
// * 作成者: 池田桜輔
// * 作成日: 2025/08/01
// *
// ------------------------------------------------------------------------------

using TMPro;
using UnityEngine;

public class InGameCountDown : MonoBehaviour
{
    [Header("カウントダウン表示用テキスト (必須)")]
    [SerializeField]
    private TextMeshProUGUI countDownText = null;

    // カウントダウンの初期時間（秒）
    float initialCountDownTime = 20f;

    // 現在の残り時間（秒）
    float currentTime;

    // アニメーターコンポーネント
    Animator animator;

    // 10カウントアニメーションを実行したか
    bool hasTenCountAnimationPlayed = false;


    private void Awake()
    {
        // countDownText がインスペクター未設定なら検索で解決を試みる
        if (countDownText == null)
        {
            var uiCanvas = GameObject.Find("UICanvas");
            if (uiCanvas == null)
            {
                Debug.LogError("[InGameCountDown] UICanvas が見つかりません。");
                return;
            }

            var inPlay = uiCanvas.transform.Find("InPlayUI");
            if (inPlay == null)
            {
                Debug.LogError("[InGameCountDown] InPlay オブジェクトが UICanvas 内に見つかりません。");
                return;
            }

            var timeObj = inPlay.transform.Find("TimerUI");
            if (timeObj == null)
            {
                Debug.LogError("[InGameCountDown] TimerUI オブジェクトが UICanvas 内に見つかりません。");
                return;
            }

            countDownText = timeObj.GetComponent<TextMeshProUGUI>();
            if (countDownText == null)
            {
                Debug.LogError("[InGameCountDown] Time(text) に TextMeshProUGUI がアタッチされていません。");
                return;
            }

            // 親オブジェクトにあるAnimatorコンポーネントを取得
            animator = inPlay.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("[InGameCountDown] InPlayUI に Animator コンポーネントがありません。");
                return;
            }
        }

        // カウントダウンの初期化
        currentTime = initialCountDownTime;
        UpdateCountDownText();
    }


    private void Update()
    {
        // 残り時間があれば減らす
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            // マイナスにならないように clamp
            if (currentTime < 0f)
                currentTime = 0f;

            UpdateCountDownText();
        }

        // 10カウントアニメーションの再生の条件を満たしているか
        if (currentTime < 10f && !hasTenCountAnimationPlayed)
        {
            // 10カウントアニメーションを再生
            hasTenCountAnimationPlayed = true;
            animator.Play("10Count");
        }
    }

    /// <summary>
    /// カウントダウンの残り時間を UI に反映する
    /// </summary>
    private void UpdateCountDownText()
    {
        if (countDownText != null)
        {
            // 秒数を整数化して表示
            int displayTime = Mathf.CeilToInt(currentTime);
            countDownText.text = displayTime.ToString();
        }
    }
}
