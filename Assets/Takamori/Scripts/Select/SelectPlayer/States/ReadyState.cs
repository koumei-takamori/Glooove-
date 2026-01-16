/**********************************************************
 *
 *  PlayerReadySelectState.cs
 *  準備完了状態
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectPlayerInputReceiver;
using static StateMachine<SelectPlayer>;

/// <summary>
/// 準備完了状態
/// </summary>
public class PlayerReadySelectState : StateBase
{
    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
        Owner.IsReady = true;
        Owner.UI.IsReady(true);
        Debug.Log("準備完了");
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        // グローブ選択
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Cancel, InputType.PRESSED))
        {
            Owner.IsReady = false;
            Owner.UI.IsReady(false);
            m_stateMashine.ChangeState((int)SelectPlayer.SelectPlayerState.GloveSelect);
        }
    }

    /*--------------------------------------------------------------------------------
　　|| ステートに出た時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに出た時の処理
    /// </summary>
    public override void OnExit()
    {
    }
}
