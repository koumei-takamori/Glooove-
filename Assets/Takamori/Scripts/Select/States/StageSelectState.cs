/**********************************************************
 *
 *  StageSelectState.cs
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
public class StageSelectState : StateBase
{
    // 入力用クールタイム
    private float m_inputCooldown = 0.2f;
    private float m_inputTimer = 0f;

    /*--------------------------------------------------------------------------------
　　|| ステートに入った時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに入った時の処理
    /// </summary>
    public override void OnEnter()
    {
        Owner.StageSelectManager.IsActive(true);
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    public override void OnUpdate()
    {
        Debug.Log("ステージ選択中");

        // クールタイム減算
        m_inputTimer -= Time.deltaTime;

        // 連続入力を受け付けない
        if (m_inputTimer > 0f) return;

        for (int i = 0; i < 2; i++)
        {
            // 入力の値を取得
            float value = Owner.GetInput(i).GetInputValue<float>(SelectPlayerActions.CharaSelect);

            // 値に応じた処理
            if (value > 0.8f)
            {
                Owner.StageSelectManager.MoveStageSelect(1);
                m_inputTimer = m_inputCooldown;
                SoundManager.Instance.PlaySE("Slide");
            }
            else if (value < -0.8f)
            {
                Owner.StageSelectManager.MoveStageSelect(-1);
                m_inputTimer = m_inputCooldown;
                SoundManager.Instance.PlaySE("Slide");
            }

            if (Owner.GetInput(i).GetInputButton(SelectPlayerActions.Decide, InputType.PRESSED))
            {
                Owner.StageSelectManager.Decide();
                m_stateMashine.ChangeState(
                   (int)SelectSceneManager.SelectState.Ready
               );
                SoundManager.Instance.PlaySE("Decide");
            }

            if (Owner.GetInput(i).GetInputButton(SelectPlayerActions.Cancel, InputType.PRESSED))
            {
                Owner.StageSelectManager.IsActive(false);
                m_stateMashine.ChangeState(
                   (int)SelectSceneManager.SelectState.PlayerSelect
               );
                SoundManager.Instance.PlaySE("Cancel");

                // プレイヤーセレクトのUIの操作を許可する
                var players = SelectPlayerManager.Instance.Players;
                foreach (var player in players)
                {
                    player.CanControll = true;
                }
            }

        }
    }

    /*--------------------------------------------------------------------------------
　　|| ステートに出た時の処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// ステートに出た時の処理
    /// </summary>s
    public override void OnExit()
    {
    }
}
