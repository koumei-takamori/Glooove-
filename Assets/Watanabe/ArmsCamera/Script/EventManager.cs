/**********************************************
 * 
 *  EventManager.cs 
 *  イベントマネージャー
 * 
 *  製作者：渡邊　翔也
 *  制作日：2025/08/25
 * 
 **********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class PlayerEvent 
{
    //引数
    public string Message;
    public PlayerEvent(string message) => Message = message;
}



public class EventManager 
{
    static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            instance ??= new EventManager();
            return instance;
        }
    }



    /// <summary>
    /// コンストラク
    /// </summary>
    private EventManager() { }


    // イベントを種類ごとに保持する Dictionary
    private Dictionary<Type, Delegate> eventTable = new Dictionary<Type, Delegate>();

    /// <summary>
    /// イベント登録
    /// </summary>
    public void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], listener);
        }
        else
        {
            eventTable[eventType] = listener;
        }
    }

    /// <summary>
    /// イベント解除
    /// </summary>
    public void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            var currentDel = Delegate.Remove(eventTable[eventType], listener);
            if (currentDel == null)
                eventTable.Remove(eventType);
            else
                eventTable[eventType] = currentDel;
        }
    }

    /// <summary>
    /// イベント発行
    /// </summary>
    public void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            var callback = eventTable[eventType] as Action<T>;
            callback?.Invoke(eventData);
        }
    }

}


////Scriptableの場合 
//public abstract class EventBase : ScriptableObject { }
//
//以下を通知の種類分だけ作成
//
//[CreateAssetMenu(menuName = "Events/PlayerEvent")]
//public class PlayerEvent : EventBase
//{
//    // イベントリスナー（Actionを保持）
//    private System.Action<string> listeners;

//    /// <summary>
//    /// 登録
//    /// </summary>
//    public void Subscribe(System.Action<string> listener)
//    {
//        listeners += listener;
//    }

//    /// <summary>
//    /// 解除
//    /// </summary>
//    public void Unsubscribe(System.Action<string> listener)
//    {
//        listeners -= listener;
//    }

//    /// <summary>
//    /// 発行
//    /// </summary>
//    public void Publish(string message)
//    {
//        listeners?.Invoke(message);
//    }
//}