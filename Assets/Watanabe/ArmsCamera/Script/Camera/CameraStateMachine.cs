/**********************************************
 * 
 *  CameraStateMachine.cs 
 *  カメラのステートマシーン
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/07/31
 * 
 **********************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine
{
    //実行中ステート
    ICameraState m_currentState;

    //各状態
    private CameraIdelState     m_idel;    //通常状態
    private CameraJumpState     m_jump;    //ジャンプ状態
    private CameraMoveState     m_move;    //移動状態
    private CameraDashState     m_dash;    //ダッシュ状態
    private CameraAttackState   m_attack;  //攻撃状態


    public CameraStateMachine(CameraContoller contoller)
    {
        //状態の生成
        m_idel      = new CameraIdelState(contoller);
        m_jump      = new CameraJumpState(contoller);
        m_move      = new CameraMoveState(contoller);
        m_dash      = new CameraDashState(contoller);
        m_attack    = new CameraAttackState(contoller);

    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="state">初期状態</param>
    public void Initialize(ICameraState state)
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
    /// 状態の切り替え
    /// </summary>
    /// <param name="state">変更したい状態</param>
    public void ChangeState(ICameraState state)
    {
        //同じなら
        if(m_currentState == state) { return; }


        m_currentState.Exit();
        m_currentState = state;
        m_currentState.Enter();
    }

    //実行カメラ
    public ICameraState GetState { get { return m_currentState; } }
    //通常状態
    public CameraIdelState GetIdelState { get { return m_idel; } }
    //ジャンプ状態
    public CameraJumpState GetJumpState { get { return m_jump; } }
    //移動状態
    public CameraMoveState GetMoveState { get { return m_move;} }
    //ダッシュ状態
    public CameraDashState GetDashState { get { return m_dash;} }
    //攻撃状態
    public CameraAttackState GetAttackState { get { return m_attack; } }
}
