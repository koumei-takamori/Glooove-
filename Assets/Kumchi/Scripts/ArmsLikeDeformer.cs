using UnityEngine;
using System.Collections.Generic;
using Ikeda.Bezier;

public class ArmsLikeDeformer : MonoBehaviour
{
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform start;
    [SerializeField] private Transform target;
    [SerializeField] private BezierCurveData curveData;
    [SerializeField] private float extendSpeed = 6f;
    [SerializeField] private float retractSpeed = 3f;
    [SerializeField] private float coilFrequency = 0f;
    [SerializeField] private float coilAmplitude = 0f;
    [SerializeField] private float hitWaitTime = 0.5f;

    // ★ゆっさゆっさ用パラメータ
    [SerializeField] private float swayAmplitude = 0.3f; // 揺れの大きさ
    [SerializeField] private float swaySpeed = 2.0f;     // 揺れの速さ

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

        // 発射前は上向きで初期化
        rootBone.rotation = Quaternion.LookRotation(Vector3.down);
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
        if (bones.Count < 2 || curveData == null) return;

        // ==== 発射入力 ====
        if (Input.GetKeyDown(KeyCode.Space) && !isExtending && !isRetracting)
        {
            isExtending = true;
            waitTimer = 0f;
            initialRotation = rootBone.rotation;

            Vector3 dir = (target.position - start.position).normalized;
            targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        // ==== 伸縮制御 ====
        if (isExtending)
        {
            t = Mathf.MoveTowards(t, 1f, Time.deltaTime * extendSpeed);
            rootBone.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

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
            rootBone.rotation = Quaternion.Slerp(targetRotation, initialRotation, 1f - t);
            if (t <= 0f) { t = 0f; isRetracting = false; }
        }
        // ルートボーンのYスケール補間
        float minScaleY = 0.01f;
        float maxScaleY = 1.0f;
        Vector3 localScale = rootBone.localScale;
        Debug.Log(t);
        localScale.y = Mathf.Lerp(minScaleY, maxScaleY, t); // t=0で最小, t=1で最大
        rootBone.localScale = localScale;
        // ==== ベジェ制御 ====
        Vector3 p0 = start.position;
        Vector3 p2 = target.position;

        Vector3 baseTangent0 = curveData.tangents[0];
        Vector3 rotatedTangent0 = start.rotation * baseTangent0;
        Vector3 t0 = Vector3.Lerp(baseTangent0, rotatedTangent0, 0.2f);
        Vector3 t1 = curveData.tangents[1];

        Vector3 p1_start = p0 + t0;
        Vector3 p1_end = p2 + t1;
        Vector3 p1 = Vector3.Lerp(p1_start, p1_end, 0.5f);

        Vector3 dirCurve = (p2 - p0).normalized;
        Vector3 right = Vector3.Cross(dirCurve, rootBone.up).normalized;


        // === rootBoneの方向をベジェ先頭方向に合わせる ===
        Vector3 forward0 = (GetBezier(p0, p1, p2, 0.01f) - p0).normalized;
        if (forward0.sqrMagnitude > 0.0001f)
        {
            Quaternion rot0 = Quaternion.LookRotation(forward0, rootBone.forward);
            rootBone.rotation = rot0 * Quaternion.Euler(90f, 0f, 0f);
        }

        // === ボーン配置 ===
        for (int i = 0; i < bones.Count; i++)
        {
            float u = (float)i / (bones.Count - 1);
            Vector3 pos = GetBezier(p0, p1, p2, u * t);
            // 付け根は揺らさない
            if (i != 0)
            {
                // 中央ボーンほど揺れる
                float centerFalloff = Mathf.Sin(u * Mathf.PI);
                // 伸びてる時だけ揺れるように t を掛ける
                float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude * centerFalloff * t;
                pos += Vector3.down * sway;
            }
            // 軽い波打ち（coil）
            float wave = Mathf.Sin(u * Mathf.PI * coilFrequency) * coilAmplitude * (1f - t);
            pos += right * wave;

            bones[i].position = pos;

            if (i > 0)
            {
                Vector3 forward = bones[i].position - bones[i - 1].position;
                if (forward.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(forward, rootBone.forward);
                    targetRot *= Quaternion.Euler(90f, 0f, 0f);
                    bones[i].rotation = targetRot;
                }
            }
        }
    }

    private Vector3 GetBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }
}
