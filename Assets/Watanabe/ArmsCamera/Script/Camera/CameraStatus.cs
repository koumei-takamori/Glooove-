using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraStatus
{

    //初期座標
    private Vector3 m_eye;

    //移動時
    //遅延
    private Vector3 m_moveDelay;
    //移動スピード
    private float m_moveSpeed;

    //ダッシュ時
    private Vector3 m_dashDelay;

    //ジャンプ時
    private Vector3 m_jumpDelay;

    //攻撃時
    private Vector3 m_attackDelay;




    public CameraStatus(CameraContoller contoller)
    {
       
        //ステータスをデータから取得
        m_eye = contoller.GetData.GetEye;

        m_moveDelay = contoller.GetData.GetMoveDelay;

        m_moveSpeed = contoller.GetData.GetMoveSpeed;

    }

    //視点の取得
    public Vector3 GetEye { get { return m_eye; } }
    //
    public Vector3 GetMoveDelay { get { return m_moveDelay; } }
    //
    public float GetMoveSpeed {  get { return m_moveSpeed; } }

}
