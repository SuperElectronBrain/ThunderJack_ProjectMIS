using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI dialogBox;

    [SerializeField]
    TextMeshProUGUI acceptButtonText;
    [SerializeField]
    TextMeshProUGUI refusalButtonText;

    public void SetName(string newName)
    {
        nameText.text = newName;
    }

    public void SetDialog(string newDialog)
    {
        dialogBox.text = newDialog;
    }

    public void SetButton(int dialogType)
    {
        switch(dialogType)
        {
            case 1:
                acceptButtonText.text = "¥Ÿ¿Ω.";
                break;
            case 2:
                break;
        }
    }

    public void ShowDialogBox(bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }
}
