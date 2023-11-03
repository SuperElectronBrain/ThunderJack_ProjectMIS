using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    None, Minute, hour, Day, WorkTime, Dialog, NextDialog, GuestEntry, GuestExit, StartConversation, EndConversation, Work, SalesSuccess, SalesFailure, Enter, Exit, StartInteraction, EndIteraction, CloseShop, Save, Load, End
}

public class EventManager
{
    static readonly Dictionary<EventType, UnityEvent> events = new Dictionary<EventType, UnityEvent>();
    static readonly Dictionary<DialogEventType, UnityEvent> dialogEvents = new Dictionary<DialogEventType, UnityEvent>();

    public static void Subscribe(EventType eventType, UnityAction listener)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e.AddListener(listener);
        else
        {
            e = new();
            e.AddListener(listener);
            events.Add(eventType, e);
        }
    }

    public static void Unsubscribe(EventType eventType, UnityAction listener)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e.RemoveListener(listener);
    }

    public static void Publish(EventType eventType)
    {
        if (events.TryGetValue(eventType, out UnityEvent e))
            e?.Invoke();
    }

    public static void Subscribe(DialogEventType eventType, UnityAction listener)
    {
        if (dialogEvents.TryGetValue(eventType, out UnityEvent e))
            e.AddListener(listener);
        else
        {
            e = new();
            e.AddListener(listener);
            dialogEvents.Add(eventType, e);
        }
    }

    public static void Unsubscribe(DialogEventType eventType, UnityAction listener)
    {
        if (dialogEvents.TryGetValue(eventType, out UnityEvent e))
            e.RemoveListener(listener);
    }

    public static void Publish(DialogEventType eventType)
    {
        if (dialogEvents.TryGetValue(eventType, out UnityEvent e))
            e?.Invoke();
    }
}
