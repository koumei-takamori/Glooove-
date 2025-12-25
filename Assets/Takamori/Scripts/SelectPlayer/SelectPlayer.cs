/**********************************************************
 *
 *  SelectPlayer.cs
 *  セレクトシーンのプレイヤー
 *
 *  制作者 : 髙森 煌明
 *  制作日 : 2025/12/21
 *
 *********************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// セレクトシーンのプレイヤー
/// </summary>
public class SelectPlayer : MonoBehaviour
{
    /// <summary>
    /// セレクトシーンのプレイヤーステート
    /// </summary>
    public enum SelectPlayerState
    {
        CharaSelect = 0,      // キャラ選択状態
        GloveSelect = 1,      // グローブ選択状態
        Ready       = 2 　　  // 準備確認状態
    }

    // ステートマシン
    private StateMachine<SelectPlayer> m_stateMachine;

    // プレイヤーID
    [SerializeField]
    private int m_playerID;
    // private NpadId m_playerID;

    /*--------------------------------------------------------------------------------
　　|| 実行前初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Awake()
    {

    }

    /*--------------------------------------------------------------------------------
　　|| 初期化処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 実行前初期化処理
    /// </summary>
    private void Start()
    {
        // ステートマシン定義
        m_stateMachine = new StateMachine<SelectPlayer>(this);
        // 各ステート追加
        m_stateMachine.Add<CharaSelectState>((int)SelectPlayerState.CharaSelect);
        m_stateMachine.Add<GloveSelectState>((int)SelectPlayerState.GloveSelect);
        m_stateMachine.Add<ReadySelectState>((int)SelectPlayerState.Ready);

        // ステートマシン開始
        m_stateMachine.OnStart((int)SelectPlayerState.CharaSelect);
    }

    /*--------------------------------------------------------------------------------
　　|| 更新処理
　　--------------------------------------------------------------------------------*/
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // ステート更新
        m_stateMachine.OnUpdate();
    }
}
