using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        public string textScript;
        public string textSelect1;
        public string textSelect2;       
        public int textNext1;
        public int textNext2;
    }

    public string currentDialogue;

    public List<DialogueData> dialogueList = new();

    [SerializeField]
    DialogueBox dialogBox;

    [SerializeField]
    int dialogueIdx = 0;

    [SerializeField]
    TextMeshPro playerText;
    [SerializeField]
    PlayerDialogBox playerDialogBox;

    [SerializeField]
    bool isSelect = false;

    // Start is called before the first frame update
    void Start()
    {
        //playerDialogBox = FindObjectOfType<PlayerDialogBox>();        
    }

    public void InitDialogue(string newDialogue, int formal)
    {
        var dialogue = GameManager.Instance.DataBase.Parser(newDialogue);

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
                    textScript = dict["Text_Script"].ToString(),
                    textSelect1 = dict["Text_Select1"].ToString(),
                    textSelect2 = dict["Text_Select2"].ToString(),
                    textNext1 = Tools.IntParse(dict["Text_Next"]),
                    textNext2 = Tools.IntParse(dict["Text_Next2"])
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
        NextDialog();

        while (!dialogueIdx.Equals(-1))
        {
            if(IsOption())
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    NextDialog();
                    GameManager.Instance.CharacterDB.GetNPC(dialogueList[dialogueIdx].characterID).TalkEnd();
                    playerDialogBox.gameObject.SetActive(true);
                    playerDialogBox.ActiveButton(true);
                    yield return new WaitUntil(() => isSelect == true);
                }                    
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    EventManager.Publish(EventType.NextDialog);
                    playerDialogBox.gameObject.SetActive(false);
                    NextDialog();
                }
            }
            
            yield return null;
        }
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerDialogBox.gameObject.SetActive(false);
                EndDialogue();
                yield break;
            }
            yield return null;
        }
    }

    void NextDialog()
    {       
        var dData = dialogueList[dialogueIdx];
        if (dData.characterID == 1)
        {
            playerDialogBox.gameObject.SetActive(true);
            playerDialogBox.SetPlayerDialog(dData.textScript);
        }
        else
        {
            var npc = GameManager.Instance.CharacterDB.GetNPC(dData.characterID);
            npc.Talk(dData.textScript);
        }
            /*dialogBox.SetName(GameManager.Instance.CharacterDB.GetCharacterName(dData.Character_ID));
        dialogBox.SetDialog(dData.Text_Script);*/
        Debug.Log(dData.textScript);
        

        if (IsOption())
        {
            Debug.Log("������");
            isSelect = false;            
            playerDialogBox.SetPlayerDialogOption(dData.textSelect1, dData.textSelect2);
        }
        else
        {
            dialogueIdx = dData.textNext1;
            playerDialogBox.ActiveButton(false);
        }
            
    }

    public void SelectOption1()
    {
        isSelect = true;
        var dData = dialogueList[dialogueIdx];

        dialogueIdx = dData.textNext1;
        playerDialogBox.gameObject.SetActive(false);
        playerDialogBox.ActiveButton(false);

        NextDialog();
    }

    public void SelectOption2()
    {
        isSelect = true;
        var dData = dialogueList[dialogueIdx];

        dialogueIdx = dData.textNext2;
        playerDialogBox.gameObject.SetActive(false);
        playerDialogBox.ActiveButton(false);
        
        NextDialog();
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
    }
}