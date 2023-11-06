using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoardUI : UiComponent
{
	public void Update()
	{
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            Stack<UiComponent> uiStack = GameManager.Instance.UIManager.uiStack;

			if (uiStack != null)
            {
                UiComponent uiComponent = uiStack.Pop();

				if (uiComponent != this)
                {  uiStack.Push(uiComponent); }
                else if (uiComponent == this)
                { uiComponent.InactiveUI(); }
			}
        }
	}

	public override void InactiveUI()
    {
        base.InactiveUI();
        
        if(sequence != null)
            sequence.PlayBackwards();
        CameraEvent.Instance.ChangeCamera(CamType.Prev);
        EventManager.Publish(EventType.EndIteraction);
    }
}
