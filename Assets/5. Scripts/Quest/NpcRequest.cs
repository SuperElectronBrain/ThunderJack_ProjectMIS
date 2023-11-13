using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRequest : MonoBehaviour
{
    [SerializeField] private Transform uiTransform;
    [SerializeField] private NpcRequestData npcRequestData;

    public void SetRequestData(NpcRequestData newNpcRequestData)
    {
        npcRequestData = newNpcRequestData;
        ShowNpcRequest();
    }

    protected virtual void ShowNpcRequest()
    {
        
    }
}

public class NpcRequest_1 : NpcRequest
{
    protected override void ShowNpcRequest()
    {
        var charDB = GameManager.Instance.CharacterDB;
        var uiTransform = GameManager.Instance.NpcRequestManager.requestUiParent;
        var npcCount = charDB.GetCharacterCount();
        
        for (int i = 1; i < npcCount; i++)
        {
            
        }
    }
}
