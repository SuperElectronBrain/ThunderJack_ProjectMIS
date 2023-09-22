using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
	public GameObject m_ButtonsPrefab;
	public GameObject m_ButtonsParent;
	public List<Button> m_Buttons;
	public Inventory m_Inventory;

	// Start is called before the first frame update
	void Start()
	{
		ReFindButton();
	}

	public void ReFindButton()
	{
		if (m_ButtonsParent != null)
		{
			m_Buttons.Clear();
			m_Buttons.TrimExcess();
			for (int i = 0; i < m_ButtonsParent.transform.childCount; i = i + 1)
			{
				Button m_Button = m_ButtonsParent.transform.GetChild(i).GetComponent<Button>();
				if (m_Button != null)
				{
					m_Buttons.Add(m_Button);
				}
			}
		}
	}

	public void ReGenerateButton()
	{
		if (m_ButtonsPrefab != null)
		{
			if (m_Inventory != null)
			{
				int ItemsCount = m_Inventory.GetAItems().Count;
				if (m_Buttons.Count < ItemsCount)
				{
					int count = ItemsCount - m_Buttons.Count;
					for (int i = 0; i < count; i = i + 1)
					{
						m_Buttons.Add(Instantiate(m_ButtonsPrefab, m_ButtonsParent.transform).GetComponent<Button>());
					}
				}
				else if(m_Buttons.Count > ItemsCount)
				{
					int count = m_Buttons.Count - ItemsCount;
					for (int i = 0; i < count; i = i + 1)
					{
						Button t_Button = m_Buttons[m_Buttons.Count - 1];
						m_Buttons.RemoveAt(m_Buttons.Count - 1);
						m_Buttons.TrimExcess();
						if (t_Button != null)
						{ 
							Destroy(t_Button.gameObject);
						}
					}
				}
			}
		}
	}

	public void ResetButtonAction()
	{
		if (m_Inventory != null)
		{
			List<AdvencedItem> t_AItems = m_Inventory.GetAItems();

			for (int i = 0; i < m_Buttons.Count; i = i + 1)
			{
				if(m_Buttons[i].GetComponent<CustomButton>() != null)
				{
					int t_Number = i;

					GameObject t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "ItemImage");
					if(t_GO != null)
					{
						Image t_Image = t_GO.GetComponent<Image>();
						if(t_Image != null)
						{
							t_Image.gameObject.SetActive(true);

							if (t_AItems[t_Number].itemAmount > 0)
							{
								t_Image.sprite = UniFunc.FindSprite(t_AItems[t_Number].itemCode);
							}
						}
					}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "Text (TMP)");
					if(t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							t_Text.text = t_AItems[t_Number].itemCode + " " + (((int)(t_AItems[t_Number].itemProgress * 100.0f)) / 100.0f);
						}
					}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "ItemAmountText (TMP)");
					if (t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							t_Text.text = t_AItems[t_Number].itemAmount + "";
						}
					}
					((CustomButton)m_Buttons[i]).onDown.RemoveAllListeners();
					((CustomButton)m_Buttons[i]).onDown.AddListener(() =>
					{
						if (m_Inventory != null)
						{
							if (m_Inventory.m_Owner != null)
							{
								PlayerCharacter t_PlayerCharacter = m_Inventory.m_Owner.gameObject.GetComponent<PlayerCharacter>();
								if (t_PlayerCharacter != null)
								{
									AdvencedItem t_AItem = m_Inventory.GetAItems()[t_Number];
									t_PlayerCharacter.m_GrabItemCode = new AdvencedItem(t_AItem.itemCode, t_AItem.itemProgress, 1);
									if(t_PlayerCharacter.m_GrabItemSprite != null)
									{
										t_PlayerCharacter.m_GrabItemSprite.sprite = UniFunc.FindSprite(m_Inventory.GetAItems()[t_Number].itemCode);
										t_PlayerCharacter.m_GrabItemSprite.gameObject.SetActive(true);
									}
								}
							}
						}
					});
				}
			}
		}
	}
}
