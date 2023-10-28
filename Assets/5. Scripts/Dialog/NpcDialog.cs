using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcDialog : MonoBehaviour
{
    [SerializeField]
    TextMeshPro script;
    [SerializeField]
    TextMeshPro cName;

    private void OnEnable()
    {
        transform.localRotation = Camera.main.transform.rotation;
    }

    public void InitDialogBox(string name)
    {
        cName.text = name;
    }

    public void SetScript(string script)
    {
        this.script.text = script;
    }
}
