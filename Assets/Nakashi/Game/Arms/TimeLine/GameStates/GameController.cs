using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    // ステートマシーン
    private GameStateMachine m_stateMachine;

    // 所属するディレクター
    public PlayableDirector m_director;
    // タイムラインのデータベース
    public TimelineDataBase m_timelineDB;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        m_stateMachine = new GameStateMachine(this);
        m_stateMachine.Initialize(m_stateMachine.GameStart());
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        m_stateMachine.Update();
    }

    /// <summary>
    /// ステートマシンの取得
    /// </summary>
    /// <returns></returns>
    public GameStateMachine GetStateMachine() => m_stateMachine;

    /// <summary>
    /// タイムラインの開始
    /// </summary>
    /// <param name="key"></param>
    public void PlayTimeline(string key)
    {
        var timeline = m_timelineDB.GetTimeline(key);
        if(timeline != null)
        {
            m_director.playableAsset = timeline;
            m_director.Play();
        }
        else
        {
            Debug.LogWarning($"Timeline'{key}'が見つからない");
        }
    }

    /// <summary>
    /// タイムライン終了処理
    /// </summary>
    /// <returns></returns>
    public bool FinishTimeline()
    {
        return m_director.state != PlayState.Playing;
    }
}
