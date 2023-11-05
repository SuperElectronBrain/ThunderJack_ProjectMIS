using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteraction
{
    public bool IsUsed { get; set; }

    public UnityEvent interactionEvent;

    [SerializeField] private AudioSource m_InteractionSound;

    public void Interaction(GameObject user)
    {
        Debug.Log("sdfasdf");
        interactionEvent?.Invoke();
        if(m_InteractionSound != null) { if (m_InteractionSound.isPlaying != false)  m_InteractionSound.Play(); } 
    }
}
