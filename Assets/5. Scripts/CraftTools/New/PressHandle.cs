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
            CursorManager.onActive?.Invoke(true);
            CursorManager.onActiveComplate.AddListener(() => press.GrabHandle());
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