using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDialogBox : MonoBehaviour
{
    Button button1;
    Button button2;
    TextMeshPro dialogScript;
    TextMeshPro dialogName;
       
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayerDialog(string script)
    {
        dialogScript.text = script;
    }

    public void SetPlayerDialogOption(string option1, string option2)
    {

    }
}

public class PlayerDialogOption
{
    public string button1Text;
    public string button2Text;
}
