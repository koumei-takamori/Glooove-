/**********************************************
 * 
 *  CameraIdelState.cs 
 *  カメラの通常状態
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraIdelState : ICameraState
{
    //コントローラー
    private CameraContoller m_contoller;
    //所有者
    private Transform m_owner;

    public CameraIdelState(CameraContoller contoller)
    {
        m_contoller = contoller;
        m_owner = m_contoller.GetOwner;
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
