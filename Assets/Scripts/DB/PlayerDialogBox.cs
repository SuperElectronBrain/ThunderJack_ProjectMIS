using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDialogBox : MonoBehaviour
{
    [SerializeField]
    PlayerDialogButton button1;
    [SerializeField]
    PlayerDialogButton button2;
    [SerializeField]
    TextMeshPro dialogScript;
    [SerializeField]
    TextMeshPro dialogName;
       
    // Start is called before the first frame update
    void Start()
    {
        button1 = transform.Find("SelectOption1").GetComponent<PlayerDialogButton>();
        button2 = transform.Find("SelectOption2").GetComponent<PlayerDialogButton>();

        button1.selectOption += GameManager.Instance.Dialogue.SelectOption1;
        button2.selectOption += GameManager.Instance.Dialogue.SelectOption2;

        dialogName.text = transform.root.gameObject.name;
        gameObject.SetActive(false);
    }

    public void SetPlayerDialog(string script)
    {
        dialogScript.text = script;
    }

    public void SetPlayerDialogOption(string option1, string option2)
    {
        button1.SetOptionText(option1);
        button2.SetOptionText(option2);
    }

    public void ActiveButton(bool active)
    {
        button1.gameObject.SetActive(active);
        button2.gameObject.SetActive(active);
        dialogScript.gameObject.SetActive(!active);
    }

    private void OnDestroy()
    {
        button1.selectOption -= GameManager.Instance.Dialogue.SelectOption1;
        button2.selectOption -= GameManager.Instance.Dialogue.SelectOption2;
    }
}

public class PlayerDialogOption
{
    public string button1Text;
    public string button2Text;
}
