using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTable : MonoBehaviour
{
    //Dictionary<int, TimeTableData> npcTimeTable;
    [SerializeField]
    List<TimeTableData> npcTimeTable;

    // Start is called before the first frame update
    void Start()
    {
        npcTimeTable = new();

        var timeTable = GameManager.Instance.DataBase.Parser("Time_Table_Day1");
       
        var behaviourMaster = GameManager.Instance.BehaviourMaster;

        for (int i = 1; i < GameManager.Instance.CharacterDB.GetCharacterCount(); i++)
            npcTimeTable.Add(new());

        int h = 0;
        int m = 0;

        foreach (var tData in timeTable)
        {
            var t = h + ":" + string.Format("{0:D2}", m);
            for (int id = 1; id < GameManager.Instance.CharacterDB.GetCharacterCount(); id++)
            {
                var name = GameManager.Instance.CharacterDB.GetCharacterEgName(id);

                npcTimeTable[id - 1].timeTableData[GameManager.Instance.GameTime.GetTimeIdx(h, m)] = behaviourMaster.GetBehaviour(Tools.IntParse(tData[name + "_A"]));
                //tData[name + "_A"];
                //tData[name + "_S"];
            }

            m += 10;
            if (m == 60)
            {
                h++;
                m = 0;
            }

        }
        EventManager.Subscribe(EventType.Minute, WorkDistribution);
        //EventManager.Publish(EventType.hour);
    }

    void WorkDistribution()
    {
        for (int id = 1; id < GameManager.Instance.CharacterDB.GetCharacterCount(); id++)
        {
            var timeIdx = GameManager.Instance.GameTime.GetTimeIdx();
            //npcTimeTable[id].timeTableData[locationIdx];
            //GameManager.Instance.GetCharacter(id).           
            //GameManager.Instance.CharacterDB.GetCharacter(id).GetComponent<NPC_Move>().SetDestination(GameManager.Instance.LocationManager.GetLocationPosition(locationIdx));
            ((NPC)GameManager.Instance.CharacterDB.GetCharacter(id)).SetCurBehaviourData(npcTimeTable[id - 1].timeTableData[timeIdx]);
        }
    }

    /*    public NPCBehaviour GetMyBehaviour(int characterId, int timeIdx)
        {
            //return npcTimeTable[characterId].timeTableData[timeIdx];
        }*/

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Minute, WorkDistribution);
    }

    private void Awake()
    {
        
    }
}

[System.Serializable]
public class TimeTableData
{
    public BehaviourData[] timeTableData = new BehaviourData[144];
    //public Dictionary<int, BehaviourData> timeTableData = new();
}

