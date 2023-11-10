using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private static UnityAction<MonologueType_Crafting, int> craftDialog;

    [SerializeField] private PlayerCharacter pc;
    
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

        craftDialog += TalkMonologue;
    }

    private void TalkMonologue(MonologueType_Crafting selectMonologueType, int craftingExp)
    {
        var monologueText = GetCraftingMonologue(selectMonologueType, craftingExp);

        pc.PopUpMonologue(monologueText, true);
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
            throw new System.Exception("��� �� ������ �����ϴ�.");
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
                if (playerMonologueList[i].textSubType == (int)selectMonologueType) // ���ϴ� ���� �������� �˻�
                {
                    if(playerMonologueList[i].textParam1 <= craftingExp && playerMonologueList[i].textParam2 >= craftingExp) // ���� Ƚ���� ���ǿ� �´��� �˻�
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
            throw new System.Exception("������ ������ �����ϴ�.");
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

