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
        
        
    }

    public override void Exit(NPC entity)
    {
        Debug.Log("이동완료");
    }

    public override void OnTransition(NPC entity)
    {
        if (entity.agent.isStopped)
        {
            entity.ChangeState(NPCBehaviour.Idle);
        }            
    }
}
