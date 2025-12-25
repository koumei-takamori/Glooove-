/**********************************************
 * 
 *  CameraJump.cs 
 *  カメラの攻撃状態
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttackState : ICameraState
{
    //コントローラー
    private CameraContoller m_contoller;


    public CameraAttackState(CameraContoller contoller)
    {
        m_contoller = contoller;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

}
