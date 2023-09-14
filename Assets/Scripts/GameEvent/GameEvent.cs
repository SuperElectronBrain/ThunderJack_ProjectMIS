using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public abstract void EventActive();
    GameEventManager gameEventManager;
    EventData eventData;

    public void InitEvent(GameEventManager gameEventManager, EventData eventData)
    {
        this.gameEventManager = gameEventManager;
        this.eventData = eventData;
    }
}
