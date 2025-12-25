/**********************************************
 * 
 *  CameraMoveState.cs 
 *  カメラの通常状態
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMoveState : ICameraState
{
    //コントローラー
    private CameraContoller m_contoller;
    //遅延
    private Vector3 m_delay;
    //移動スピード
    private float m_speed;


    public CameraMoveState(CameraContoller contoller)
    {
        //コントローラーの取得
        m_contoller = contoller;
        
        //遅延        
        m_delay = m_contoller.GetStatus.GetMoveDelay;
        //移動スピード
        m_speed = m_contoller.GetStatus.GetMoveSpeed;

        

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

            Debug.Log(m_delay);

        }

    }

    public void Exit()
    {
       
    }

    public void Update()
    {

    }

}
