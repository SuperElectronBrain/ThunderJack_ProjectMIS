using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent
{
    void Event();
}

public class OreEvent : GameEvent, IGameEvent
{
    public void Event()
    {
        NoticeData noticeData = new NoticeData
        {
            noticeName = GetEventData.eventName,
            noticeDescription = GetEventData.eventScript,
        };

        GetGameEventManager.Notice(noticeData);
        Debug.Log("±¤¹°°¡°Ý »ó½Â");
    }

    public override void EventActive()
    {
        Event();
    }    
}