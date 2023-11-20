using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayEventComponent : MonoBehaviour
{
	public float delay = 0.0f;
	public UnityEvent m_OnEnable = new UnityEvent();
	private void OnEnable()
	{
		StartCoroutine(Coroutine(() => { m_OnEnable.Invoke(); }, delay));
	}

	private IEnumerator Coroutine(UnityEngine.Events.UnityAction pAction, float time)
	{
		yield return new WaitForSeconds(time);
		pAction.Invoke();
	}
}
