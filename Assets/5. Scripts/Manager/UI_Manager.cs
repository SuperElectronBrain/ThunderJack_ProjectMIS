using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public Stack<UiComponent> uiStack = new();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(uiStack.Count == 0)
            {
                // ¿É¼ÇÃ¢
            }
            else
            {
                uiStack.Pop().InactiveUI();
            }
        }
    }

    public void AddUI(UiComponent newUI)
    {
        uiStack.Push(newUI);
    }
}
