// -------------------------------------------------------------
// BezierCurveData.cs
// ベジェ曲線の制御点と接線を保存する ScriptableObject
// 制作者 : 池田桜輔
// 改良日 : 2025/09/03
// -------------------------------------------------------------

using UnityEngine;

namespace Ikeda.Bezier
{
    [CreateAssetMenu(menuName = "Ikeda/BezierCurveData", fileName = "BezierCurveData")]
    public class BezierCurveData : ScriptableObject
    {
        // 制御点の配列
        public Vector3[] points = new Vector3[2]
        {
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0)
        };

        // 接線の配列
        public Vector3[] tangents = new Vector3[2]
        {
        Vector3.right * 0.5f,
        Vector3.left * 0.5f
        };
    }

}