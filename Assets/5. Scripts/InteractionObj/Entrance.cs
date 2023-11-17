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
                switch (npc.CharacterData.characterType)
                {
                    case CharacterType.Merchant:
                        npc.ChangeState(NPCBehaviour.Business);
                        break;
                    case CharacterType.PartTimer:
                        npc.ChangeState(NPCBehaviour.PartTimer);
                        break;
                }
            }
            else if(npc.Schedule.aiParam1 == 3)
            {
                npc.ChangeState(NPCBehaviour.Rest);
            }            
        }        
    }
}
