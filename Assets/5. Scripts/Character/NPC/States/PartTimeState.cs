using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTimeState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        entity.agent.isStopped = true;
        entity.agent.enabled = false;
        entity.gameObject.transform.position = entity.destinationPos;
        entity.isSales = true;
        entity.curBehaviour = NPCBehaviour.PartTimer;

        entity.SkAni.AnimationName = "A_work1_F";

        switch(entity.CharacterData.characterEgName)
        {
            case "Harry":
                entity.shopDialog = "Furniture_Shop_Text_Master";
                break;
            case "Dokan":
                entity.shopDialog = "Material_Shop_Text_Master";
                break;
        }
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        entity.agent.enabled = true;
        entity.agent.isStopped = false;
        entity.prevBehaviour = NPCBehaviour.PartTimer;
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}