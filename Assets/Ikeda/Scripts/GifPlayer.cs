using UnityEngine;
using UnityEngine.UI;

public class GifPlayer : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;    // 表示対象のUI
    [SerializeField] private int frameCount = 32;  // フレーム数（横に並んでいる数）
    [SerializeField] private float frameRate = 15f; // 1秒あたりのフレーム数
    [SerializeField] private Texture2D spriteSheet; // 横並びのスプライトシートPNG

    private float timer = 0f;
    private int currentFrame = 0;

    void Start()
    {
        // スプライトシートをRawImageにセット
        rawImage.texture = spriteSheet;

        // 最初のフレーム（左端）を表示
        rawImage.uvRect = new Rect(0f, 0f, 1f / frameCount, 1f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;

            // 次のフレームへ
            currentFrame = (currentFrame + 1) % frameCount;

            // UV座標を更新してフレームを切り替える
            rawImage.uvRect = new Rect(
                (float)currentFrame / frameCount, // 左端をずらす
                0f,                               // Yは変えない（横一列だから）
                1f / frameCount,                  // 幅
                1f                                // 高さ（全体）
            );
        }
    }
}
