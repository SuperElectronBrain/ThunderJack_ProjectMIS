using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationState : State<NPC>
{
    bool isTalking;
    public override void Enter(NPC entity)
    {
        entity.lookDir.SetDir(transform.position, entity.curInteractionObj.transform.position);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scaleX, transform.localScale.y, transform.localScale.z);

        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "FRONT";
        else
            entity.SkAni.AnimationName = "BACK";

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
            entity.ChangeState(NPCBehaviour.Idle);
    }

    void EndConversation()
    {
        isTalking = false;
    }
}

