/**********************************************
 * 
 *  CameraContoller.cs 
 *  カメラの管理クラス
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CameraContoller : MonoBehaviour
{
    [SerializeField, Header("1Pのトランスフォーム")]
    private Transform m_1PTransform;
    [SerializeField, Header("2Pのトランスフォーム")]
    private Transform m_2PTransform;


    //1P用カメラオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private CinemachineCamera m_1PCamera;

    //２P用カメラオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private CinemachineCamera m_2PCamera;

    //ターゲットグループオブジェクト
    [SerializeField, Header("子オブジェクト")]
    private GameObject m_targetGroupOb;

    //ターゲットグループスクリプト
    private CinemachineTargetGroup m_targetGroup;

    private void Awake()
    {

        //オブジェクトからスクリプトを取得
        m_targetGroup = m_targetGroupOb.GetComponent<CinemachineTargetGroup>();
    }



    // Start is called before the first frame update
    void Start()
    {

        //ターゲットグループの設定
        TargetGroupSetting();

        //１Pカメラの設定
        m_1PCamera.Follow = m_1PTransform;
        m_1PCamera.LookAt = m_targetGroupOb.transform;

        //２Pカメラの設定
        m_2PCamera.Follow = m_2PTransform;
        m_2PCamera.LookAt = m_targetGroupOb.transform;

    }

    // Update is called once per frame
    void Update()
    {


    }

    /// <summary>
    /// ターゲットグループの設定
    /// </summary>
    private void TargetGroupSetting()
    {

        m_targetGroup.Targets.Add(
            new CinemachineTargetGroup.Target
            {
                Object = m_1PTransform,
                Weight = 1.0f,
                Radius = 0.5f
            }
        );

        m_targetGroup.Targets.Add(
            new CinemachineTargetGroup.Target
            {
                Object = m_2PTransform,
                Weight = 1.0f,
                Radius = 0.5f
            }
);

    }

    public void InitCameraTargets()
    {

    }

    //所有者の取得
    public Transform Owner { get { return m_1PTransform; } set { m_1PTransform = value; } }
    //ターゲットの取得
    public Transform Target { get { return m_2PTransform; } set { m_2PTransform = value; } }

}
