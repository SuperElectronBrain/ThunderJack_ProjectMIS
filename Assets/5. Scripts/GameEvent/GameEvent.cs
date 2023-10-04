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

    public GameEventType GetEventType { get { return (GameEventType)eventData.eventType; } }
    public float GetEventValue { get { return eventData.eventValue; } }
    protected void SetNoticeBorad()
    {
        NoticeData noticeData = new NoticeData
        {
            noticeName = GetEventData.eventName,
            noticeDescription = GetEventData.eventScript,
        };

        gameEventManager.Notice(noticeData);
    }
    protected GameEventManager GetGameEventManager { get { return gameEventManager; } }
    protected EventData GetEventData { get { return eventData; } }
}

namespace RandomGameEvent
{
    public class Sunny : GameEvent
    {
        public override void EventActive()
        {
            SetNoticeBorad();
        }

        public override void EventInactive()
        {
            
        }
    }

    public class FameEvent : GameEvent
    {
        public override void EventActive()
        {
            SetNoticeBorad();
        }

        public override void EventInactive()
        {
            
        }
    }

    public class OreEvent : GameEvent
    {
        public override void EventActive()
        {
            SetNoticeBorad();
        }

        public override void EventInactive()
        {

        }
    }

    public class AppearDemonLoad : GameEvent
    {
        public override void EventActive()
        {
            SetNoticeBorad();
        }

        public override void EventInactive()
        {
            
        }
    }
}