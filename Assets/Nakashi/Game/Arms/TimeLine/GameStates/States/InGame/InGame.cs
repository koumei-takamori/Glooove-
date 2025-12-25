//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			InGame.h
// <概要>		　　ゲーム中
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

public class InGame : IGameState
{
    // 所属コントローラー
    private GameController m_controller;

    public InGame(GameController controller)
    {
        m_controller = controller;
    }

    /// <summary>
    /// 入出時
    /// </summary>
    public void Enter()
    {
        Debug.Log("InGame中!");
        m_controller.PlayTimeline("InGame");
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

        if(m_controller.FinishTimeline())
        {
            m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().KOState());
        }
    }
}
