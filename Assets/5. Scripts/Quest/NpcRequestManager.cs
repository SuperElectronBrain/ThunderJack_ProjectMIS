using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NpcRequestManager : MonoBehaviour
{
    private const string requestID = "Plea_ID";
    private const string requestName = "Plea_Name";
    private const string requestType = "Plea_Type";
    private const string requestDescription = "Plea_Description";
    private const string requestDay = "Plea_Day";
    private const string requestNPC = "Plea_NPC";
    private const string targetType = "Target_Type";
    private const string targetID = "Target_ID";
    private const string targetScriptID = "Target_Script_ID";
    private const string targetValue = "Target_Value";
    private const string targetComplete = "Target_Complate";
    private const string nextRequest = "Next_Plea";
    private const string rewardType = "Reward_Type";
    private const string rewardValue = "Reward_Value";

    [SerializeField] private List<NpcRequestData> npcRequestDataList;
    [SerializeField] private List<NpcRequestData> inProgressRequests;

    [SerializeField] public Transform requestUiParent;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadNpcRequest();

        Init();
    }

    public void Init()
    {
        requestUiParent = GameObject.FindWithTag("QuestUI").transform;
    }

    void LoadNpcRequest()
    {
        var npcRequestList = GameManager.Instance.DataBase.Parser("Plea_Master");
        npcRequestDataList = new();
        inProgressRequests = new();

        for (var i = 0; i < npcRequestList.Count; i++)
        {
            npcRequestDataList.Add(
                new NpcRequestData
                {
                    requestID = Tools.IntParse(npcRequestList[i][requestID]),
                    requestName = npcRequestList[i][requestName].ToString(),
                    requestType = (NpcRequestType)Tools.EnumParse<NpcRequestType>(npcRequestList[i][requestType]),
                    requestDescription = npcRequestList[i][requestDescription].ToString(),
                    requestDay = Tools.IntParse(npcRequestList[i][requestDay]),
                    requestNPC = Tools.IntParse(npcRequestList[i][requestNPC]),
                    targetType = (NpcRequestTargetType)Tools.EnumParse<NpcRequestTargetType>(npcRequestList[i][targetType]),
                    targetID = Tools.IntParse(npcRequestList[i][targetID]),
                    targetValue = Tools.IntParse(npcRequestList[i][targetValue]),
                    targetScript = Tools.IntParse(npcRequestList[i][targetScriptID]),
                    targetComplete = Tools.IntParse(npcRequestList[i][targetComplete]),
                    nextRequest = Tools.IntParse(npcRequestList[i][nextRequest]),
                    rewardType = Tools.IntParse(npcRequestList[i][rewardType]),
                    rewardValue = Tools.IntParse(npcRequestList[i][rewardValue])
                }
            );
        }
    }

    public NpcRequestData GetNpcRequest(int requestID)
    {
        return npcRequestDataList[requestID - 1];
    }

    public List<NpcRequestData> GetInProgressRequest()
    {
        return inProgressRequests;
    }

    public void AcceptRequest(int requestID)
    {
        inProgressRequests.Add(npcRequestDataList[requestID - 1]);
        StartCoroutine(AddRequest(requestID));
        EventManager.Publish(DialogEventType.QuestStart);
    }

    IEnumerator AddRequest(int index)
    {
        var newRequest = AddressableManager.LoadObject<GameObject>("Request" + index);

        while (nextRequest == null)
        {
            yield return null;
        }

        var request = Instantiate(newRequest, transform.Find("NpcRequest")).GetComponent<NpcRequest>();
        request.SetRequestData(npcRequestDataList[index - 1]);
    }

    public void CompleteRequest(int requestID)
    {
        for(int i = 0;i < inProgressRequests.Count; i++)
        {
            if (inProgressRequests[i].requestID == requestID)
            {
                inProgressRequests.RemoveAt(i);
                EventManager.Publish(DialogEventType.QuestComplate);
            }
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

[System.Serializable]
public class NpcRequestData
{
    public int requestID;
    public string requestName;
    public NpcRequestType requestType;
    public string requestDescription;
    public int requestDay;
    public int requestNPC;
    public NpcRequestTargetType targetType;
    public int targetID;
    public int targetValue;
    public int targetComplete;
    public int targetScript;
    public int nextRequest;
    public int rewardType;
    public int rewardValue;
}
