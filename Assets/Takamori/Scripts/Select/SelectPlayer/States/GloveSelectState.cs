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

    // 左右の決定状態
    private Dictionary<GloveSide, bool> m_isDecided
        = new Dictionary<GloveSide, bool>(){　
            { GloveSide.Left,  false },
            { GloveSide.Right, false }
        };

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
        // 最初は右を選択する
        currentSide = GloveSide.Left;

        m_isDecided[GloveSide.Left] = false;
        m_isDecided[GloveSide.Right] = false;

        Owner.UI.GloveCancel(GloveSide.Left);
        Owner.UI.GloveCancel(GloveSide.Right);
        Owner.UI.ChangeGloveSide(currentSide);
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

        // 選択するグローブの左右を変更
        // 左右の入力を受け取る
        float side = Owner.InputReceiver
            .GetInputValue<float>(SelectPlayerActions.GloveSideSelect);

        // 左のグローブを選択
        if (side > 0.8f && currentSide != GloveSide.Right)
        {
            currentSide = GloveSide.Right;
            Owner.UI.ChangeGloveSide(currentSide);
            m_inputTimer = m_inputCooldown;
            SoundManager.Instance.PlaySE("Slide");

        }
        // 左のグローブを選択
        else if (side < -0.8f && currentSide != GloveSide.Left)
        {
            currentSide = GloveSide.Left;
            Owner.UI.ChangeGloveSide(currentSide);
            m_inputTimer = m_inputCooldown;
            SoundManager.Instance.PlaySE("Slide");

        }

        // グローブの選択
        // 選択されていなければグローブの変更を可能にする
        if (!m_isDecided[currentSide])
        {
            float glove = Owner.InputReceiver
                .GetInputValue<float>(SelectPlayerActions.GloveSelect);

            // 下のグローブを選択
            if (glove > 0.8f)
            {
                Owner.ChangeGloveIndex(currentSide, 1);
                m_inputTimer = m_inputCooldown;
                SoundManager.Instance.PlaySE("Slide");
            }
            // 上のグローブを選択
            else if (glove < -0.8f)
            {
                Owner.ChangeGloveIndex(currentSide, -1);
                m_inputTimer = m_inputCooldown;
                SoundManager.Instance.PlaySE("Slide");
            }
        }


        // 決定
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Decide, InputType.PRESSED))
        {
            // グローブが決定していなければグローブの決定
            if (!m_isDecided[currentSide])
            {
                // グローブを決定する
                m_isDecided[currentSide] = true;
                Owner.UI.GloveDecide(currentSide);
                SoundManager.Instance.PlaySE("Decide");
                
                // 両方決定したら準備完了
                if (m_isDecided[GloveSide.Left] && m_isDecided[GloveSide.Right])
                {
                    m_stateMashine.ChangeState(
                        (int)SelectPlayer.SelectPlayerState.Ready
                    );
                }
            }
        }

        // キャンセル
        if (Owner.InputReceiver.GetInputButton(SelectPlayerActions.Cancel, InputType.PRESSED))
        {
            // グローブが決定していたらグローブをキャンセル
            if (m_isDecided[currentSide])
            {
                // 決定解除
                m_isDecided[currentSide] = false;
                Owner.UI.GloveCancel(currentSide);
                SoundManager.Instance.PlaySE("Cancel");
            }
            else
            {
                // 両方未決定ならキャラ選択へ戻る
                Owner.UI.CancelCharaIndex();
                m_stateMashine.ChangeState(
                    (int)SelectPlayer.SelectPlayerState.CharaSelect
                );
                SoundManager.Instance.PlaySE("Cancel");
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
        Owner.UI.ChangeGloveSide(GloveSide.None);
        Debug.Log("左グローブ決定：" + Owner.GetGloveIndex(GloveSide.Left));
        Debug.Log("右グローブ決定：" + Owner.GetGloveIndex(GloveSide.Right));
    }
}
