using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailBoxUIScript : MonoBehaviour
{
	public QuestComponet m_QuestComponet;
	public TextMeshProUGUI m_Text;
	[HideInInspector] public UnityEvent<bool> m_OnButtonClick = new UnityEvent<bool>();

	// Start is called before the first frame update
	void Start()
	{
		GameObject t_GO =  UniFunc.GetChildOfName(transform, "AcceptButton");
		if(t_GO != null)
		{
			Button t_AcceptButton = t_GO.GetComponent<Button>();
			if(t_AcceptButton != null)
			{
				t_AcceptButton.onClick.AddListener(() => { OnButtonClick(true); });
			}
		}

		t_GO = UniFunc.GetChildOfName(transform, "UnacceptButton");
		if (t_GO != null)
		{
			Button t_UnacceptButton = t_GO.GetComponent<Button>();
			if (t_UnacceptButton != null)
			{
				t_UnacceptButton.onClick.AddListener(() => { OnButtonClick(false); });
			}
		}
	}

	void OnButtonClick(bool param)
	{ m_OnButtonClick.Invoke(param); }

	public void DisplayMail(bool p_Bool, string p_Script)
	{
		gameObject.SetActive(p_Bool);
		if (p_Bool == true)
		{
			if (m_Text != null)
			{
				m_Text.text = p_Script;
			}
		}
	}
}
