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
using static StateMachine<SelectPlayer>;

/// <summary>
/// キャラ選択状態
/// </summary>
public class GloveSelectState : StateBase
{
    // 現在操作しているグローブ
    private SelectPlayer.GloveSide currentSide = SelectPlayer.GloveSide.Left;

    public override void OnEnter()
    {
        currentSide = 0; // 最初は右
        Debug.Log("グローブ選択開始");
    }

    public override void OnUpdate()
    {
        // -------- 左右キー：操作対象切り替え --------
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSide = GloveSide.Left;
            Owner.CurrentGloveSide = currentSide;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSide = GloveSide.Right;
            Owner.CurrentGloveSide = currentSide;
        }


        // -------- 上下キー：グローブ変更 --------
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Owner.AddGloveIndex(currentSide, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Owner.AddGloveIndex(currentSide, -1);
        }

        // -------- 決定 --------
        if (Owner.IsDecide())
        {
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.Ready
            );
        }

        Debug.Log("操作中：" + (currentSide == 0 ? "右" : "左"));
        Debug.Log("左グローブ：" + Owner.GetGloveIndex(SelectPlayer.GloveSide.Left));
        Debug.Log("右グローブ：" + Owner.GetGloveIndex(SelectPlayer.GloveSide.Right));
    }

    public override void OnExit()
    {
        Debug.Log("左グローブ決定：" + Owner.GetGloveIndex(SelectPlayer.GloveSide.Left));
        Debug.Log("右グローブ決定：" + Owner.GetGloveIndex(SelectPlayer.GloveSide.Right));
    }
}
