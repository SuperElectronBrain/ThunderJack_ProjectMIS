using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class MonologueUI
{
	public GameObject m_MonologueGO;
	public UnityEngine.UI.Image m_PortraitImage;
	public TextMeshProUGUI m_MonologueText;


	//public static implicit operator MonologueUI(string p_String) { return new MonologueUI(); }
	//public static implicit operator string(MonologueUI p_MonologueUI)
	//{
	//	string t_String = null;
	//	if (p_MonologueUI.m_MonologueGO != null) { t_String = p_MonologueUI.ToString(); }
	//	return t_String;
	//}

	public static implicit operator MonologueUI(GameObject p_GO)
	{
		MonologueUI t_MonologueUI = new MonologueUI();//null;
		if (p_GO != null)
		{
			t_MonologueUI = new MonologueUI();
			t_MonologueUI.m_MonologueGO = p_GO;
		}
		return t_MonologueUI;
	}
	public static implicit operator GameObject(MonologueUI p_MonologueUI)
	{
		GameObject t_GO = null;
		if (p_MonologueUI.m_MonologueGO != null) { t_GO = p_MonologueUI.m_MonologueGO; }
		return t_GO;
	}
}

public class PlayerCharacterUIScript : MonoBehaviour
{
	public ClockUIScript m_ClockUIScript;
	public TextMeshProUGUI m_MoneyText;
	public TextMeshProUGUI m_HonerText;
	public InventoryUIScript m_InventoryUIScript;
	public NPCStoreUIScript m_NPCStoreUIScript;
	public RecipeBookUIScript m_RecipeBookUIScript;
	public QuestListUIScript m_QuestListUIScript;
	public MailBoxUIScript m_MailBoxUIScript;
	public UnityEngine.UI.Image m_MouseGrabIcon;
	public MonologueUI m_MonologueUI = new MonologueUI();

	// Start is called before the first frame update
	void Start()
	{
		ReFindUI();
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	public void ReFindUI()
	{
		if (m_ClockUIScript == null) { m_ClockUIScript = UniFunc.GetChildComponent<ClockUIScript>(transform); }
		if (m_MoneyText == null) { m_MoneyText = UniFunc.GetChildOfName(transform, "MoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_HonerText == null) { m_HonerText = UniFunc.GetChildOfName(transform, "HonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_InventoryUIScript == null) { m_InventoryUIScript = UniFunc.GetChildComponent<InventoryUIScript>(transform); }
		if (m_NPCStoreUIScript == null) { m_NPCStoreUIScript = UniFunc.GetChildComponent<NPCStoreUIScript>(transform); }
		if (m_RecipeBookUIScript == null) { m_RecipeBookUIScript = UniFunc.GetChildComponent<RecipeBookUIScript>(transform); }
		if (m_QuestListUIScript == null) { m_QuestListUIScript = UniFunc.GetChildComponent<QuestListUIScript>(transform); }
		if (m_MailBoxUIScript == null) { m_MailBoxUIScript = UniFunc.GetChildComponent<MailBoxUIScript>(transform); ; }
		if (m_MouseGrabIcon == null) { m_MouseGrabIcon = UniFunc.GetChildOfName(transform, "MouseGrabItem").GetComponent<UnityEngine.UI.Image>(); }
		if (m_MonologueUI.m_MonologueGO == null)
		{
			m_MonologueUI.m_MonologueGO = UniFunc.GetChildOfName(transform, "MonologueUI").GetComponent<GameObject>();
			if(m_MonologueUI.m_MonologueGO != null)
			{
				m_MonologueUI.m_PortraitImage = UniFunc.GetChildComponent<UnityEngine.UI.Image>(m_MonologueUI.m_MonologueGO);
				m_MonologueUI.m_MonologueText = UniFunc.GetChildComponent<TextMeshProUGUI>(m_MonologueUI.m_MonologueGO);
			}
		}
	}
}
