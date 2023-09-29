using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour, IInteraction
{
    Transform seatTransform;
    [SerializeField]
    GameObject g;

    public bool IsUsed { get; set; }

    public void Start()
    {
        seatTransform = transform.GetChild(0);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            g.transform.SetParent(seatTransform, true);
            g.transform.localPosition = Vector3.zero;
            g.GetComponent<NPC>().ChangeState(NPCBehaviour.Sitting);
        }
    }

    public void Interaction(GameObject go)
    {

    }

    public void Interaction()
    {
        
    }
}
