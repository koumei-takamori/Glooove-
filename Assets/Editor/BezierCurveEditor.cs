// -------------------------------------------------------------
// BezierCurveEditor.cs
// ベジェ曲線の制御点と接線をシーンビューで操作・描画するエディタ拡張（XZ平面限定）
// インスペクターを見やすく整理
// 制作者 : 池田桜輔
// 改良日 : 2025/09/03
// -------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Ikeda.Bezier
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveEditor : Editor
    {
        // カーブ設定の折りたたみフラグ
        private bool showCurveSettings = true;

        // 接線設定の折りたたみフラグ
        private bool showTangentSettings = true;

        // 制御点操作の折りたたみフラグ
        private bool showPointsSettings = true;

        // SceneView の描画処理
        private void OnSceneGUI()
        {
            // ターゲットの BezierCurve を取得
            BezierCurve curve = (BezierCurve)target;

            // curveData がなければ処理を行わない
            if (curve.curveData == null) return;

            // すべての制御点を順に処理
            for (int i = 0; i < curve.curveData.points.Length; i++)
            {
                // 現在の制御点の座標を取得
                Vector3 currentPos = curve.curveData.points[i];

                // 制御点移動開始チェック
                EditorGUI.BeginChangeCheck();

                // 制御点ハンドルの色を黄色に設定
                Handles.color = Color.yellow;

                // 制御点をXZ平面で自由に移動（新形式）
                Vector3 newPos = Handles.FreeMoveHandle(
                    currentPos,            // 現在位置
                    0.1f,                  // ハンドルサイズ
                    Vector3.zero,          // スナップなし
                    Handles.SphereHandleCap// ハンドル形状
                );
                newPos.y = currentPos.y;  // Y座標は固定

                // 位置が変わったら更新
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(curve.curveData, "Move Point");
                    curve.curveData.points[i] = newPos;
                    EditorUtility.SetDirty(curve.curveData);
                }

                // 接線が表示されている場合
                if (curve.showTangents)
                {
                    // 接線ターゲット座標を取得
                    Vector3 tangentTarget = curve.curveData.points[i] + curve.curveData.tangents[i];

                    // 接線移動開始チェック
                    EditorGUI.BeginChangeCheck();

                    // ハンドル色を緑に設定
                    Handles.color = Color.green;

                    // 接線をXZ平面で自由に移動（新形式）
                    Vector3 newTangentPos = Handles.FreeMoveHandle(
                        tangentTarget,        // 現在位置
                        0.07f,                // ハンドルサイズ
                        Vector3.zero,         // スナップなし
                        Handles.CubeHandleCap // ハンドル形状
                    );
                    newTangentPos.y = tangentTarget.y; // Y座標固定

                    // 接線が変わったら更新
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(curve.curveData, "Move Tangent");
                        curve.curveData.tangents[i] = new Vector3(
                            newTangentPos.x - curve.curveData.points[i].x,
                            curve.curveData.tangents[i].y,
                            newTangentPos.z - curve.curveData.points[i].z
                        );
                        EditorUtility.SetDirty(curve.curveData);
                    }

                    // 接線をラインで描画
                    Handles.DrawLine(curve.curveData.points[i], curve.curveData.points[i] + curve.curveData.tangents[i]);
                }

                // 制御点番号をラベルで表示
                Handles.Label(curve.curveData.points[i], $"P{i}");
            }

            // 曲線描画色を設定
            Handles.color = Color.cyan;

            // 全制御点間のベジェ曲線を描画
            for (int i = 0; i < curve.curveData.points.Length - 1; i++)
            {
                Handles.DrawBezier(
                    curve.curveData.points[i],
                    curve.curveData.points[i + 1],
                    curve.curveData.points[i] + curve.curveData.tangents[i],
                    curve.curveData.points[i + 1] + curve.curveData.tangents[i + 1],
                    Color.cyan,
                    null,
                    curve.lineWidth
                );
            }

            // 曲線を閉じる場合の描画
            if (curve.closed && curve.curveData.points.Length > 2)
            {
                Handles.DrawBezier(
                    curve.curveData.points[curve.curveData.points.Length - 1],
                    curve.curveData.points[0],
                    curve.curveData.points[curve.curveData.points.Length - 1] + curve.curveData.tangents[curve.curveData.tangents.Length - 1],
                    curve.curveData.points[0] + curve.curveData.tangents[0],
                    Color.cyan,
                    null,
                    curve.lineWidth
                );
            }
        }

        // インスペクター描画
        public override void OnInspectorGUI()
        {
            BezierCurve curve = (BezierCurve)target;

            // CurveData を指定
            curve.curveData = (BezierCurveData)EditorGUILayout.ObjectField(
                "Curve Data", curve.curveData, typeof(BezierCurveData), false);

            // CurveData 未設定なら警告
            if (curve.curveData == null)
            {
                EditorGUILayout.HelpBox("BezierCurveData を割り当ててください", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();

            // カーブ設定
            showCurveSettings = EditorGUILayout.Foldout(showCurveSettings, "カーブ設定");
            if (showCurveSettings)
            {
                // 曲線太さ
                curve.lineWidth = EditorGUILayout.Slider("Curve Width", curve.lineWidth, 0.1f, 3.0f);

                // 曲線を閉じるか
                curve.closed = EditorGUILayout.Toggle("Closed", curve.closed);
            }

            EditorGUILayout.Space();

            // 接線設定
            showTangentSettings = EditorGUILayout.Foldout(showTangentSettings, "制御点の接線設定");
            if (showTangentSettings)
            {
                // 接線表示ON/OFF
                curve.showTangents = EditorGUILayout.Toggle("Show Tangents", curve.showTangents);

                // 接線太さ
                curve.tangentLineWidth = EditorGUILayout.Slider("Tangent Width", curve.tangentLineWidth, 0.1f, 3.0f);
            }

            EditorGUILayout.Space();

            // 制御点操作
            showPointsSettings = EditorGUILayout.Foldout(showPointsSettings, "Points");
            if (showPointsSettings)
            {
                // 制御点数を表示
                EditorGUILayout.LabelField("Point Count: " + curve.curveData.points.Length);

                // 制御点追加ボタン
                if (GUILayout.Button("Add Point"))
                {
                    Undo.RecordObject(curve.curveData, "Add Point");
                    var newPoints = new Vector3[curve.curveData.points.Length + 1];
                    var newTangents = new Vector3[curve.curveData.tangents.Length + 1];
                    curve.curveData.points.CopyTo(newPoints, 0);
                    curve.curveData.tangents.CopyTo(newTangents, 0);
                    newPoints[newPoints.Length - 1] = curve.curveData.points[curve.curveData.points.Length - 1] + Vector3.forward;
                    newTangents[newTangents.Length - 1] = Vector3.forward * 0.5f;
                    curve.curveData.points = newPoints;
                    curve.curveData.tangents = newTangents;
                    EditorUtility.SetDirty(curve.curveData);
                }

                // 制御点削除ボタン（最低2点は残す）
                if (curve.curveData.points.Length > 2 && GUILayout.Button("Remove Last Point"))
                {
                    Undo.RecordObject(curve.curveData, "Remove Point");
                    var newPoints = new Vector3[curve.curveData.points.Length - 1];
                    var newTangents = new Vector3[curve.curveData.tangents.Length - 1];
                    for (int i = 0; i < newPoints.Length; i++)
                    {
                        newPoints[i] = curve.curveData.points[i];
                        newTangents[i] = curve.curveData.tangents[i];
                    }
                    curve.curveData.points = newPoints;
                    curve.curveData.tangents = newTangents;
                    EditorUtility.SetDirty(curve.curveData);
                }
            }

            // GUI変更があれば Dirty フラグ
            if (GUI.changed)
            {
                EditorUtility.SetDirty(curve);
            }
        }
    }
}