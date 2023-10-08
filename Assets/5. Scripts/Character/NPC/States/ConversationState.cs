using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationState : State<NPC>
{
    bool isTalking;
    bool isTwinkling;

    public override void Enter(NPC entity)
    {
        GameManager.Instance.GameTime.TimeStop(true);
        CameraEvent.Instance.SetTarget(transform);
        CameraEvent.Instance.ChangeCamera(CamType.Conversation);
        isTalking = true;
        isTwinkling = true;
        entity.isTalk = true;
        entity.agent.isStopped = true;
        entity.lookDir.SetDir(transform.position, entity.curInteractionObj.transform.position);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        var newScale = entity.myTransform.localScale;
        newScale.x = Mathf.Abs(entity.myTransform.localScale.x) * scaleX;

        entity.myTransform.localScale = newScale;

        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "A_talk1_F";
        else
            entity.SkAni.AnimationName = "A_talk1_B";

        StartCoroutine(CTwinkling(entity, 1.5f));        

        //entity.StartConversation();
        EventManager.Subscribe(EventType.EndConversation, EndConversation);
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        GameManager.Instance.GameTime.TimeStop(false);
        CameraEvent.Instance.SetTarget(transform);
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
        Debug.Log("변경 전 " + entity.SkAni.AnimationName);
        yield return new WaitForSeconds(delay);

        if (isTwinkling)
            entity.SkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("1", "2"), true, 0);
        else
            entity.SkAni.AnimationState.AddAnimation(0, entity.SkAni.AnimationName.Replace("2", "1"), true, 0);

        Debug.Log("변경 후 " + entity.SkAni.AnimationName);

        isTwinkling = !isTwinkling;

        StartCoroutine(CTwinkling(entity, delay));
    }
}

