/**********************************************
 * 
 *  StateMashine.cs 
 *  ステートマシン (ジェネリック型)
 *  ステートマシンを量産できるように設計
 * 
 *  製作者：髙森煌明
 *  制作日：2024/04/18
 * 
 **********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ステートマシンクラス
/// </summary>
/// <typeparam name="TOwner">オーナーのクラス名</typeparam>
public class StateMachine<TOwner>
{
    /// <summary>
    /// ステートの基底クラス
    /// </summary>
    /// ここでしか使はないためStateMashine内で作成
    /// abstractは抽象クラス、抽象メソッドにつける修飾子
    public abstract class StateBase
    {
        // ステートマシン
        public StateMachine<TOwner> m_stateMashine;
        // オーナを取得
        protected TOwner Owner { get { return m_stateMashine.Owner; } }
        // ステートに入った時の処理
        public virtual void OnEnter() { }
        // 更新処理
        public virtual void OnUpdate() { }
        // ステートに出たときの処理
        public virtual void OnExit() { }
    }

    // オーナー
    private TOwner Owner { get; }
    // 現在のステート
    private StateBase m_currentState;
    // 前のステート
    private StateBase m_prevState;
    // 全てのステート定義
    private readonly Dictionary<int, StateBase> m_states = new Dictionary<int, StateBase>();

    public Dictionary<int, StateBase> State { get { return m_states; } }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="owner"></param>
    public StateMachine(TOwner owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// ステート登録
    /// ステートマシン初期化後にこのメソッドを呼ぶ
    /// </summary>
    /// <param name="stateID">ステートID</param>
    /// <typeparam name="T">ステート型</typeparam>
    /// where T : StateBase, new()はジェネリックにおける制約をつけている
    /// TがStateBaseを継承していて且つnew()制約によって、その型がパラメータなしのコンストラクタを持つこと
    public void Add<T>(int stateID) where T : StateBase, new()
    {
        // 全てのステート定義の中になければ返す
        if (m_states.ContainsKey(stateID))
        {
            Debug.LogError("このステートIDは定義されていません : " + stateID);
            return;
        }
        // ステート定義を登録しステートマシンに自分を入れる
        var newState = new T { m_stateMashine = this };
        // ステート追加する
        m_states.Add(stateID, newState);
    }

    /// <summary>
    /// ステート開始処理
    /// </summary>
    /// <param name="stateID">ステートID</param>
    public void OnStart(int stateID)
    {
        if (!m_states.TryGetValue(stateID, out var nextState))
        {
            Debug.LogError("このステートIDは定義されていません : " + stateID);
            return;
        }
        // 現在のステートに設定して処理を開始
        m_currentState = nextState;
        m_currentState.OnEnter();
    }

    /// <summary>
    /// ステート更新処理
    /// </summary>
    public void OnUpdate()
    {
        m_currentState.OnUpdate();
    }

    /// <summary>
    /// 次のステートに切り替える
    /// </summary>
    /// <param name="stateID">切り替えるステートID</param>
    public void ChangeState(int stateID)
    {
        if (!m_states.TryGetValue(stateID, out var nextState))
        {
            Debug.LogError("このステートIDは定義されていません : " + stateID);
            return;
        }
        // 前のステートを保持
        m_prevState = m_currentState;
        // ステートを切り替える
        m_currentState.OnExit();
        m_currentState = nextState;
        m_currentState.OnEnter();
    }

    /// <summary>
    /// 前回のステートに切り替える
    /// </summary>
    public void ChangePrevState()
    {
        if (m_prevState == null)
        {
            Debug.LogError("prevState is null!!");
            return;
        }
        // 前のステートと現在のステートを入れ替える
        (m_prevState, m_currentState) = (m_currentState, m_prevState);
    }
}


