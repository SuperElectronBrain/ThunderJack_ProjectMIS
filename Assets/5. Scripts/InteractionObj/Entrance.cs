using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour, IInteraction, INpcOnly
{
    public bool IsUsed { get; set; }

    public void Interaction(GameObject user)
    {
        if (user.TryGetComponent(out NPC npc))
        {
            if(npc.Schedule.aiParam1 == 2)
            {
                npc.ChangeState(NPCBehaviour.Business);
            }
            else if(npc.Schedule.aiParam1 == 3)
            {
                npc.ChangeState(NPCBehaviour.Rest);
            }            
        }        
    }
}
