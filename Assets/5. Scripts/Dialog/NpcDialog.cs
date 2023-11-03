using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NpcDialog : MonoBehaviour
{
    [SerializeField]
    TextMeshPro script;
    [SerializeField]
    TextMeshPro cName;
    [SerializeField]
    UnityEngine.UI.Text text;

    private void OnEnable()
    {
        transform.localRotation = Camera.main.transform.rotation;

        Debug.Log("aaaa");
        
        /*var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();*/
    }

    public void InitDialogBox(string name)
    {
        cName.text = name;
    }

    public void SetScript(string script)
    {
        this.text.text = script;

        text.DOKill();
        text.text = null;
        text.DOText(script, 1f).OnUpdate(() =>
        {
            this.script.text = text.text;
        });
    }
}
