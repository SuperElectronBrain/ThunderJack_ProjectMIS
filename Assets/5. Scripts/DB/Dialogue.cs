using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DialogEventType
{
    None = -1, ShopGemOpen = 1, ShopJewelryOpen, QuestStart, QuestComplate
}

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueData
    {
        public int textID;
        public int characterID;
        public int animationID;
        public int textType;
        public int textFormal;          
        public string textScript1;
        public string textScript2;
        public string textSelect1;
        public string textSelect2;       
        public int textNext1;
        public int textNext2;
        public DialogEventType eventID1;
        public DialogEventType eventID2;
    }

    public string currentDialogue;

    public List<DialogueData> dialogueList = new();

    [SerializeField]
    int dialogueIdx = 0;
    
    [SerializeField]
    PlayerDialogBox playerDialogBox;

    [SerializeField]
    bool isSelect = false;

    // Start is called before the first frame update
    void Start()
    {
        //playerDialogBox = FindObjectOfType<PlayerDialogBox>();      
        //EventManager.Subscribe(EventType.NextDialog, ShowOption);
    }

    public void InitSetting(PlayerDialogBox playerDialog)
    {
        playerDialogBox = playerDialog;
    }

    public void InitDialogue(string newDialogue, int formal)
    {
        var dialogue = GameManager.Instance.DataBase.Parser(newDialogue);
        Debug.Log(newDialogue + "Read Dialogue " + formal);

        foreach (var dict in dialogue)
        {
            int charId = int.Parse(dict["Character_ID"].ToString());

            dialogueList.Add(
                new DialogueData
                {                    
                    textID = Tools.IntParse(dict["Text_ID"]),
                    characterID = charId,
                    textType = Tools.IntParse(dict["Text_Type"]),
                    animationID = Tools.IntParse(dict["Animation_ID"]),
                    textFormal = Tools.IntParse(dict["Text_Formal"]),
                    textScript1 = dict["Text_Script_1"].ToString(),
                    textScript2 = dict["Text_Script_2"].ToString(),
                    textSelect1 = dict["Text_Select1"].ToString(),
                    textSelect2 = dict["Text_Select2"].ToString(),
                    textNext1 = Tools.IntParse(dict["Text_Next1"]),
                    textNext2 = Tools.IntParse(dict["Text_Next2"]),
                    eventID1 = (DialogEventType)Tools.IntParse(dict["Event_ID_1"]),
                    eventID2 = (DialogEventType)Tools.IntParse(dict["Event_ID_2"])
                }
            );
        }

        for(int i = 0; i < dialogueList.Count; i++)
        {
            if(dialogueList[i].textFormal == formal)
            {
                dialogueIdx = i;
                break;
            }
        }

        StartCoroutine(StartConversation());
    }

    IEnumerator StartConversation()
    {
        isSelect = true;
        var dData = dialogueList[dialogueIdx];

        CurDialog(dData);

        if (!isSelect)
        {
            yield return new WaitUntil(() => isSelect);
            dData = dialogueList[dialogueIdx];
            CurDialog(dData);
        }
            
        
        while (dialogueIdx != -1)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isSelect)
                    yield return new WaitUntil(() => isSelect);
                
                EventManager.Publish(dData.eventID1);
                EventManager.Publish(EventType.NextDialog);
                playerDialogBox.gameObject.SetActive(false);

                if (!IsOption())
                {
                    dialogueIdx = dData.textNext1;

                    if (dialogueIdx == -1)
                    {
                        playerDialogBox.gameObject.SetActive(false);
                        EndDialogue();
                        break;
                    }
                    
                    dData = dialogueList[dialogueIdx];
                }
                CurDialog(dData);
            }
        }

        /*while (true)
        {
            yield return null;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerDialogBox.gameObject.SetActive(false);
                EndDialogue();
                yield break;
            }
        }*/
    }

    IEnumerator CShowOption(bool isFirts = false)
    {
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.E))
            {
                EventManager.Publish(EventType.NextDialog);
                playerDialogBox.gameObject.SetActive(false);
                isSelect = false;
                var dData = dialogueList[dialogueIdx];
                playerDialogBox.SetPlayerDialogOption(dData.textSelect1, dData.textSelect2);
                GameManager.Instance.CharacterDB.GetNPC(dialogueList[dialogueIdx].characterID).TalkEnd();
                //playerDialogBox.gameObject.SetActive(true);
                playerDialogBox.ActiveButton(true);
                yield break;
            }
        }
    }

    void CurDialog(DialogueData dData)
    {
        if (dData.characterID == 0)
        {
            playerDialogBox.SetPlayerDialog(dData.textScript1);
            playerDialogBox.gameObject.SetActive(true);
        }
        else
        {
            var npc = GameManager.Instance.CharacterDB.GetNPC(dData.characterID);
            npc.Talk(dData.textScript1);
        }

        if (IsOption())
        {
            isSelect = false;
            StartCoroutine(CShowOption());
        }
        else
        {

        }
    }

    void NextDialog()
    {       
        var dData = dialogueList[dialogueIdx];
        if (dData.characterID == 0)
        {
            playerDialogBox.SetPlayerDialog(dData.textScript1);
            playerDialogBox.gameObject.SetActive(true);
        }
        else
        {
            var npc = GameManager.Instance.CharacterDB.GetNPC(dData.characterID);
            npc.Talk(dData.textScript1);
        }
        
        Debug.Log(dData.textScript1);
        

        if (IsOption())
        {
            /*Debug.Log("������");
            isSelect = false;            
            playerDialogBox.SetPlayerDialogOption(dData.textSelect1, dData.textSelect2);*/
        }
        else
        {
            ActiveDialogEvent();
            dialogueIdx = dData.textNext1;
            playerDialogBox.ActiveButton(false);
        }
    }

    void ActiveDialogEvent()
    {
        EventManager.Publish(dialogueList[dialogueIdx].eventID1);
    }

    public void SelectOption1()
    {
        var dData = dialogueList[dialogueIdx];

        EventManager.Publish(dData.eventID1);
        
        dialogueIdx = dData.textNext1;
        dData = dialogueList[dialogueIdx];
        
        playerDialogBox.gameObject.SetActive(false);
        playerDialogBox.ActiveButton(false);
     
        isSelect = true;
        //CurDialog(dData);
        //NextDialog();
    }

    public void SelectOption2()
    {
        var dData = dialogueList[dialogueIdx];

        EventManager.Publish(dData.eventID2);
        
        dialogueIdx = dData.textNext2;
        dData = dialogueList[dialogueIdx];
        
        playerDialogBox.gameObject.SetActive(false);
        playerDialogBox.ActiveButton(false);
        
        isSelect = true;
        //CurDialog(dData);
        //NextDialog();
    }

    bool IsOption()
    {
        if (dialogueIdx == -1)
            return false;
        return dialogueList[dialogueIdx].textType == 2;
    }

    public void EndDialogue()
    {
        dialogueIdx = 0;
        dialogueList.Clear();
        EventManager.Publish(EventType.EndConversation);
        EventManager.Publish(EventType.EndIteraction);
        GameManager.Instance.GameTime.TimeStop(false);
    }
}
