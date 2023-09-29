using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonologueType
{
    Waiting = 1, Craft
}

public enum MonologueType_Crafting
{
    Crafting = 1, Failure, Success
}

public class PlayerMonologue : MonoBehaviour
{
    [SerializeField]
    List<PlayerMonologueData> playerMonologueList = new List<PlayerMonologueData>();
    [SerializeField]
    MonologueType_Crafting type;
    [SerializeField]
    int craftingExp;

    // Start is called before the first frame update
    void Start()
    {
        var playerMonologue = GameManager.Instance.DataBase.Parser("monologue_Master");

        foreach(var monologue in playerMonologue)
        {
            playerMonologueList.Add(
                new PlayerMonologueData
                {
                    textMainType = Tools.IntParse(monologue["Text_Main_Type"]),
                    textSubType = Tools.IntParse(monologue["Text_Sub_Type"]),
                    textScript = monologue["Text_Script"].ToString(),
                    textParam1 = Tools.IntParse(monologue["Parameter_1"]),
                    textParam2 = Tools.IntParse(monologue["Parameter_2"]),
                    textRate = Tools.FloatParse(monologue["Rate"])
                });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log(GetWaitingMonologue());
            Debug.Log(GetCraftingMonologue(type, craftingExp));
        }
    }

    public string GetWaitingMonologue()
    {
        List<PlayerMonologueData> waitingMonologue = new();

        for(int i = 0; i < playerMonologueList.Count; i++)
        {
            if (playerMonologueList[i].textMainType == (int)MonologueType.Waiting)
                waitingMonologue.Add(playerMonologueList[i]);
            else
                break;
        }

        string returnStr = "";

        if (waitingMonologue.Count == 0)
            throw new System.Exception("대기 중 독백이 없습니다.");
        else
        {
            int returnIdx = 0;
            float maxValue = Random.Range(0, waitingMonologue[0].textRate);

            if (waitingMonologue.Count > 1)
            {                
                for (int i = 1; i < waitingMonologue.Count; i++)
                {
                    float randomRate = Random.Range(0, waitingMonologue[i].textRate);
                    if (maxValue < randomRate)
                    {
                        returnIdx = i;
                        maxValue = randomRate;
                    }
                }                
            }

            returnStr = waitingMonologue[returnIdx].textScript;
        }            

        return returnStr;
    }

    public string GetCraftingMonologue(MonologueType_Crafting selectMonologueType, int craftingExp)
    {
        List<PlayerMonologueData> craftingMonologue = new();

        for (int i = 0; i < playerMonologueList.Count; i++)
        {
            if (playerMonologueList[i].textMainType == (int)MonologueType.Waiting)
                continue;
            else if (playerMonologueList[i].textMainType == (int)MonologueType.Craft)
            {
                if (playerMonologueList[i].textSubType == (int)selectMonologueType) // 원하는 독백 종류인지 검사
                {
                    if(playerMonologueList[i].textParam1 <= craftingExp && playerMonologueList[i].textParam2 >= craftingExp) // 제작 횟수가 조건에 맞는지 검사
                        craftingMonologue.Add(playerMonologueList[i]);
                    else
                        continue;
                }                    
                else
                    continue;
            }                
            else
                break;
        }

        string returnStr = "";

        if (craftingMonologue.Count == 0)
            throw new System.Exception("선택한 독백이 없습니다.");
        else
        {
            int returnIdx = 0;
            float maxValue = Random.Range(0, craftingMonologue[0].textRate);

            if (craftingMonologue.Count > 1)
            {
                for (int i = 1; i < craftingMonologue.Count; i++)
                {
                    float randomRate = Random.Range(0, craftingMonologue[i].textRate);
                    if (maxValue < randomRate)
                    {
                        returnIdx = i;
                        maxValue = randomRate;
                    }
                }
            }

            returnStr = craftingMonologue[returnIdx].textScript;
        }

        return returnStr;
    }
}

[System.Serializable]
public class PlayerMonologueData
{
    public int textMainType;
    public int textSubType;
    public string textScript;
    public int textParam1;
    public int textParam2;
    public float textRate;
}

