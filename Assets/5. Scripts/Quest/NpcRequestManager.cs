using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcRequestManager : MonoBehaviour
{
    private const string requestID = "Plea_ID";
    private const string requestName = "Plea_Name";
    private const string requestType = "Plea_Type";
    private const string requestDay = "Plea_Day";
    private const string requestNPC = "Plea_NPC";
    private const string targetType = "Target_Type";
    private const string targetID = "Target_ID";
    private const string targetScriptID = "Target_Script_ID";
    private const string targetValue = "Target_Value";
    private const string targetComplate = "Target_Complate";
    private const string nextRequest = "Next_Plea";
    private const string rewardType = "Reward_Type";
    private const string rewardValue = "Reward_Value";

    [SerializeField] private List<NpcRequestData> npcRequestDataList;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadNpcRequest();
    }

    void LoadNpcRequest()
    {
        var npcRequestList = GameManager.Instance.DataBase.Parser("Plea_Master");
        npcRequestDataList = new();

        for (var i = 0; i < npcRequestList.Count; i++)
        {
            npcRequestDataList.Add(
                new NpcRequestData
                {
                    requestID = Tools.IntParse(npcRequestList[i][requestID]),
                    requestName = npcRequestList[i][requestID].ToString(),
                    requestType = (NpcRequestType)Tools.EnumParse<NpcRequestType>(npcRequestList[i][requestID]),
                    requestDay = Tools.IntParse(npcRequestList[i][requestID]),
                    requestNPC = Tools.IntParse(npcRequestList[i][requestID]),
                    targetType = (NpcRequestTargetType)Tools.EnumParse<NpcRequestTargetType>(npcRequestList[i][requestID]),
                    targetID = Tools.IntParse(npcRequestList[i][requestID]),
                    targetValue = Tools.IntParse(npcRequestList[i][requestID]),
                    targetComplete = Tools.IntParse(npcRequestList[i][requestID]),
                    nextRequest = Tools.IntParse(npcRequestList[i][requestID]),
                    rewardType = Tools.IntParse(npcRequestList[i][requestID]),
                    rewardValue = Tools.IntParse(npcRequestList[i][requestID])
                }
            );
        }
    }
}

public enum NpcRequestTargetType
{
    Npc = 1, Object, Item
}

public enum NpcRequestType
{
    Normal = 1, Link
}

public class NpcRequestData
{
    public int requestID;
    public string requestName;
    public NpcRequestType requestType;
    public int requestDay;
    public int requestNPC;
    public NpcRequestTargetType targetType;
    public int targetID;
    public int targetValue;
    public int targetComplete;
    public int nextRequest;
    public int rewardType;
    public int rewardValue;
}
