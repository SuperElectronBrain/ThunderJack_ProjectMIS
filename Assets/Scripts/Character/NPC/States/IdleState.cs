using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "FRONT";
        else
            entity.SkAni.AnimationName = "BACK";
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}