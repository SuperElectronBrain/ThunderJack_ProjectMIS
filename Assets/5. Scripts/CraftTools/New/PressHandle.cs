using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    public class PressHandle : MonoBehaviour
    {
        [SerializeField]
        private Press press;

        private void OnMouseDown()
        {
            CursorManager.SetCursorPosition(transform.position);
            CursorManager.onActiveComplate.AddListener(() => press.GrabHandle());
            CursorManager.onActive?.Invoke(true);
        }

        private void OnMouseEnter()
        {
            press.EnterHandle();
        }

        private void OnMouseExit()
        {
            press.ExitHandle();
        }
    }
}