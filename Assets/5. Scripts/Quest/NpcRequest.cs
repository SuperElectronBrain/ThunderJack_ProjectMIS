using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRequest : MonoBehaviour
{
    [SerializeField] protected NpcRequestData npcRequestData;

    public void SetRequestData(NpcRequestData newNpcRequestData)
    {
        npcRequestData = newNpcRequestData;
        InitNpcRequest();
        ShowNpcRequest();
    }

    protected virtual void InitNpcRequest() { }

    protected virtual void ShowNpcRequest() { }
}
