using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public abstract void EventActive();
    public abstract void EventInactive();
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

namespace RandomGameEvent
{
    public class FameEvent : GameEvent
    {
        public override void EventActive()
        {
            
        }

        public override void EventInactive()
        {
            
        }
    }

    public class OreEvent : GameEvent
    {
        public override void EventActive()
        {
            NoticeData noticeData = new NoticeData
            {
                noticeName = GetEventData.eventName,
                noticeDescription = GetEventData.eventScript,
            };

            GetGameEventManager.Notice(noticeData);
            Debug.Log("±¤¹°°¡°Ý »ó½Â");
        }

        public override void EventInactive()
        {

        }
    }
}