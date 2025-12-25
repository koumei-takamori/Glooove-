/**********************************************
 * 
 *  CameraIdel.cs 
 *  カメラのダッシュ状態
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDashState : ICameraState
{
    //コントローラー
    private CameraContoller m_contoller;
    //遅延
    private Vector3 m_delay;

    


    public CameraDashState(CameraContoller contoller)
    {
        m_contoller = contoller;
    }

    public void Enter()
    {


        // Transposer コンポーネントを取得
        var transposer = m_contoller.GetCamera(CameraContoller.CameraType.NORMAL).GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            //Damping値の設定
            transposer.m_XDamping = m_delay.x;
            transposer.m_YDamping = m_delay.y;
            transposer.m_ZDamping = m_delay.z;
        }


    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
