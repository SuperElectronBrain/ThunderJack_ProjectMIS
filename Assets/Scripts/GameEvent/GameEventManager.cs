using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameEventType
{
    None, Fame, OrePrice, Collect , AppearDemonLord = 6
}

public class GameEventManager : MonoBehaviour
{
    GameEvent dayGameEvent;
    GameEventType dayGameEventType;

    [SerializeField]
    List<EventData> gameEventData;
    List<GameEvent> gameEventList;
    [SerializeField]
    List<GameEventBase> gameEventListBase;

    [SerializeField]
    NoticeBoard noticeBoard;

    [SerializeField]
    int positiveEventWeight;
    [SerializeField]
    int negativeEventWeight;

    // Start is called before the first frame update
    void Start()
    {
        gameEventData = new();

        noticeBoard = GameObject.Find("Notice Board").GetComponent<NoticeBoard>();

        var eventData = GameManager.Instance.DataBase.Parser("Random_Event_DataTable");

        foreach (var e in eventData)
        {
            var eId = (e["Event_ID"]).ToString();
            gameEventData.Add(new EventData
            {
                eventName = e["Event_Name"].ToString(),
                eventScript = e["Event_Script"].ToString(),
                eventType = Tools.IntParse(e["Event_Type"]),
                eventValue = Tools.IntParse(e["Event_Value"])
            }
            );
        }

        EventManager.Subscribe(EventType.Day, NewDayEvent);
        EventManager.Publish(EventType.Day);
    }

    public void Notice(NoticeData noticeData)
    {
        noticeBoard.SetNoticeBoard(noticeData);
    }

    public void NewDayEvent()
    {
        int randomEventIdx = Random.Range(0, gameEventData.Count);

        switch ((GameEventType)gameEventData[randomEventIdx].eventType)
        {
            case GameEventType.None:
                break;
            case GameEventType.Fame:
                dayGameEvent = new RandomGameEvent.FameEvent();
                break;
            case GameEventType.OrePrice:
                dayGameEvent = new RandomGameEvent.OreEvent();
                break;
            case GameEventType.Collect:
                break;
            case GameEventType.AppearDemonLord:
                break;
        }
        dayGameEvent.InitEvent(this, gameEventData[randomEventIdx]);

        dayGameEvent.EventActive();
    }

    public GameEventType GetGameEventType()
    {
        return dayGameEvent.GetEventType;
    }

    public int GetGameEventValue()
    {
        return dayGameEvent.GetEventValue;
    }
}

[System.Serializable]
public class EventData
{
    public string eventName;
    public string eventScript;
    public int eventType;
    public int eventValue;
}

[System.Serializable]
public class GameEventBase
{
    public UnityEvent gameEvent;
}