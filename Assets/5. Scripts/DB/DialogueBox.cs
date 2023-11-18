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
        dialogBox.text = newDialog.Replace("\\n", "\n");
    }

    public void SetAcceptButton(string newDialog)
    {
        if (newDialog.Equals("-1"))
            acceptButtonText.transform.parent.parent.gameObject.SetActive(false);
        else
            acceptButtonText.transform.parent.parent.gameObject.SetActive(true);
        acceptButtonText.text = newDialog;
    }

    public void SetRefusalButton(string newDialog)
    {
        if (newDialog.Equals("-1"))
            refusalButtonText.transform.parent.parent.gameObject.SetActive(false);
        else
            refusalButtonText.transform.parent.parent.gameObject.SetActive(true);
        refusalButtonText.text = newDialog;
    }

    public void ShowDialogBox(bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }
}
