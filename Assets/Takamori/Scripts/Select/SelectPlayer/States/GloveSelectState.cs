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
using static SelectPlayer;
using static SelectPlayerInputReceiver;
using static StateMachine<SelectPlayer>;

/// <summary>
/// キャラ選択状態
/// </summary>
public class GloveSelectState : StateBase
{
    // 現在操作しているグローブ
    private GloveSide currentSide = GloveSide.Left;

    public override void OnEnter()
    {
        currentSide = 0; // 最初は右
        Debug.Log("グローブ選択開始");
    }

    public override void OnUpdate()
    {
        // -------- 左右キー：操作対象切り替え --------
        if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.GloveSideSelect) > 0.8)
        {
            currentSide = GloveSide.Left;
            Owner.CurrentGloveSide = currentSide;
        }
        else if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.GloveSideSelect) < 0.8)
        {
            currentSide = GloveSide.Right;
            Owner.CurrentGloveSide = currentSide;
        }


        // -------- 上下キー：グローブ変更 --------
        if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.GloveSelect) > 0.8)
        {
            Owner.AddGloveIndex(currentSide, 1);
        }
        else if (Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.GloveSelect) < 0.8)
        {
            Owner.AddGloveIndex(currentSide, -1);
        }

        // -------- 決定 --------
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Decide, InputType.PRESSED))
        {
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.Ready
            );
        }
    }

    public override void OnExit()
    {
        Debug.Log("左グローブ決定：" + Owner.GetGloveIndex(GloveSide.Left));
        Debug.Log("右グローブ決定：" + Owner.GetGloveIndex(GloveSide.Right));
    }
}
