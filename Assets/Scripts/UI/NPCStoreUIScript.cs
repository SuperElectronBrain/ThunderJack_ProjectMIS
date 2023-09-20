using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class NPCStoreUIScript : MonoBehaviour
{
	private int m_ItemCode = 0;
	[HideInInspector] public int m_ItemPrice = 0;
	[HideInInspector] public int m_SelectCount = 0;

	public TextMeshProUGUI m_StoreNameText;
	public GameObject m_ButtonsPrefab;
	public GameObject m_ButtonsParent;
	public List<UnityEngine.UI.Button> m_Buttons;
	public GameObject m_TradePanel;
	public TextMeshProUGUI ItemCountText;
	[HideInInspector] public Inventory m_Inventory;
	[HideInInspector] public Inventory m_NPCInventory;

	// Start is called before the first frame update
	void Start()
	{
		DefineTradePanelAction();
		ReFindButton();
	}

	// Update is called once per frame
	//void Update()
	//{
	//
	//}

	private void OnDisable()
	{
		m_ItemCode = 0;
		m_SelectCount = 0;
		m_SelectCount = 0;
		m_NPCInventory = null;
	}

	private void DefineTradePanelAction()
	{
		if (m_TradePanel != null) 
		{
			GameObject t_GO = UniFunc.GetChildOfName(m_TradePanel.transform, "DecreaseButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_SelectCount = m_SelectCount - 1;
						if (m_SelectCount < 0) { m_SelectCount = 0; }
						if (ItemCountText != null) { ItemCountText.text = m_SelectCount + ""; }
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_TradePanel.transform, "IncreaseButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_SelectCount = m_SelectCount + 1;
						if (ItemCountText != null) { ItemCountText.text = m_SelectCount + ""; }
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_TradePanel.transform, "BuyButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						if(m_Inventory.FindAItem(1000, 1.0f, m_SelectCount * m_ItemPrice) == true)
						{
							m_Inventory.PopAItem(1000, 1.0f, m_SelectCount * m_ItemPrice);
							m_Inventory.AddAItem(m_ItemCode, 1.0f, m_SelectCount);

							m_ItemCode = 0;
							m_SelectCount = 0;
							m_ItemPrice = 0;
							if (ItemCountText != null) { ItemCountText.text = m_SelectCount + ""; }
							if (m_ButtonsParent != null) { if (m_ButtonsParent.transform.parent != null) { m_ButtonsParent.transform.parent.gameObject.SetActive(true); } }
							if (m_TradePanel != null) { m_TradePanel.SetActive(false); }
						}
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_TradePanel.transform, "CancelButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() => 
					{
						m_ItemCode = 0;
						m_SelectCount = 0;
						m_ItemPrice = 0;
						if (ItemCountText != null) { ItemCountText.text = m_SelectCount + ""; }
						if (m_ButtonsParent != null) { if (m_ButtonsParent.transform.parent != null) { m_ButtonsParent.transform.parent.gameObject.SetActive(true); } }
						if (m_TradePanel != null) { m_TradePanel.SetActive(false); }
					});
				}
			}
		}
	}

	public void RefreshUI()
	{
		ReFindButton();
		ReGenerateButton();
		ResetButtonAction();
	}

	public void ReFindButton()
	{
		if (m_ButtonsParent != null)
		{
			m_Buttons.Clear();
			m_Buttons.TrimExcess();
			for (int i = 0; i < m_ButtonsParent.transform.childCount; i = i + 1)
			{
				UnityEngine.UI.Button m_Button = m_ButtonsParent.transform.GetChild(i).GetComponent<UnityEngine.UI.Button>();
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
			if (m_NPCInventory != null)
			{
				int ItemsCount = m_NPCInventory.GetAItems().Count;
				if (m_Buttons.Count < ItemsCount)
				{
					int count = ItemsCount - m_Buttons.Count;
					for (int i = 0; i < count; i = i + 1)
					{
						m_Buttons.Add(Instantiate(m_ButtonsPrefab, m_ButtonsParent.transform).GetComponent<UnityEngine.UI.Button>());
					}
				}
				else if (m_Buttons.Count > ItemsCount)
				{
					int count = m_Buttons.Count - ItemsCount;
					for (int i = 0; i < count; i = i + 1)
					{
						UnityEngine.UI.Button t_Button = m_Buttons[m_Buttons.Count - 1];
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
		if (m_NPCInventory != null)
		{
			List<AdvencedItem> t_AItems = m_NPCInventory.GetAItems();

			for (int i = 0; i < m_Buttons.Count; i = i + 1)
			{
				if (m_Buttons[i].GetComponent<CustomButton>() != null)
				{
					GameObject t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "ItemImage");
					if (t_GO != null)
					{
						UnityEngine.UI.Image t_Image = t_GO.GetComponent<UnityEngine.UI.Image>();
						if (t_Image != null)
						{
							t_Image.gameObject.SetActive(true);

							if (t_AItems[i].itemAmount > 0)
							{
								t_Image.sprite = UniFunc.FindSprite(t_AItems[i].itemCode + "");
							}
						}
					}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "ItemNameText (TMP)");
					if (t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							
							t_Text.text = UniFunc.FindItemData(t_AItems[i].itemCode).itemNameKo;// + (((int)(t_AItems[i].itemProgress * 100.0f)) / 100.0f);
						}
					}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "PriceText (TMP)");
					if (t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							t_Text.text = "$" + t_AItems[i].itemAmount;
						}
					}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "Text (TMP)");
					if (t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							t_Text.text = UniFunc.FindItemData(t_AItems[i].itemCode).itemText;
						}
					}

					int t_Number = i;
					m_Buttons[i].onClick.RemoveAllListeners();
					m_Buttons[i].onClick.AddListener(() =>
					{
						m_ItemCode = t_AItems[t_Number].itemCode;
						m_ItemPrice = t_AItems[t_Number].itemAmount;
						if (m_ButtonsParent != null) { if (m_ButtonsParent.transform.parent != null) { m_ButtonsParent.transform.parent.gameObject.SetActive(false); } }
						if (m_TradePanel != null) { m_TradePanel.SetActive(true); }
					});
				}
			}
		}
	}
}
