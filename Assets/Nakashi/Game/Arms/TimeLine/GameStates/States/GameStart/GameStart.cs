//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			GameStart.h
// <概要>		　　ゲームスタート
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using UnityEngine;

public class GameStart : IGameState
{
    // 所属コントローラー
    private GameController m_controller;

    public GameStart(GameController controller)
    {
        m_controller = controller;
    }

    /// <summary>
    /// 入出時
    /// </summary>
    public void Enter()
    {
        Debug.Log("Start中!");
        m_controller.PlayTimeline("Start");
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
            m_controller.GetStateMachine().ChangeState(m_controller.GetStateMachine().InGame());
        }
    }
}
