/**********************************************************
 *
 *  CharaSelectState.cs
 *  キャラの選択状態
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using UnityEngine;
using static SelectPlayerInputReceiver;
using static StateMachine<SelectPlayer>;

/// <summary>
/// キャラ選択状態
/// </summary>
public class CharaSelectState : StateBase
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
        // クールタイム減算
        m_inputTimer -= Time.deltaTime;

        // 連続入力を受け付けない
        if (m_inputTimer > 0f) return;

        // 入力の値を取得
        float value = Owner.InputReceiver.GetInputValue<float>(SelectPlayerActions.CharaSelect);

        // 値に応じた処理
        if (value > 0.8f)
        {
            Owner.ChangeCharaIndex(1);
            m_inputTimer = m_inputCooldown;
        }
        else if (value < -0.8f)
        {
            Owner.ChangeCharaIndex(-1);
            m_inputTimer = m_inputCooldown;
        }

        // 決定 → グローブ選択へ
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Decide,InputType.PRESSED))
        {
            Owner.UI.DecideCharaIndex();
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
        Debug.Log(Owner.CharaIndex + "キャラ選択完了");
    }
}
