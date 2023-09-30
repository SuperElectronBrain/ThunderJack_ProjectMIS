using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour, IInteraction
{
    public bool IsUsed { get; set; }

    public void Interaction(GameObject user)
    {
        if (user.TryGetComponent(out NPC npc))
        {
            //user.SetActive(false);
            npc.ChangeState(NPCBehaviour.Rest);            
        }        
    }
}
