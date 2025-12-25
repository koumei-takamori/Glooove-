//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/26
// <file>			KO.h
// <概要>		　　KO
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

public class KO : IGameState
{
    // 所属コントローラー
    private GameController m_controller;

    public KO(GameController controller)
    {
        m_controller = controller;
    }

    /// <summary>
    /// 入出時
    /// </summary>
    public void Enter()
    {
        Debug.Log("KO中!");
        m_controller.PlayTimeline("KO");
    }
    /// <summary>
    /// 退出時
    /// </summary>
    public void Exit()
    {

    }
    /// <summary>
    /// 更新時
    /// </summary>
    public void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().GameStart());
        }
    }
}
