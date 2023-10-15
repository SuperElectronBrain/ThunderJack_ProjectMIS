using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcQuestType
{
    Normal = 1, Chain
}

public enum NpcQuestTargetType
{
    NPC = 1, Object, Item
}

public enum NpcQuestRewardType
{
    None = -1, Gold = 1, Item
}

public class NpcQuestManager : MonoBehaviour
{
    [SerializeField]
    List<NpcQuestData> npcQuestData;

    // Start is called before the first frame update
    void Start()
    {
        npcQuestData = new List<NpcQuestData>();

        var npcQuests = GameManager.Instance.DataBase.Parser("Plea_Master");

        foreach (var npcQuest in npcQuests)
        {
            npcQuestData.Add(
                new NpcQuestData
                {
                    questID = Tools.IntParse(npcQuest["Plea_ID"]),
                    questName = npcQuest["Plea_Name"].ToString(),
                    questType = (NpcQuestType)Tools.IntParse(npcQuest["Plea_Type"]),
                    questDay = Tools.IntParse(npcQuest["Plea_Day"]),
                    npcID = Tools.IntParse(npcQuest["Plea_NPC"]),
                    targetType = (NpcQuestTargetType)Tools.IntParse(npcQuest["Target_Type"]),
                    targetID = Tools.IntParse(npcQuest["Target_ID"]),
                    targetValue = Tools.IntParse(npcQuest["Target_Value"]),
                    targetComplate = Tools.IntParse(npcQuest["Target_Complate"]),
                    nextQuest = Tools.IntParse(npcQuest["Next_Plea"]),
                    rewardType = (NpcQuestRewardType)Tools.IntParse(npcQuest["Reward_Type"]),
                    rewardValue = Tools.IntParse(npcQuest["Reward_Value"])
                }
                );
        }

        EventManager.Subscribe(EventType.Day, ChangeDialogToQuestDialog);
    }

    public void ChangeDialogToQuestDialog()
    {

    }

    void SearchTodayQuest()
    {
        for(int i = 0; i < npcQuestData.Count; i++)
        {
            if(npcQuestData[i].questDay == GameManager.Instance.GameTime.GetDay())
            {

            }
        }
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Day, ChangeDialogToQuestDialog);
    }
}

[System.Serializable]
public class NpcQuestData
{
    public int questID;
    public string questName;
    public NpcQuestType questType;
    public int questDay;
    public int npcID;
    public NpcQuestTargetType targetType;
    public int targetID;
    public int targetValue;
    public int targetComplate;
    public int nextQuest;
    public NpcQuestRewardType rewardType;
    public int rewardValue;
}
