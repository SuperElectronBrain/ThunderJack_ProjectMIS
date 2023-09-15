using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationState : State<NPC>
{
    bool isTalking;
    public override void Enter(NPC entity)
    {
        CameraEvent.Instance.ChangeCamera(CamType.Conversation);
        isTalking = true;
        entity.agent.isStopped = true;
        entity.lookDir.SetDir(transform.position, entity.curInteractionObj.transform.position);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        var newScale = entity.myTransform.localScale;
        newScale.x = Mathf.Abs(entity.myTransform.localScale.x) * scaleX;

        entity.myTransform.localScale = newScale;

        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "FRONT";
        else
            entity.SkAni.AnimationName = "BACK";

        entity.StartConversation();
        EventManager.Subscribe(EventType.EndConversation, EndConversation);
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        EventManager.Unsubscribe(EventType.EndConversation, EndConversation);
    }

    public override void OnTransition(NPC entity)
    {
        if (!isTalking)
            entity.ChangeState(entity.PrevBehaviour);
    }

    void EndConversation()
    {
        CameraEvent.Instance.ChangeCamera(CamType.Prev);
        isTalking = false;
    }
}

