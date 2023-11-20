using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo : MonoBehaviour
{
    [SerializeField] private GameObject highlightObj;

    [SerializeField] private MemoUI memoUI;
    
    private void OnMouseDown()
    {
        memoUI.ActiveUI();
    }

    private void OnMouseEnter()
    {
        highlightObj.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlightObj.SetActive(false);
    }

    public void UpdateOrderSheet(string order)
    {
        memoUI.SetMemo(order);
    }
}
