using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        entity.agent.SetDestination(entity.destinationPos);
    }

    public override void Execute(NPC entity)
    {
        entity.lookDir.SetDir(entity.agent.velocity);

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scaleX, transform.localScale.y, transform.localScale.z);
        
        if (entity.lookDir.isFront)
        {                
            if (entity.lookDir.isSideWalk)
                entity.SkAni.AnimationName = "FRONT_Walk2";
            else
                entity.SkAni.AnimationName = "FRONT_Walk1";
        }
        else
        {
            if (entity.lookDir.isSideWalk)
                entity.SkAni.AnimationName = "BACK_Walk2";
            else
                entity.SkAni.AnimationName = "BACK_Walk1";
        }                
    }

    public override void Exit(NPC entity)
    {
        Debug.Log("이동완료");
        entity.agent.ResetPath();
    }

    public override void OnTransition(NPC entity)
    {
        if (entity.agent.remainingDistance <= 0.1f)
        {
            entity.ChangeState(NPCBehaviour.Idle);
        }            
    }
}
