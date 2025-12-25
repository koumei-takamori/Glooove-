/**********************************************************
 *  BezierToScaledSpline.cs
 *  ScriptableObject（BezierCurveData）をもとに
 *  正しいベジェ曲線→スプライン変換を検証するスクリプト
 *
 *  制作者 : 池田桜輔
 *  改良日 : 2025/11/04
 **********************************************************/

using UnityEngine;
using UnityEngine.Splines;

namespace Ikeda.Bezier
{
    [ExecuteAlways]
    [RequireComponent(typeof(SplineContainer))]
    public class BezierToScaledSpline : MonoBehaviour
    {
        [Header("=== ベジェカーブデータ ===")]
        [SerializeField] private BezierCurveData bezierData;

        [Header("=== 接続対象 ===")]
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        [Header("=== スケール設定 ===")]
        [Tooltip("ScriptableObject内の長さに対する倍率")]
        [SerializeField] private float scaleMultiplier = 1f;

        private SplineContainer splineContainer;

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                RebuildSpline();
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                RebuildSpline();
            }
        }

        /// <summary>
        /// ScriptableObjectのベジェデータからSplineを再構築
        /// </summary>
        private void RebuildSpline()
        {
            if (bezierData == null || startPoint == null || endPoint == null)
                return;

            if (splineContainer == null)
                splineContainer = GetComponent<SplineContainer>();

            var spline = splineContainer.Spline;
            spline.Clear();

            // 開始点・終了点
            Vector3 p0 = transform.InverseTransformPoint(startPoint.position);
            Vector3 p3 = transform.InverseTransformPoint(endPoint.position);

            // ScriptableObjectの基準長さ
            float baseDist = Vector3.Distance(bezierData.points[0], bezierData.points[^1]);
            float dist = Vector3.Distance(startPoint.position, endPoint.position);
            float scale = (baseDist > 0f ? dist / baseDist : 1f) * scaleMultiplier;

            // 接線ベクトルを変換
            Vector3 worldP1 = startPoint.position + (startPoint.rotation * bezierData.tangents[0] * scale);
            Vector3 worldP2 = endPoint.position + (endPoint.rotation * bezierData.tangents[^1] * scale);

            // ローカル空間に変換
            Vector3 p1 = transform.InverseTransformPoint(worldP1);
            Vector3 p2 = transform.InverseTransformPoint(worldP2);

            // Splineに設定
            spline.Add(new BezierKnot(
                p0,
                Vector3.zero,       // inTangent
                (p1 - p0),          // outTangent
                startPoint.rotation
            ));
            spline.Add(new BezierKnot(
                p3,
                (p2 - p3),          // inTangent
                Vector3.zero,       // outTangent
                endPoint.rotation
            ));

            spline.Closed = false;
        }
    }
}
