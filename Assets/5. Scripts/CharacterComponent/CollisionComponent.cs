using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionComponent : MonoBehaviour
{
	[HideInInspector] public List<Collision> m_Collisions = new List<Collision>();
	[HideInInspector] public List<Collider> m_Colliders = new List<Collider>();

	public UnityEvent m_OnCollisionEnter = new UnityEvent();
	public UnityEvent m_OnCollisionExit = new UnityEvent();
	[HideInInspector] public UnityEvent<Collider> m_OnCollisionEnterUseParam = new UnityEvent<Collider>();
	[HideInInspector] public UnityEvent<Collider> m_OnCollisionExitUseParam = new UnityEvent<Collider>();

	private void OnCollisionEnter(Collision collision)
	{
		int count = 0;
		for(int i = 0; i < m_Collisions.Count; i = i + 1)
		{
			if (m_Collisions[i] == collision) { count = count + 1; break; }
		}
		if(count < 1) { m_Collisions.Add(collision); }
		m_OnCollisionEnter.Invoke();
		m_OnCollisionEnterUseParam.Invoke(collision.collider);
	}
	private void OnCollisionExit(Collision collision) 
	{
		for (int i = 0; i < m_Collisions.Count; i = i + 1)
		{
			if (m_Collisions[i] == collision)
			{
				m_Collisions.RemoveAt(i);
				m_Collisions.TrimExcess();
				break;
			}
		}
		m_OnCollisionExit.Invoke();
		m_OnCollisionExitUseParam.Invoke(collision.collider);
	}

	private void OnTriggerEnter(Collider other)
	{
		int count = 0;
		for (int i = 0; i < m_Colliders.Count; i = i + 1)
		{
			if (m_Colliders[i] == other) { count = count + 1; break; }
		}
		if (count < 1) { m_Colliders.Add(other); }
		m_OnCollisionEnter.Invoke();
		m_OnCollisionEnterUseParam.Invoke(other);
	}
	private void OnTriggerExit(Collider other)
	{
		for (int i = 0; i < m_Colliders.Count; i = i + 1)
		{
			if (m_Colliders[i] == other)
			{
				m_Colliders.RemoveAt(i);
				m_Colliders.TrimExcess();
				break;
			}
		}
		m_OnCollisionExit.Invoke();
		m_OnCollisionExitUseParam.Invoke(other);
	}
}