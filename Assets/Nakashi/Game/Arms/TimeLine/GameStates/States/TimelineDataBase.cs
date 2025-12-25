//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <製作者>			NakashimaYuto
// <製作開始日>		2025/08/22
// <file>			TimelineDataBase.h
// <概要>		　　タイムライン用のデータベース
// <著作権>         Copyright (c) 2025 NakashimaYuto. All rights reserved.
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "CreateTimelineDataBase/TimelineData", fileName = "TimelineDataBase")]
public class TimelineDataBase : ScriptableObject
{
    [System.Serializable]
    public class TimelineEntry
    {
        public string m_key;
        public PlayableAsset m_timeline;
    }

    [SerializeField]
    public List<TimelineEntry> m_timelines = new List<TimelineEntry>();

    private Dictionary<string, PlayableAsset> m_dictionary;

    /// <summary>
    /// 辞書としてのタイムライン取得用
    /// </summary>
    public Dictionary<string, PlayableAsset> Dict
    {
        get
        {
            if (m_dictionary == null)
            {
                m_dictionary = new Dictionary<string, PlayableAsset>();
                foreach (var t in m_timelines)
                {
                    if (!m_dictionary.ContainsKey(t.m_key))
                    {
                        m_dictionary.Add(t.m_key, t.m_timeline);
                    }
                }
            }
            return m_dictionary;
        }
    }

    /// <summary>
    /// タイムラインの取得
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public PlayableAsset GetTimeline(string key)
    {
        Dict.TryGetValue(key, out var timeline);
        return timeline;
    }

}
