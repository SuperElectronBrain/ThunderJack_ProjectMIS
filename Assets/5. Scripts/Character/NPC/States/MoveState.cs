using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State<NPC>
{
    public Vector3 vel;
    [SerializeField]
    bool isMove;

    public override void Enter(NPC entity)
    {
        entity.agent.isStopped = false;
        entity.agent.SetDestination(entity.destinationPos);
        StartCoroutine(MoveCheck(entity));
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
                entity.SkAni.AnimationName = "A_walk_F";
            else
                entity.SkAni.AnimationName = "A_walk_F";
        }
        else
        {
            if (entity.lookDir.isSideWalk)
                entity.SkAni.AnimationName = "A_walk_B";
            else
                entity.SkAni.AnimationName = "A_walk_B";
        }                
    }

    public override void Exit(NPC entity)
    {
        Debug.Log(gameObject.name + "이동완료");        
    }

    public override void OnTransition(NPC entity)
    {
        if (entity.IsInSight())
        {
            entity.ChangeState(NPCBehaviour.Greeting);
        }
        if(isMove)
        {
            if (entity.agent.remainingDistance <= 0.5f)
            {
                if (!entity.agent.hasPath || entity.agent.velocity.sqrMagnitude == 0f)
                {
                    entity.ChangeState(NPCBehaviour.Idle);
                }
            }
        }             
    }

    IEnumerator MoveCheck(NPC entity)
    {
        isMove = false;

        while (true)
        {
            if(entity.agent.velocity.sqrMagnitude != 0)
            {
                isMove = true;
                yield break;
            }
            yield return null;
        }
    }
}
