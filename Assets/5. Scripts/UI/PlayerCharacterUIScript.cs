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

[Serializable]
public class ItemInfoDisplay
{
	public GameObject m_ItemInfoDisplayGO;
	public RectTransform m_ItemInfoDisplayRect;
	public TextMeshProUGUI m_ItemNameText;
	public TextMeshProUGUI m_ItemInfoText;

	public static implicit operator ItemInfoDisplay(GameObject p_GO)
	{
		ItemInfoDisplay t_ItemInfoDisplay = new ItemInfoDisplay();//null;
		if (p_GO != null)
		{
			t_ItemInfoDisplay = new ItemInfoDisplay();
			t_ItemInfoDisplay.m_ItemInfoDisplayGO = p_GO;
		}
		return t_ItemInfoDisplay;
	}
	public static implicit operator GameObject(ItemInfoDisplay p_ItemInfoDisplay)
	{
		GameObject t_GO = null;
		if (p_ItemInfoDisplay.m_ItemInfoDisplayGO != null) { t_GO = p_ItemInfoDisplay.m_ItemInfoDisplayGO; }
		return t_GO;
	}
}

[Serializable]
public class InteractionIcon
{
	public RectTransform m_InteractionIconRect;
	public UnityEngine.UI.Image m_InteractionImage;
	public TextMeshProUGUI m_InteractionText;

	public Sprite m_Talk;
	public Sprite m_Plants;
	public Sprite m_Open;
	public Sprite m_Mail;
	public Sprite m_Get;
	public Sprite m_Fish;
}


