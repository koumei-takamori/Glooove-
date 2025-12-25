using UnityEngine;

/// <summary>
/// World空間Canvasを常にカメラ正面へ向けるクラス
/// RectTransform対応
/// </summary>
public class BillboardCanvas : MonoBehaviour
{
    /// <summary>
    /// 参照するカメラ
    /// </summary>
    [SerializeField]
    private Camera targetCamera;

    /// <summary>
    /// RectTransform参照
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        /// RectTransformを取得
        rectTransform = GetComponent<RectTransform>();

        /// カメラ未設定時はメインカメラを使用
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    /// <summary>
    /// 毎フレーム処理
    /// </summary>
    private void LateUpdate()
    {
        /// カメラ位置を見る
        rectTransform.LookAt(targetCamera.transform);

        /// 裏向き防止のためY軸を反転
        rectTransform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
