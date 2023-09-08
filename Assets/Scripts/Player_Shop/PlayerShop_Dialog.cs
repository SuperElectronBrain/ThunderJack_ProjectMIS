using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShop_Dialog : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text dialogBox;

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
