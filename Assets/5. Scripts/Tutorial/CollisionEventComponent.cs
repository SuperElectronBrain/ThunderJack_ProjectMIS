using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEventComponent : MonoBehaviour
{
	public UnityEvent m_OnCollisionEnter = new UnityEvent();
	public UnityEvent m_OnCollisionExit = new UnityEvent();

	public UnityEvent m_OnTriggerEnter = new UnityEvent();
	public UnityEvent m_OnTriggerExit = new UnityEvent();

	private void OnCollisionEnter(Collision collision)
	{
		bool bCollision = false;
		if (collision.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			bCollision = true;
		}

		if(bCollision == true)
		{
			m_OnCollisionEnter.Invoke();
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		bool bCollision = false;
		if (collision.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			bCollision = true;
		}

		if (bCollision == true)
		{
			m_OnCollisionExit.Invoke();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		bool bTrigger = false;
		if (other.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			bTrigger = true;
		}

		if (bTrigger == true)
		{
			m_OnTriggerEnter.Invoke();
		}
	}
	private void OnTriggerExit(Collider other)
	{
		bool bTrigger = false;
		if (other.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			bTrigger = true;
		}

		if (bTrigger == true)
		{
			m_OnTriggerExit.Invoke();
		}
	}
}
