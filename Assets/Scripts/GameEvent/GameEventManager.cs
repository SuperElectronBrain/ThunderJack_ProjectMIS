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
        Invoke("NewDayEvent", 1f);
    }

    public void Notice(NoticeData noticeData)
    {
        noticeBoard.SetNoticeBoard(noticeData);
    }

    public void NewDayEvent()
    {
        /*int randomEventIdx = Random.Range(0, gameEventData.Count);

        switch ((GameEventType)gameEventData[randomEventIdx].eventType)
        {
            case GameEventType.None:
                break;
            case GameEventType.Fame:
                break;
            case GameEventType.OrePrice:
                dayGameEvent = new OreEvent();
                break;
            case GameEventType.Collect:
                break;
            case GameEventType.AppearDemonLord:
                break;
        }*/
        dayGameEvent = new OreEvent();
        dayGameEvent.InitEvent(this, gameEventData[7]);

        dayGameEvent.EventActive();
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