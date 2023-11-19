using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationState : State<NPC>
{
    bool isTalking;
    bool isTwinkling;

    public override void Enter(NPC entity)
    {
        EventManager.Publish(EventType.StartInteraction);
        EventManager.Publish(EventType.StartConversation);
        EventManager.Subscribe(EventType.EndConversation, EndConversation);
        CameraEvent.Instance.ChangeCamera(CamType.Conversation);
        if(entity.prevBehaviour != NPCBehaviour.Business)
            entity.isAcquaintance = true;
        
        isTalking = true;
        isTalking = true;
        entity.isTalk = true;
        entity.agent.isStopped = true;
        entity.lookDir.SetDir(entity.myTransform, entity.curInteractionObj.transform.position);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        var newScale = entity.myTransform.localScale;
        newScale.x = Mathf.Abs(entity.myTransform.localScale.x) * scaleX;

        entity.myTransform.localScale = newScale;

        if (entity.lookDir.isFront)
            entity.ChangeAni("A_talk1_F");
        else
            entity.ChangeAni("A_talk1_B");

        StartCoroutine(CTwinkling(entity, 1.5f));        

        //entity.StartConversation();
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        EventManager.Publish(EventType.EndIteraction);
        EventManager.Unsubscribe(EventType.EndConversation, EndConversation);
        StopAllCoroutines();
        entity.isTalk = false;
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

    IEnumerator CTwinkling(NPC entity, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isTwinkling)
        {
            entity.SkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("1", "2"), true, 0);
            entity.ShadowSkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("1", "2"), true, 0);
        }
        else
        {
            entity.SkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("2", "1"), true, 0);
            entity.ShadowSkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("2", "1"), true, 0);
        }
            

        isTwinkling = !isTwinkling;

        StartCoroutine(CTwinkling(entity, delay));
    }
}

