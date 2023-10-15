using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    None, Minute, hour, Day, Dialog, NextDialog, GuestEntry, GuestExit, StartConversation, EndConversation, Work, SalesSuccess, SalesFailure, Enter, Exit, EndIteraction, CloseShop, Save, Load, End
}

public enum DialogEvent
{

}

public class EventManager
{
    static readonly Dictionary<EventType, UnityEvent> events = new Dictionary<EventType, UnityEvent>();

    public static void Subscribe(EventType eventType, UnityAction listner)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e.AddListener(listner);
        else
        {
            e = new();
            e.AddListener(listner);
            events.Add(eventType, e);
        }
    }

    public static void Unsubscribe(EventType eventType, UnityAction listner)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e.RemoveListener(listner);
    }

    public static void Publish(EventType eventType)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e?.Invoke();
    }
}
