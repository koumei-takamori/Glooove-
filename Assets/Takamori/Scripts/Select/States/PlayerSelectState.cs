/**********************************************************
 *
 *  PlayerSelectState.cs
 *  キャラの選択状態
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using UnityEngine;
using static SelectPlayerInputReceiver;
using static StateMachine<SelectSceneManager>;

/// <summary>
/// キャラ選択状態
/// </summary>
public class PlayerSelectState : StateBase
{
    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        if (IsAllPlayerReady())
        {
            m_stateMashine.ChangeState(
               (int)SelectSceneManager.SelectState.StageSelect
           );
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

    /*--------------------------------------------------------------------------------
    || 全プレイヤーが準備完了か
    --------------------------------------------------------------------------------*/
    public bool IsAllPlayerReady()
    {
        var players = SelectPlayerManager.Instance.Players;

        if (players.Count < 2 || players == null) return false;

        foreach (var player in players)
        {
            if (!player.IsReady)
            {
                return false;
            }
        }
        return true;
    }
}
