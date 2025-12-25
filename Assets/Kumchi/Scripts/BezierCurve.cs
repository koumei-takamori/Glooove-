// -------------------------------------------------------------
// BezierCurve.cs
// ScriptableObject(BezirCurveData) を参照して
// シーンビューでベジェ曲線を可視化するコンポーネント
// 制作者 : 池田桜輔
// 改良日 : 2025/09/03
// -------------------------------------------------------------

using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    // 曲線データ（ScriptableObject）
    public BezierCurveData curveData;

    // 曲線を閉じるかどうか
    public bool closed = false;

    // 曲線の太さ
    public float lineWidth = 2f;

    // 接線を表示するかどうか
    public bool showTangents = true;

    // 接線の太さ
    public float tangentLineWidth = 1f;


}
