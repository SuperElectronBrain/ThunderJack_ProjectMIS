using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        entity.agent.enabled = false;
        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "A_sit1_F";
        else
            entity.SkAni.AnimationName = "A_sit1_B";
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        entity.agent.enabled = true;
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}
