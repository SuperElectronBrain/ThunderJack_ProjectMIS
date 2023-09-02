using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EventType
{
    None
}

public class EventManager
{
    static readonly Dictionary<EventType, UnityEvent> events = new Dictionary<EventType, UnityEvent>();

    public static void Subscribe(EventType eventType, UnityAction listner)
    {
        UnityEvent e;

        if (events.TryGetValue(eventType, out e))
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
        UnityEvent e;

        if (events.TryGetValue(eventType, out e))
            e.RemoveListener(listner);
    }

    public static void Publish(EventType eventType)
    {
        UnityEvent e;

        if (events.TryGetValue(eventType, out e))
            e.Invoke();
    }
}
