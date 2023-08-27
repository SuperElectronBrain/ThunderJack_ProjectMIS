using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text dialogBox;

    static DialogueBox instance;
    public static DialogueBox Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    public void SetDialog(string newDialog)
    {
        dialogBox.text = newDialog;
    }
}
