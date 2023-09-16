using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public abstract void EventActive();
    GameEventManager gameEventManager;
    EventData eventData;

    public void InitEvent(GameEventManager gameEventManager, EventData eventData)
    {
        this.gameEventManager = gameEventManager;
        this.eventData = eventData;
    }

    protected int GetEventValue { get { return eventData.eventValue; } }
    protected GameEventManager GetGameEventManager { get { return gameEventManager; } }
    protected EventData GetEventData { get { return eventData; } }
}
