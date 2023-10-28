using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour, IInteraction, INpcOnly
{
    Transform seatTransform;

    public bool IsUsed { get; set; }

    public void Start()
    {
        seatTransform = transform.GetChild(0);
    }

    public void Interaction(GameObject go)
    {
        go.transform.SetParent(seatTransform, true);
        go.transform.localPosition = Vector3.zero;
        if (go.TryGetComponent(out NPC npc))
        {
            npc.ChangeState(NPCBehaviour.Sitting);
        }
    }
}