public class PlayerCharacterUIScript : MonoBehaviour
{
	public ClockUIScript m_ClockUIScript;
	public CurrencyUIScript m_CurrencyUIScript;
	//public TextMeshProUGUI m_MoneyText;
	//public TextMeshProUGUI m_HonerText;
	//private TextMeshProUGUI m_AddMoneyText;
	//private TextMeshProUGUI m_AddHonerText;
	public InventoryUIScript m_InventoryUIScript;
	public NPCStoreUIScript m_NPCStoreUIScript;
	public RecipeBookUIScript m_RecipeBookUIScript;
	public QuestListUIScript m_QuestListUIScript;
	public MailBoxUIScript m_MailBoxUIScript;
	public UnityEngine.UI.Image m_MouseGrabIcon;
	public MonologueUI m_MonologueUI = new MonologueUI();
	public ItemInfoDisplay m_ItemInfoDisplay = new ItemInfoDisplay();
	public InteractionIcon m_InteractionIcon = new InteractionIcon();
	//public UnityEngine.UI.Image m_InteractionIcon;
	public UnityEngine.UI.Image m_FadeUI;

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
		if (m_CurrencyUIScript == null) { m_CurrencyUIScript = UniFunc.GetChildComponent<CurrencyUIScript>(transform); }
		//if (m_MoneyText == null) { m_MoneyText = UniFunc.GetChildOfName(transform, "MoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		//if (m_HonerText == null) { m_HonerText = UniFunc.GetChildOfName(transform, "HonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
		//if (m_AddMoneyText == null) { m_AddMoneyText = UniFunc.GetChildOfName(transform, "AddMoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		//if (m_AddHonerText == null) { m_AddHonerText = UniFunc.GetChildOfName(transform, "AddHonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_InventoryUIScript == null) { m_InventoryUIScript = UniFunc.GetChildComponent<InventoryUIScript>(transform); }
		if (m_NPCStoreUIScript == null) { m_NPCStoreUIScript = UniFunc.GetChildComponent<NPCStoreUIScript>(transform); }
		if (m_RecipeBookUIScript == null) { m_RecipeBookUIScript = UniFunc.GetChildComponent<RecipeBookUIScript>(transform); }
		if (m_QuestListUIScript == null) { m_QuestListUIScript = UniFunc.GetChildComponent<QuestListUIScript>(transform); }
		if (m_MailBoxUIScript == null) { m_MailBoxUIScript = UniFunc.GetChildComponent<MailBoxUIScript>(transform); ; }
		if (m_MouseGrabIcon == null) { m_MouseGrabIcon = UniFunc.GetChildOfName(transform, "MouseGrabItem").GetComponent<UnityEngine.UI.Image>(); }
		if (m_MonologueUI == null) { m_MonologueUI = new MonologueUI(); }
		if (m_MonologueUI != null)
		{
			if (m_MonologueUI.m_MonologueGO == null)
			{
				m_MonologueUI.m_MonologueGO = UniFunc.GetChildOfName(transform, "MonologueUI").GetComponent<GameObject>();
			}
			if (m_MonologueUI.m_MonologueGO != null)
			{
				if(m_MonologueUI.m_PortraitImage == null)
				{
					m_MonologueUI.m_PortraitImage = UniFunc.GetChildComponent<UnityEngine.UI.Image>(m_MonologueUI.m_MonologueGO);
				}
				if (m_MonologueUI.m_MonologueText == null)
				{
					m_MonologueUI.m_MonologueText = UniFunc.GetChildComponent<TextMeshProUGUI>(m_MonologueUI.m_MonologueGO);
				}
			}
		}
		if (m_ItemInfoDisplay == null) { m_ItemInfoDisplay = new ItemInfoDisplay(); }
		if (m_ItemInfoDisplay != null)
		{
			if (m_ItemInfoDisplay.m_ItemInfoDisplayGO == null)
			{
				m_ItemInfoDisplay.m_ItemInfoDisplayGO = UniFunc.GetChildOfName(transform, "ItemInformationDisplay").GetComponent<GameObject>();
			}
			if (m_ItemInfoDisplay.m_ItemInfoDisplayGO != null)
			{
				if(m_ItemInfoDisplay.m_ItemInfoDisplayRect == null)
				{
					m_ItemInfoDisplay.m_ItemInfoDisplayRect = m_ItemInfoDisplay.m_ItemInfoDisplayGO.GetComponent<RectTransform>();
				}
				if (m_ItemInfoDisplay.m_ItemNameText == null)
				{ 
					m_ItemInfoDisplay.m_ItemNameText = UniFunc.GetChildOfName(m_ItemInfoDisplay.m_ItemInfoDisplayGO, "ItemNameText (TMP)").GetComponent<TextMeshProUGUI>();
				}
				if (m_ItemInfoDisplay.m_ItemInfoText == null)
				{ 
					m_ItemInfoDisplay.m_ItemInfoText = UniFunc.GetChildOfName(m_ItemInfoDisplay.m_ItemInfoDisplayGO, "ItemInformationText (TMP)").GetComponent<TextMeshProUGUI>();
				}
			}
		}
		if (m_InteractionIcon == null) { m_InteractionIcon = new InteractionIcon(); }
		if (m_InteractionIcon != null)
		{
			if (m_InteractionIcon.m_InteractionIconRect == null)
			{
				m_InteractionIcon.m_InteractionIconRect = UniFunc.GetChildOfName(transform, "InteractionIcon").GetComponent<RectTransform>();
			}
			if (m_InteractionIcon.m_InteractionIconRect != null)
			{
				if(m_InteractionIcon.m_InteractionImage == null)
				{
					m_InteractionIcon.m_InteractionImage = UniFunc.GetChildOfName(m_InteractionIcon.m_InteractionIconRect.gameObject, "InteractionImage").GetComponent<UnityEngine.UI.Image>();
				}
				if (m_InteractionIcon.m_InteractionText == null)
				{
					m_InteractionIcon.m_InteractionText = UniFunc.GetChildOfName(m_InteractionIcon.m_InteractionIconRect.gameObject, "InteractionText (TMP)").GetComponent<TextMeshProUGUI>();
				}
			}
		}
		if (m_FadeUI == null) { m_FadeUI = UniFunc.GetChildOfName(transform, "FadeUI").GetComponent<UnityEngine.UI.Image>(); }
	}

	//public void AddMoneyText(string param)
	//{
	//	if(m_AddMoneyText != null)
	//	{
	//		m_AddMoneyText.gameObject.SetActive(true);
	//		m_AddMoneyText.text = param;
	//		Invoke("MoneyTextDeactivate", 3);
	//	}
	//}
	//
	//private void MoneyTextDeactivate()
	//{
	//	if (m_AddMoneyText != null) { m_AddMoneyText.gameObject.SetActive(false); }
	//}
	//
	//public void AddHonerText(string param)
	//{
	//	if (m_AddHonerText != null)
	//	{
	//		m_AddHonerText.gameObject.SetActive(true);
	//		m_AddHonerText.text = param;
	//		Invoke("HonerTextDeactivate", 3);
	//	}
	//}
	//
	//private void HonerTextDeactivate()
	//{
	//	if (m_AddHonerText != null) { m_AddHonerText.gameObject.SetActive(false); }
	//}
}
