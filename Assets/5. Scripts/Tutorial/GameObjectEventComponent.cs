using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectEventComponent : MonoBehaviour
{
	public UnityEvent m_OnStart = new UnityEvent();
	public UnityEvent m_OnDestroy = new UnityEvent();
	public UnityEvent m_OnEnable = new UnityEvent();
	public UnityEvent m_OnDisable = new UnityEvent();

	private void OnEnable()
	{
		m_OnEnable.Invoke();
	}

	// Start is called before the first frame update
	void Start()
	{
		m_OnStart.Invoke();
	}

	private void OnDisable()
	{
		m_OnDisable.Invoke();
	}

	private void OnDestroy()
	{
		m_OnDestroy.Invoke();
	}
}
