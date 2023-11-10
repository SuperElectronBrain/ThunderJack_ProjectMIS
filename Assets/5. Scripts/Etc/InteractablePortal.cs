using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractablePortal : Portal, IInteraction
{
	[SerializeField] private AudioSource m_InteractionSound;

	public bool IsUsed { get; set; }

	public void Interaction(GameObject user)
	{
		if (m_InteractionSound != null) { if (m_InteractionSound.isPlaying == false) m_InteractionSound.Play(); }
		LoadingScene();
	}
}
