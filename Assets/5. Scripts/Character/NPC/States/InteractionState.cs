using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        entity.targetInteractionObj.GetComponent<IInteraction>().Interaction(gameObject);
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
