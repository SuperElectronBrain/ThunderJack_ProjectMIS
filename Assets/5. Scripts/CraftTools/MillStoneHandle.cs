using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStoneHandle : MonoBehaviour
{
    [SerializeField]
    private RavenCraftCore.MillStone millStone;

    private void OnMouseDown()
    {
        CursorManager.SetCursorPosition(transform.position);
        CursorManager.onActiveComplate.AddListener(() => millStone.GrabHandle(true));
        CursorManager.onActive?.Invoke(true);
    }

    private void OnMouseEnter()
    {
        millStone.EnterHandle();
    }

    private void OnMouseExit()
    {
        millStone.ExitHandle();
    }
}
