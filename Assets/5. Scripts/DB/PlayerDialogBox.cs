using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
    [SerializeField]
    Text text;

    private void OnEnable()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        button1.selectOption += GameManager.Instance.Dialogue.SelectOption1;
        button2.selectOption += GameManager.Instance.Dialogue.SelectOption2;

        dialogName.text = transform.root.gameObject.name;
        gameObject.SetActive(false);
    }

    public void SetPlayerDialog(string script)
    {
        text.DOKill();
        text.text = null;
        text.DOText(script, 1f).OnUpdate(()=>
        {
            dialogScript.text = text.text;
        });
    }

    public void SetPlayerDialogOption(string option1, string option2)
    {
        button1.SetOptionText(option1);
        button2.SetOptionText(option2);
        button1.transform.parent.gameObject.SetActive(true);
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