using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteraction
{
    public bool IsUsed { get; set; }

    public UnityEvent interactionEvent;

    public void Interaction(GameObject user)
    {
        Debug.Log("sdfasdf");
        interactionEvent?.Invoke();
    }
}
