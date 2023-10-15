using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    List<QuestData> questList;

    // Start is called before the first frame update
    void Start()
    {
        questList = new List<QuestData>();
        var questDB = GameManager.Instance.DataBase.Parser("Quest_Master");

        foreach (var quest in questDB)
        {
            questList.Add(
                new QuestData
                {
                    questID = Tools.IntParse(quest["Quest_ID"]),
                    questName = quest["Quest_Name"].ToString(),
                    questTimeLimit = Tools.IntParse(quest["TimeLimit"]),
                    guestID = Tools.IntParse(quest["Guest_ID"]),
                    questGrade = Tools.IntParse(quest["Quest_Grade"]),
                    questScript = quest["Quest_Script"].ToString(),
                    questCharacter = quest["Quest_Character"].ToString(),
                    requestItemID = Tools.IntParse(quest["Request"]),
                    startRate = Tools.FloatParse(quest["StartRate"]),
                    resetRate = Tools.FloatParse(quest["ResetRate"]),
                    rate = Tools.FloatParse(quest["Rate"]),
                    dayRate = Tools.FloatParse(quest["DayRate"])
                }
                );
        }
    }

    public QuestData GetQuestData(int questID)
    {
        return questList[questID - 1];
    }

    public List<QuestData> GetQuestList()
    {
        return questList;
    }

    public void SetQuestRate(int questID, float questRate)
    {
        if(questID > questList.Count || questID < 0)
        {
            Debug.LogError("ÀÇ·Ú ÀÎµ¦½º ¿À·ù");
            return;
        }

        questList[questID - 1].rate = questRate;
    }

    public int GetQuestCount()
    {
        return questList.Count;
    }
}

[System.Serializable]
public class QuestData
{
    public int questID;
    public string questName;
    public int questTimeLimit;
    public int guestID;
    public int questGrade;
    public string questScript;
    public string questCharacter;
    public int requestItemID;
    public float startRate;
    public float resetRate;
    public float rate;
    public float dayRate;
}
