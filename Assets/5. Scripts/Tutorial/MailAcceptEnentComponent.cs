using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MailAcceptEnentComponent : MonoBehaviour
{
	public UnityEvent m_OnAcceptButtonClick = new UnityEvent();
	public UnityEvent m_OnUnacceptButtonClick = new UnityEvent();

	public void RegistButtonEvent()
    {
		GameObject mailUI = UniFunc.GetChildOfName(PlayerCharacterUIScript.main.gameObject, "MailUI");
		if (mailUI != null)
		{
			GameObject acceptButton = UniFunc.GetChildOfName(mailUI, "AcceptButton");
			if (acceptButton != null)
			{
				UnityEngine.UI.Button button = acceptButton.GetComponent<UnityEngine.UI.Button>();
				if (button != null)
				{
					button.onClick.AddListener(() => { m_OnAcceptButtonClick.Invoke(); });
				}
			}

			GameObject unacceptButton = UniFunc.GetChildOfName(mailUI, "UnacceptButton");
			if (unacceptButton != null)
			{
				UnityEngine.UI.Button button = unacceptButton.GetComponent<UnityEngine.UI.Button>();
				if (button != null)
				{
					button.onClick.AddListener(() => { m_OnUnacceptButtonClick.Invoke(); });
				}
			}
		}
	}
}
