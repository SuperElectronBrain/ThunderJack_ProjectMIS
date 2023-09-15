using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State<NPC>
{
    public Vector3 vel;
    public override void Enter(NPC entity)
    {
        entity.agent.isStopped = false;
        entity.agent.SetDestination(entity.destinationPos);
    }

    public override void Execute(NPC entity)
    {
        entity.lookDir.SetDir(entity.agent.velocity);
        vel = entity.agent.velocity;

        var scaleX = entity.lookDir.isRight ? -1 : 1;
        var newScale = entity.myTransform.localScale;
        newScale.x = Mathf.Abs(entity.myTransform.localScale.x) * scaleX;

        entity.myTransform.localScale = newScale;
        
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
        //entity.agent.ResetPath();
    }

    public override void OnTransition(NPC entity)
    {
/*        if (entity.agent.remainingDistance <= 0.1f)
        {
            entity.ChangeState(NPCBehaviour.Idle);
        }            */
    }
}
