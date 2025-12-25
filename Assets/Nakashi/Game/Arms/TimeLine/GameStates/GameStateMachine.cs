//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			GameStateMachine.h
// <概要>		　　ゲームステートマシーン
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    // 現在の状態
    IGameState m_currentState;

    // 現在のステートの状態取得
    public IGameState GetCurrentState() => m_currentState;

    // 状態一覧
    private GameStart m_gameStart;
    private InGame m_inGame;
    private KO m_ko;

    public GameStart GameStart() => m_gameStart;
    public InGame InGame() => m_inGame;
    public KO KOState() => m_ko;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="controller">所属コントローラー</param>
    public GameStateMachine(GameController controller)
    {
        m_gameStart = new GameStart(controller);
        m_inGame = new InGame(controller);
        m_ko = new KO(controller);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="state"></param>
    public void Initialize(IGameState state)
    {
        m_currentState = state;
        m_currentState.Enter();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void Update()
    {
        m_currentState.Update();
    }

    /// <summary>
    /// ステートの変更
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(IGameState state)
    {
        m_currentState.Exit();
        m_currentState = state;
        m_currentState.Enter();
    }
}
