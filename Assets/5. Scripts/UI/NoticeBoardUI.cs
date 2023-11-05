using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoardUI : UiComponent
{
    public override void InactiveUI()
    {
        base.InactiveUI();
        
        if(sequence != null)
            sequence.PlayBackwards();
        CameraEvent.Instance.ChangeCamera(CamType.Prev);
        EventManager.Publish(EventType.EndIteraction);
    }
}
