/**********************************************************
 *
 *  GloveSelectState.cs
 *  グローブの選択状態
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateMachine<SelectPlayer>;

/// <summary>
/// キャラ選択状態
/// </summary>
public class GloveSelectState : StateBase
{
    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
        Debug.Log("グローブ選択開始");
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        int dir = Owner.GetVerticalOnce();
        if (dir != 0)
        {
            Owner.AddGloveIndex(dir);
        }

        // 決定 → Readyへ
        if (Owner.IsDecide())
        {
            // ステートへ遷移
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.Ready
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
        Debug.Log(Owner.GetGloveIndex() + "グローブ選択完了");
    }
}
