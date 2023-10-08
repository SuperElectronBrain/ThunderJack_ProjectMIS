using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI dialogBox;

    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    public void SetDialog(string newDialog)
    {
        dialogBox.text = newDialog;
    }

    public void ShowDialogBox(bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }
}
