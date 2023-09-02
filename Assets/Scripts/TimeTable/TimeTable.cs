using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTable : MonoBehaviour
{
    Dictionary<int, TimeTableData> npcTimeTable;

    // Start is called before the first frame update
    void Start()
    {
        npcTimeTable = new Dictionary<int, TimeTableData>();

        var timeTable = GameManager.Instance.DataBase.Parser("Time_Table_Day1");
       
        var behaviourMaster = GameManager.Instance.BehaviourMaster;

        foreach (var tData in timeTable)
        {
            int h = 0;
            int m = 10;

            while(h != 24)
            {
                var t = h + ":" + string.Format("{0:D2}", m);
                //Debug.Log(GameManager.Instance.GameTime.GetTimeIdx(h, m));
                //Debug.Log(t + " : " + tData[t]);
                for (int id = 1; id < GameManager.Instance.CharacterDB.GetCharacterCount(); id++)
                {
                    string npcName = GameManager.Instance.CharacterDB.GetCharacterEgName(id);
                    

                    if (npcTimeTable.TryAdd(id, new()) == false)
                        npcTimeTable[id].timeTableData[GameManager.Instance.GameTime.GetTimeIdx(h, m)] = behaviourMaster.GetBehaviour(Tools.IntParse(tData[t]));
                    else
                        npcTimeTable[id].timeTableData[GameManager.Instance.GameTime.GetTimeIdx(h, m)] = behaviourMaster.GetBehaviour(Tools.IntParse(tData[t]));
                }

                m += 10;
                if (m == 60)
                {
                    h++;
                    m = 0;
                }
            }
        }

        EventManager.Subscribe(EventType.hour, WorkDistribution);
    }

    void WorkDistribution()
    {
        for (int id = 1; id < GameManager.Instance.CharacterDB.GetCharacterCount(); id++)
        {
            var locationIdx = GameManager.Instance.GameTime.GetTimeIdx();
            //npcTimeTable[id].timeTableData[locationIdx];
            Debug.Log(locationIdx);
            //GameManager.Instance.GetCharacter(id).
            GameManager.Instance.CharacterDB.GetCharacter(id).GetComponent<NPC_Move>().SetDestination(GameManager.Instance.LocationManager.GetLocationPosition(locationIdx));
        }
    }

    /*    public NPCBehaviour GetMyBehaviour(int characterId, int timeIdx)
        {
            //return npcTimeTable[characterId].timeTableData[timeIdx];
        }*/

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.hour, WorkDistribution);
    }
}

public class TimeTableData
{
    public BehaviourData[] timeTableData = new BehaviourData[144];
    //public Dictionary<int, BehaviourData> timeTableData = new();
}
