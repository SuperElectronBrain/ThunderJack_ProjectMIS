using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeTable : MonoBehaviour
{
    public List<Dictionary<string, TimeTableData>> timeTableList = new List<Dictionary<string, TimeTableData>>();

    private void Start()
    {
        LoadTimeTable();

        EventManager.Subscribe(EventType.Minute, ScheduleDistribution);
        ScheduleDistribution();
    }

    void LoadTimeTable()
    {
        DataBase_Character dc = GameManager.Instance.CharacterDB;

        for(int i = 1; i <= dc.GetCharacterCount(); i++)
        {
            timeTableList.Add(new Dictionary<string, TimeTableData>());

            int day = GameManager.Instance.GameTime.GetDay();
            try
            {
                var timeTable = GameManager.Instance.DataBase.Parser(dc.GetCharacterEgName(i) + "_Time_Table_Day" + day);

                foreach (var t in timeTable)
                {
                    timeTableList[i - 1].Add(t["ConditionTime"].ToString(),
                        new TimeTableData
                        {
                            aiType = Tools.IntParse(t["AIType"]),
                            aiParam1 = Tools.IntParse(t["AiParameter"])
                        }
                        );
                }
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex);
                Debug.Log(dc.GetCharacterEgName(i) + "_Time_Table_Day" + day);
                throw new System.Exception(dc.GetCharacterEgName(i) + "의 타임테이블을 찾을 수 없습니다");
            }            
        }
    }

    void ScheduleDistribution()
    {
        DataBase_Character dc = GameManager.Instance.CharacterDB;
        GameTime gt = GameManager.Instance.GameTime;

        for (int i = 1; i <= dc.GetCharacterCount(); i++)
        {
            Debug.Log(gt.GetTime());
            NPC npc = dc.GetNPC(i);
            var key = gt.GetTime();

            npc.SetSchedule(timeTableList[i - 1][key]);

            //.SetSchedule(timeTableList[i][gt.GetTime()]);
        }
    }
}

[System.Serializable]
public class TimeTableData
{
    public int aiType;
    public int aiParam1;
}