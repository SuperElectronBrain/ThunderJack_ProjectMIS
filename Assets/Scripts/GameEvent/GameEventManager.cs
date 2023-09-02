using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
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

        var eventData = GameManager.Instance.DataBase.Parser("Random_Event_DataTable");

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

            GameManager.Instance.GameTime.dayEvent += NewDayEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EventSetting(GameEventType eventType, float eventValue)
    {
        switch (eventType)
        {
            case GameEventType.None:
                break;
            case GameEventType.CustomerF:
                break;
            case GameEventType.CustomerA:
                break;
            case GameEventType.CustomerE:
                break;
            case GameEventType.CustomerW:
                break;
            case GameEventType.Fame:
                break;
            case GameEventType.OrePrice:
                break;
            case GameEventType.Collect:
                break;
            case GameEventType.Tax:
                break;
            case GameEventType.VisitRate:
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
