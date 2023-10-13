using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : State<NPC>
{
    [SerializeField]
    Vector3 originPos;
    public override void Enter(NPC entity)
    {
        originPos = entity.transform.position;
        entity.agent.isStopped = true;
        entity.agent.enabled = false;
        entity.gameObject.transform.position = new Vector3(0, 1000, 0);
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        transform.position = originPos;
        entity.agent.enabled = true;
        entity.agent.isStopped = false;
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}
