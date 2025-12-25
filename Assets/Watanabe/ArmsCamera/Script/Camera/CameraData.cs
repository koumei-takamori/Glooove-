using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera/CreateCameraDate", fileName = "CameraData")]
public class CameraData : ScriptableObject
{

    [Header("パラメータ")]
    [SerializeField,Header("視点")]
    private Vector3 m_eye;


    [Header("移動時パラメーター設定")]
    [SerializeField, Header("遅延")]
    private Vector3 m_moveDelay;
    [SerializeField, Header("移動スピード")]
    private float m_moveMoveSpeed;

    [Header("ダッシュ時パラメーター設定")]
    [SerializeField, Header("遅延")]
    private Vector3 m_dashDelay;
    [SerializeField, Header("移動スピード")]
    private float m_dashMoveSpeed;

    [Header("ジャンプ時パラメーター設定")]
    [SerializeField, Header("遅延")]
    private Vector3 m_jumpDelay;



    //視点
    public Vector3 GetEye { get { return m_eye; }}
    //移動時の遅延
    public Vector3 GetMoveDelay { get { return m_moveDelay; }}
    //移動時の移動スピード
    public float GetMoveSpeed {  get { return m_moveMoveSpeed; }}



}
