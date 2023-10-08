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
    UnityEngine.UI.Text tempText;

    public void InitDialogBox(string name)
    {
        cName.text = name;
    }

    public void SetScript(string script)
    {
        this.script.text = script;

        tempText.DOKill();
        tempText.text = null;
        tempText.DOText(script, 1f).OnUpdate(() =>
        {
            this.script.text = tempText.text;
        });
    }
}
