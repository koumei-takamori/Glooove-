using UnityEngine;

/// <summary>
/// プレイヤーとターゲットが常に向かい合うようにする。
/// LookAtを使わずにSlerpで補間回転することで、ベジェの制御を壊さない。
/// </summary>
public class FaceEachOther : MonoBehaviour
{
    [Header("相手オブジェクト")]
    public Transform target;

    [Header("回転スピード（大きいほど即座に向く）")]
    public float rotateSpeed = 10f;

    void Update()
    {
        if (target == null) return;

        // 相手方向ベクトルを取得
        Vector3 dir = target.position - transform.position;

        // 完全に重なってたら無視（NaN対策）
        if (dir.sqrMagnitude < 0.0001f) return;

        // 回転したい方向
        Quaternion lookRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        // 現在の回転から目標回転に向けてスムーズに補間
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
    }
}
