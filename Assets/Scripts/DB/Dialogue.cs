using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueData
    {
        public int Text_ID;
        public int Character_ID;
        public int Animation_ID;
        public int Text_Type;
        public int Fame_Grade;
        public int Text_Day;
        public float Text_Rate;
        public string Text_Script;
        public int Text_Next;
    }

    public string currentDialogue;

    public List<DialogueData> dialogueList = new();

    int dialogueIdx = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitDialogue(string newDialogue)
    {
        var dialogue = DataBase.Instance.Parser(newDialogue);

        foreach (var dict in dialogue)
        {
            int charId = int.Parse(dict["Character_ID"].ToString());

            dialogueList.Add(
                new DialogueData
                {
                    Text_ID = int.Parse(dict["Text_ID"].ToString()),
                    Character_ID = charId,
                    Animation_ID = int.Parse(dict["Animation_ID"].ToString()),
                    Text_Type = int.Parse(dict["Text_Type"].ToString()),
                    Fame_Grade = int.Parse(dict["Fame_Grade"].ToString()),
                    Text_Day = int.Parse(dict["Text_Day"].ToString()),
                    Text_Rate = float.Parse(dict["Text_Rate"].ToString()),
                    Text_Script = dict["Text_Script"].ToString(),
                    Text_Next = int.Parse(dict["Text_Next"].ToString())
                }
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DialogueBox.Instance.SetName(DataBase.Instance.GetCharacterName(dialogueList[dialogueIdx].Character_ID));
            DialogueBox.Instance.SetDialog(dialogueList[dialogueIdx].Text_Script);
            //Character.Instance.PlayAnimation((AnimationType)dialogueList[dialogueIdx].Animation_ID);
            GameManager.Instance.GetCharacter(dialogueList[dialogueIdx].Character_ID).PlayAnimation((AnimationType)dialogueList[dialogueIdx].Animation_ID);

            dialogueIdx = dialogueList[dialogueIdx].Text_Next;
        }
    }

    public void EndDialogue()
    {
        dialogueList.Clear();
    }
}
