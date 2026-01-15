/**********************************************************
 *
 *  ReadySelectState.cs
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
public class ReadySelectState : StateBase
{
    private bool m_isLoad; 

    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
        m_isLoad = false;
        Owner.ReadyUI.Animator.SetBool("GameReady", true);
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        for (int i = 0; i < 2; i++)
        {
            if (m_isLoad) return;

            if (Owner.GetInput(i).GetInputButton(SelectPlayerActions.Decide, InputType.PRESSED))
            {
                m_isLoad = true;
                Owner.StartCoroutine(Owner.EnterToPlayScene(1.0f));
            }

            if (Owner.GetInput(i).GetInputButton(SelectPlayerActions.Cancel, InputType.PRESSED))
            {
                Owner.ReadyUI.Animator.SetBool("GameReady", false);
                m_stateMashine.ChangeState(
                   (int)SelectSceneManager.SelectState.StageSelect
               );
            }
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
