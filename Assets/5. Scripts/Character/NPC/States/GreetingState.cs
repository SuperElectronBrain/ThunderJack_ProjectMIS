using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreetingState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        elapsedTime = 0;
        entity.isMeet = true;

        entity.FindPlayer();
        entity.lookDir.SetDir(entity.myTransform, entity.player.transform.position);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        var newScale = entity.myTransform.localScale;
        newScale.x = Mathf.Abs(entity.myTransform.localScale.x) * scaleX;

        entity.myTransform.localScale = newScale;

        if (entity.lookDir.isFront)
        {
            entity.ChangeAni("A_hi_F");
        }
        else
        {
            entity.ChangeAni("A_hi_B");
        }
        
        entity.agent.isStopped = true;
    }

    public override void Execute(NPC entity)
    {
        elapsedTime += Time.deltaTime;
    }

    public override void Exit(NPC entity)
    {
        entity.agent.isStopped = true;
    }

    public override void OnTransition(NPC entity)
    {
        if(elapsedTime >= 1f)
        {
            entity.ChangeState(entity.PrevBehaviour);
        }
    }
}
