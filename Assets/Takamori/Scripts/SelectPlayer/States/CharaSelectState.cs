/**********************************************************
 *
 *  CharaSelectState.cs
 *  キャラの選択状態
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
public class CharaSelectState : StateBase
{
    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
        Debug.Log("キャラ選択開始");
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        // 横入力でキャラ変更
        int dir = Owner.GetHorizontalOnce();
        if (dir != 0)
        {
            Owner.AddCharaIndex(dir);
        }

        // 決定 → グローブ選択へ
        if (Owner.IsDecide())
        {
            // グローブ選択ステートへ遷移
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.GloveSelect
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
        Debug.Log(Owner.GetCharaIndex() + "キャラ選択完了");
    }
}
