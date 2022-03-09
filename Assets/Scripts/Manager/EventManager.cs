﻿using System;  
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.Events;  

public class EventArg : EventArgs
{
    public string ArgName;
    public Event_CallBack CallBack;
}
public delegate void Event_CallBack(object _data = null);
public class EventManager
{
    //private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();  
    /// <summary>    
    /// 事件緩存    
    /// </summary>    
    private static List<EventArg> callbackList = new List<EventArg>();

    /// <summary>  
    /// 添加事件監聽  
    /// </summary>  
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void AddListener(string eventName, Event_CallBack listener)
    {
        EventArg eventarg = callbackList.Find(a => a.ArgName == eventName); ;
        if (eventarg == null)
        {
            eventarg = new EventArg();
            eventarg.ArgName = eventName;
            eventarg.CallBack = listener;
            callbackList.Add(eventarg);
        }
        else
        {
            eventarg.CallBack += listener;
        }
    }
    /// <summary>  
    /// 移除事件監聽  
    /// </summary>  
    /// <param name="eventName"></param>  
    /// <param name="listener"></param>  
    public static void RemoveListener(string eventName, Event_CallBack listener)
    {
        EventArg eventarg = callbackList.Find(a => a.ArgName == eventName); ;
        if (eventarg != null)
        {
            eventarg.CallBack -= listener;//移除監聽    
            if (eventarg.CallBack == null)//該類型是否還有回調,如果沒有,移除    
                callbackList.Remove(eventarg);
        }
    }
    /// <summary>  
    /// 觸發事件  
    /// </summary>  
    /// <param name="eventName"></param>  
    public static void TriggerEvent(string eventName, object sender = null)
    {
        // callbackList.ForEach((item) => {
        //     Debug.Log("callbackList " + item.ArgName);
        // });
        // Debug.Log("Trigger Event: " + eventName);
        EventArg eventarg = callbackList.Find(a => a.ArgName == eventName); ;
        if (eventarg == null)
        {
        }
        else
        {
            eventarg.CallBack(sender);//傳入參數,執行回調    
        }
    }
}
