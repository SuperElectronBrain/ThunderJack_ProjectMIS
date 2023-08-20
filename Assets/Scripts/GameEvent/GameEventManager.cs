using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None, CustomerF, CustomerA, CustomerE, CustomerW, Fame, OrePrice, Collect ,Tax, VisitRate
}

public class GameEventManager : MonoBehaviour
{
    GameEvent gameEvent;
    Dictionary<string, EventData> gameEventData;

    // Start is called before the first frame update
    void Start()
    {
        gameEventData = new Dictionary<string, EventData>();

        var eventData = DataBase.Instance.Parser("Random_Event_DataTable");

        foreach (var e in eventData)
        {
            var eId = (e["Event_ID"]).ToString();
            gameEventData.Add(eId, new EventData
            {
                eventId = eId,
                eventName = e["Event_Name"].ToString(),
                eventScript = e["Event_Script"].ToString(),
                eventType = Tools.IntParse(e["Event_Type"]),
                eventValue = Tools.IntParse(e["Event_Value"])
            }
            );
        }

            GameTime.Instance.dayEvent += NewDayEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EventSetting(EventType eventType, float eventValue)
    {
        switch (eventType)
        {
            case EventType.None:
                break;
            case EventType.CustomerF:
                break;
            case EventType.CustomerA:
                break;
            case EventType.CustomerE:
                break;
            case EventType.CustomerW:
                break;
            case EventType.Fame:
                break;
            case EventType.OrePrice:
                break;
            case EventType.Collect:
                break;
            case EventType.Tax:
                break;
            case EventType.VisitRate:
                break;
        }
    }

    public void NewDayEvent()
    {      


        //EventSetting();
        gameEvent.EventEffect();
    }
}

public class EventData
{
    public string eventId;
    public string eventName;
    public string eventScript;
    public int eventType;
    public int eventValue;
}
