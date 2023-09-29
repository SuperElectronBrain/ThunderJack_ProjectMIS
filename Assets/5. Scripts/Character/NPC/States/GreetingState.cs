using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreetingState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        elapsedTime = 0;
        entity.isMeet = true;
        entity.SkAni.AnimationName = "A_hi_F";
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
