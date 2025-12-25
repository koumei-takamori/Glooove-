// -------------------------------------------------------------
// ProceduralSpringMesh.cs
// ScriptableObject（BezierCurveData）からベジェ曲線を生成し、
// プレイヤー→ターゲット間距離に合わせてSpline化＆メッシュ生成を行う
// 制作者 : 池田桜輔
// 改良日 : 2025/10/31
// -------------------------------------------------------------

using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Ikeda.Bezier
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SplineContainer))]
    public class ProceduralSpringMesh : MonoBehaviour
    {
        [Header("=== ベジェカーブデータ ===")]
        [SerializeField] private BezierCurveData bezierData;

        [Header("=== 接続対象 ===")]
        [SerializeField] private Transform player;   // アームの根元
        [SerializeField] private Transform target;   // 敵や接続先など

        [Header("=== メッシュ設定 ===")]
        [SerializeField, Tooltip("曲線方向の分割数")] private int segmentCount = 250;
        [SerializeField, Tooltip("チューブの半径")] private float radius = 0.1f;
        [SerializeField, Tooltip("円周方向の分割数")] private int radialSegments = 8;

        private SplineContainer splineContainer;
        private Mesh mesh;


        private void Awake()
        {
            splineContainer = GetComponent<SplineContainer>();
            mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        private void Update()
        {
            if (player != null && target != null)
            {
                RebuildSplineRuntime(player, target);
            }

            GenerateMesh();
        }

        /// <summary>
        /// スプラインを再構成
        /// </summary>
        private void RebuildSplineRuntime(Transform start, Transform end)
        {
            if (bezierData == null) return;

            var spline = splineContainer.Spline;
            spline.Clear();

            // プレイヤー・ターゲットのワールド座標をローカル空間に変換
            Vector3 p0 = transform.InverseTransformPoint(start.position);
            Vector3 p3 = transform.InverseTransformPoint(end.position);

            // ベースのベジェデータの長さと現在の距離からスケール比を算出
            float dist = Vector3.Distance(start.position, end.position);
            float baseDist = Vector3.Distance(bezierData.points[0], bezierData.points[^1]);
            float scale = baseDist > 0f ? dist / baseDist : 1f;

            // Tangentベクトルをプレイヤー・ターゲットの回転に応じてスケーリング
            Vector3 worldP1 = start.position + (start.rotation * bezierData.tangents[0] * scale);
            Vector3 worldP2 = end.position + (end.rotation * bezierData.tangents[^1] * scale);

            // Tangent制御点もローカル空間に変換
            Vector3 p1 = transform.InverseTransformPoint(worldP1);
            Vector3 p2 = transform.InverseTransformPoint(worldP2);


            // 開始点
            spline.Add(new BezierKnot(
                p0,
                Vector3.zero,       // 前方向（inTangent）はなし
                (p1 - p0),          // 次に向かう方向（outTangent）
                start.rotation
            ));

            // 終点
            spline.Add(new BezierKnot(
                p3,
                (p2 - p3),          // 前から入ってくる方向（inTangent）
                Vector3.zero,       // 次方向（outTangent）はなし
                end.rotation
            ));

            spline.Closed = false;
        }

        /// <summary>
        /// 現在のスプラインに沿ってチューブ状メッシュを生成
        /// </summary>
        private void GenerateMesh()
        {
            var spline = splineContainer.Spline;
            if (spline == null) return;

            var vertices = new Vector3[(segmentCount + 1) * (radialSegments + 1)];
            var triangles = new int[segmentCount * radialSegments * 6];

            // 各スプライン上の点に沿って円形断面を配置
            for (int i = 0; i <= segmentCount; i++)
            {
                float t = i / (float)segmentCount;
                float3 pos = spline.EvaluatePosition(t);
                float3 tangent = math.normalize(spline.EvaluateTangent(t));

                // 接線と直交する補助ベクトルを計算
                float3 bitangent = math.cross(tangent, new float3(0, 1, 0));
                if (math.lengthsq(bitangent) < 0.001f)
                    bitangent = math.cross(tangent, new float3(1, 0, 0));
                bitangent = math.normalize(bitangent);
                float3 normal = math.normalize(math.cross(bitangent, tangent));

                // 円周方向に頂点を生成
                for (int j = 0; j <= radialSegments; j++)
                {
                    float angle = (j / (float)radialSegments) * math.PI * 2f;
                    float3 offset = normal * math.cos(angle) * radius + bitangent * math.sin(angle) * radius;
                    vertices[i * (radialSegments + 1) + j] = pos + offset;
                }
            }

            // 三角形インデックスの生成
            int idx = 0;
            for (int i = 0; i < segmentCount; i++)
            {
                for (int j = 0; j < radialSegments; j++)
                {
                    int a = i * (radialSegments + 1) + j;
                    int b = a + radialSegments + 1;
                    int c = b + 1;
                    int d = a + 1;

                    triangles[idx++] = a;
                    triangles[idx++] = b;
                    triangles[idx++] = d;
                    triangles[idx++] = d;
                    triangles[idx++] = b;
                    triangles[idx++] = c;
                }
            }

            // メッシュ情報を更新
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.triangles = triangles.Reverse().ToArray();
            mesh.RecalculateNormals();
        }
    }
}
