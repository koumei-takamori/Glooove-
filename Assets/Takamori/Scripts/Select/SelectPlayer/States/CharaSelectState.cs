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
using static SelectPlayerInputReceiver;
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
        if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.CharaSelect) > 0.8)
        {
            Owner.AddCharaIndex(1);
        }

        if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.CharaSelect) < 0.8)
        {
            Owner.AddCharaIndex(-1);
        }

        // 決定 → グローブ選択へ
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Decide,InputType.PRESSED))
        {
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.GloveSelect
            );
        }

        Debug.Log("選択中キャラ：" + Owner.CharaIndex);
    }

    /*--------------------------------------------------------------------------------
　　|| ステートに出た時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに出た時の処理
    /// </summary>
    public override void OnExit()
    {
        Debug.Log(Owner.CharaIndex + "キャラ選択完了");
    }
}
