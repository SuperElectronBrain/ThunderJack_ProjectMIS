using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMonologueDisplayer : MonoBehaviour
{
	[SerializeField] private string script = "";
	[SerializeField] private float displayTime = 1.0f;

	public void PopupMonologue()
	{
		WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble(script, true); }, 0);
		WaitFewSeconds(() => { PlayerCharacter.main.PopUpSpeechBubble("", false); }, displayTime); 
	}

	private void WaitFewSeconds(UnityEngine.Events.UnityAction pAction, float time)
	{
		StartCoroutine(Coroutine(pAction, time));
	}

	private IEnumerator Coroutine(UnityEngine.Events.UnityAction pAction, float time)
	{
		yield return new WaitForSeconds(time);
		pAction.Invoke();
	}
}
