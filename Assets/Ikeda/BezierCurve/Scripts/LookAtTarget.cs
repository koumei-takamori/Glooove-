/*******************************************************
 *  LookAtTarget.cs
 *  
 *  常に指定したターゲット方向を向くスクリプト
 *  作成者 : 池田桜輔
 *  更新日 : 2025/10/31
 *  
 *  概要 :
 *    Inspectorで指定したオブジェクトの方向を
 *    常に向くように制御します。
 *    Sceneビュー・ゲームプレイ両方で動作します。
 *******************************************************/

using UnityEngine;

[ExecuteAlways]
public class LookAtTarget : MonoBehaviour
{
    [Header("注視するターゲット")]
    [Tooltip("このオブジェクトの方向を常に向きます")]
    public Transform target;

    [Header("Y軸を固定するか")]
    [Tooltip("有効にすると水平方向だけで向きます")]
    public bool lockYRotation = false;

    private void Update()
    {
        // ターゲットが未指定の場合は何もしない
        if (target == null)
            return;

        // ターゲット方向のベクトルを計算
        Vector3 direction = target.position - transform.position;

        // Y軸を固定する場合（上下の傾きを無視）
        if (lockYRotation)
            direction.y = 0f;

        // ゼロベクトルを防止
        if (direction.sqrMagnitude < 0.0001f)
            return;

        // ターゲット方向を向く
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
