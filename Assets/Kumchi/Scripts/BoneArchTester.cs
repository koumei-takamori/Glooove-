using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 直線に伸び縮みするアーム（最初のボーンの逆向き補正付き）
/// </summary>
public class StraightArmDeformer : MonoBehaviour
{
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform start;
    [SerializeField] private Transform target;
    [SerializeField] private float extendSpeed = 6f;
    [SerializeField] private float retractSpeed = 3f;
    [SerializeField] private float hitWaitTime = 0.5f;
    [SerializeField] private Vector3 boneForwardAxis = Vector3.up; // モデルのボーンがどの軸方向に伸びているか

    private List<Transform> bones = new List<Transform>();
    private float t = 0f;
    private float waitTimer = 0f;
    private bool isExtending = false;
    private bool isRetracting = false;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        bones.Clear();
        GetAllBones(rootBone);
        initialRotation = rootBone.rotation;
    }

    private void GetAllBones(Transform current)
    {
        bones.Add(current);
        for (int i = 0; i < current.childCount; i++)
            GetAllBones(current.GetChild(i));
    }

    void Update()
    {
        if (bones.Count < 2) return;

        // ====== 入力処理 ======
        if (Input.GetKeyDown(KeyCode.Space) && !isExtending && !isRetracting)
        {
            isExtending = true;
            waitTimer = 0f;
            initialRotation = rootBone.rotation;
            Vector3 dir = (target.position - start.position).normalized;
            targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        // ====== アニメーション制御 ======
        if (isExtending)
        {
            t = Mathf.MoveTowards(t, 1f, Time.deltaTime * extendSpeed);
            if (Mathf.Abs(t - 1f) < 0.01f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= hitWaitTime)
                {
                    isExtending = false;
                    isRetracting = true;
                }
            }
        }
        else if (isRetracting)
        {
            t = Mathf.MoveTowards(t, 0f, Time.deltaTime * retractSpeed);
            if (t <= 0f) isRetracting = false;
        }

        // ====== 位置更新 ======
        Vector3 startPos = start.position;
        Vector3 endPos = Vector3.Lerp(start.position, target.position, t);

        for (int i = 0; i < bones.Count; i++)
        {
            float u = (float)i / (bones.Count - 1);
            Vector3 pos = Vector3.Lerp(startPos, endPos, u);
            bones[i].position = pos;
        }

        // ====== 回転更新 ======
        for (int i = 0; i < bones.Count; i++)
        {
            if (i == 0)
            {
                // 最初のボーンは「次のボーン」を向かせる
                Vector3 dir = (bones[i + 1].position - bones[i].position).normalized;
                Quaternion look = Quaternion.FromToRotation(boneForwardAxis, dir);
                bones[i].rotation = look;
            }
            else if (i < bones.Count - 1)
            {
                // 中間ボーンは前後の中間方向を向かせる
                Vector3 dirPrev = (bones[i].position - bones[i - 1].position).normalized;
                Vector3 dirNext = (bones[i + 1].position - bones[i].position).normalized;
                Vector3 dirAvg = ((dirPrev + dirNext) * 0.5f).normalized;
                Quaternion look = Quaternion.FromToRotation(boneForwardAxis, dirAvg);
                bones[i].rotation = look;
            }
            else
            {
                // 最後のボーンは「前のボーンの方向」を向く
                Vector3 dir = (bones[i].position - bones[i - 1].position).normalized;
                Quaternion look = Quaternion.FromToRotation(boneForwardAxis, dir);
                bones[i].rotation = look;
            }
        }
    }
}
