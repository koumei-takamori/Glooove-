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

    // 入力用クールタイム
    private float m_inputCooldown = 0.2f;
    private float m_inputTimer = 0f;

    public override void OnEnter()
    {
        currentSide = 0; // 最初は右
        Debug.Log("グローブ選択開始");
    }

    public override void OnUpdate()
    {
        // クールタイム減算
        m_inputTimer -= Time.deltaTime;

        // 連続入力を受け付けない
        if (m_inputTimer > 0f) return;

        // -------- 左右キー：操作対象切り替え --------
        float side = Owner.InputReceiver
            .GetInputValue<float>(SelectPlayerActions.GloveSideSelect);

        if (side > 0.8f && currentSide != GloveSide.Right)
        {
            currentSide = GloveSide.Right;
            Owner.CurrentGloveSide = currentSide;
            m_inputTimer = m_inputCooldown;
        }
        else if (side < -0.8f && currentSide != GloveSide.Left)
        {
            currentSide = GloveSide.Left;
            Owner.CurrentGloveSide = currentSide;
            m_inputTimer = m_inputCooldown;
        }


        // -------- 上下キー：グローブ変更 --------
        float glove = Owner.InputReceiver
           .GetInputValue<float>(SelectPlayerActions.GloveSelect);

        if (glove > 0.8f)
        {
            Owner.AddGloveIndex(currentSide, 1);
            m_inputTimer = m_inputCooldown;
        }
        else if (glove < -0.8f)
        {
            Owner.AddGloveIndex(currentSide, -1);
            m_inputTimer = m_inputCooldown;
        }

        // -------- 決定 --------
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Decide, InputType.PRESSED))
        {
            m_stateMashine.ChangeState(
                (int)SelectPlayer.SelectPlayerState.Ready
            );
        }

        // 戻る → グローブ選択
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Cancel, InputType.PRESSED))
        {
            m_stateMashine.ChangeState((int)SelectPlayer.SelectPlayerState.CharaSelect);
        }

    }

    public override void OnExit()
    {
        Debug.Log("左グローブ決定：" + Owner.GetGloveIndex(GloveSide.Left));
        Debug.Log("右グローブ決定：" + Owner.GetGloveIndex(GloveSide.Right));
    }
}
