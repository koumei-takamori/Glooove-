using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
/// <summary>
/// ボーン付きモデルのボーンを回転させるテスト用スクリプト
/// </summary>
public class RollBone : MonoBehaviour
{
    // 一番上のボーン
    [Header("一番上のボーン")]
    public Transform topBone;

    [Header("Bezier")]
    [SerializeField] private BezierCurveData curveData;
    private List<Transform> bones = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        bones.Clear();
        if (topBone != null) GetAllBones(topBone);
    }
    private void GetAllBones(Transform current)
    {
        bones.Add(current);
        for (int i = 0; i < current.childCount; i++)
        {
            Debug.Log($"Initial Bone: {current.name}, Position: {current.position}");

            GetAllBones(current.GetChild(i));
        }
    }
    // Update is called once per frame
    void Update()
    {
        Transform b1 = bones[1];
        Transform b2 = bones[2];

        // ボーン方向（world）
        Vector3 axisWorld = (b2.position - b1.position).normalized;

        // ローカル軸に変換する
        Vector3 axisLocal = b1.InverseTransformDirection(axisWorld).normalized;

        // 回転角度
        float angle = 45.0f * Time.deltaTime;

        // 任意回転
        Quaternion addRot = Quaternion.AngleAxis(angle, axisLocal);

        // ローカル空間の軸とローカル回転を正しく合成
        b1.localRotation = addRot * b1.localRotation;
    }


}
