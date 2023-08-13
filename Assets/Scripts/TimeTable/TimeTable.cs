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

        var timeTable = DataBase.Instance.Parser("TimeTable");

        int h = 0;
        int m = 10;

        foreach (var tData in timeTable)
        {
            for(int id = 1; id < GameManager.Instance.GetCharacterCount(); id++)
            {
                string npcName = GameManager.Instance.GetCharacterName(id);
                if (tData[npcName].ToString() != string.Empty)
                {
                    Debug.Log(npcName + " : " + h + ":" + m + " : " + (tData[npcName].ToString()));
                }                    
            }
            
            m += 10;
            if (m == 60)
            {
                h++;
                m = 0;
            }
            if(h == 24)
                h = 0;
            
        }
        //npcTimeTable.Add(1,)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NPCBehaviour GetMyBehaviour(int characterId, int timeIdx)
    {
        return npcTimeTable[characterId].timeTableData[timeIdx];
    }
}

public class TimeTableData
{
    public Dictionary<int, NPCBehaviour> timeTableData;
}
