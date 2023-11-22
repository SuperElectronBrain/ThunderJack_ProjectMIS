using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UiComponent : MonoBehaviour
{
    protected UI_Sequence sequence;

    private void Start()
    {
        sequence = GetComponent<UI_Sequence>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InactiveUI();
        }
    }

    public virtual void ActiveUI()
    {
        GameManager.Instance.UIManager.AddUI(this);
        gameObject.SetActive(true);
        ActiveSound();
    }

    public virtual void InactiveUI()
    {
        InactiveSound();
        gameObject.SetActive(false);        
    }

    protected virtual void ActiveSound() { }

    protected virtual void InactiveSound() { }
}