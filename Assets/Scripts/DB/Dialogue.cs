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
        public int textSelect1;
        public int textSelect2;       
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitDialogue(string newDialogue)
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
                    textSelect1 = Tools.IntParse(dict["Text_Select1"]),
                    textSelect2 = Tools.IntParse(dict["Text_Select2"]),
                    textNext1 = Tools.IntParse(dict["Text_Next"]),
                    textNext2 = Tools.IntParse(dict["Text_Next2"])
                }
            );
        }

        StartCoroutine(StartConversation());
    }

    IEnumerator StartConversation()
    {
        NextDialog();
        //dialogBox.ShowDialogBox();
        while (!dialogueIdx.Equals(-1))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EventManager.Publish(EventType.NextDialog);
                playerText.gameObject.SetActive(false);
                NextDialog();
            }
            yield return null;
        }
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerText.gameObject.SetActive(false);
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
            playerText.gameObject.SetActive(true);
            playerText.text = dData.textScript;
        }
        else
        {
            var npc = GameManager.Instance.CharacterDB.GetNPC(dData.characterID);
            npc.Talk(dData.textScript);
        }
            /*dialogBox.SetName(GameManager.Instance.CharacterDB.GetCharacterName(dData.Character_ID));
        dialogBox.SetDialog(dData.Text_Script);*/
        Debug.Log(dData.textScript);
        dialogueIdx = dData.textNext1;
    }

    public void EndDialogue()
    {
        dialogueIdx = 0;
        dialogueList.Clear();
        EventManager.Publish(EventType.EndConversation);
    }
}
