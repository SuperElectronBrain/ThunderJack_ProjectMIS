using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : MonoBehaviour
{
	public List<Collision> m_Collisions = new List<Collision>();
	public List<Collider> m_Colliders = new List<Collider>();

	private void OnCollisionEnter(Collision collision)
	{
		int count = 0;
		for(int i = 0; i < m_Collisions.Count; i = i + 1)
		{
			if (m_Collisions[i] == collision)
			{
				count = count + 1;
				break;
			}
		}

		if(count < 1) { m_Collisions.Add(collision); }
	}
	private void OnCollisionExit(Collision collision) 
	{
		for (int i = 0; i < m_Collisions.Count; i = i + 1)
		{
			if (m_Collisions[i] == collision)
			{
				m_Collisions.RemoveAt(i);
				break;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		int count = 0;
		for (int i = 0; i < m_Colliders.Count; i = i + 1)
		{
			if (m_Colliders[i] == other)
			{
				count = count + 1;
				break;
			}
		}

		if (count < 1) { m_Colliders.Add(other); }
	}
	private void OnTriggerExit(Collider other)
	{
		for (int i = 0; i < m_Colliders.Count; i = i + 1)
		{
			if (m_Colliders[i] == other)
			{
				m_Colliders.RemoveAt(i);
				break;
			}
		}
	}
}
