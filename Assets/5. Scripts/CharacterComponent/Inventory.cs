using RavenCraftCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum ItemCode
{
	None = 0,

	Ruby = 1,
	Sapphire,
	Topaz,
	Diamond,
	Spinel,

	Ring = 22,
	Necklace,
	Bangle,
	Earring,
	Pin,
	Brooch,

	RubyRing,
	RubyNecklace,

	Money = 1000,
	Honor,
}

[Serializable]
public struct AdvencedItem
{
	[SerializeField] public int itemCode;
	[SerializeField] public float itemProgress;
	[SerializeField] public int itemAmount;
	[SerializeField] public int selectCount;

	public AdvencedItem(AdvencedItem p_AdvencedItem)
	{
		itemCode = p_AdvencedItem.itemCode;
		itemProgress = p_AdvencedItem.itemProgress;
		itemAmount = p_AdvencedItem.itemAmount;
		selectCount = p_AdvencedItem.selectCount;
	}
	public AdvencedItem(int p_ItemCode = 0, float p_ItemProgress = 0.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{
		itemCode = p_ItemCode;
		itemProgress = p_ItemProgress;
		itemAmount = p_ItemAmount;
		selectCount = p_SelectCount;
	}

	public bool IsAddable(AdvencedItem p_AdvencedItem)
	{
		int count = 0;
		if (itemCode != p_AdvencedItem.itemCode) { count = count + 1; }
		if (itemProgress != p_AdvencedItem.itemProgress) { count = count + 1; }

		return count < 1 ? true : false;
	}

	public static AdvencedItem operator + (AdvencedItem p_AdvencedItem0, AdvencedItem p_AdvencedItem1)
	{
		bool isAddable = p_AdvencedItem0.IsAddable(p_AdvencedItem1);
		int t_ItemAmount = isAddable ? p_AdvencedItem0.itemAmount + p_AdvencedItem1.itemAmount : p_AdvencedItem0.itemAmount;
		int t_SelectCount = isAddable ? p_AdvencedItem0.selectCount + p_AdvencedItem1.selectCount : p_AdvencedItem0.selectCount;
		return new AdvencedItem(p_AdvencedItem0.itemCode, p_AdvencedItem0.itemProgress, t_ItemAmount, t_SelectCount);
	}

	public static implicit operator AdvencedItem(string p_String) { return new AdvencedItem(); }
	public static implicit operator string(AdvencedItem p_AItem)
	{
		string t_String = null;
		if (p_AItem.itemCode != 0 && p_AItem.itemProgress != 0.0f) { t_String = p_AItem.ToString(); }
		return t_String;
	}
}

//[Serializable]
//public struct Item
//{
//	[SerializeField] public ItemCode itemCode;
//	[SerializeField] public int itemAmount;
//
//	public Item(ItemCode p_itemCode = ItemCode.None, int p_itemAmount = 0)
//	{
//		itemCode = p_itemCode;
//		itemAmount = p_itemAmount;
//	}
//}

public struct SelectedItem
{
	[SerializeField] public int itemCode;
	[SerializeField] public int itemAmount;

	public SelectedItem(int p_itemCode = 0, int p_itemAmount = 0)
	{
		itemCode = p_itemCode;
		itemAmount = p_itemAmount;
	}
}

public class Inventory : MonoBehaviour
{
	[SerializeField]private List<AdvencedItem> m_AItems = new List<AdvencedItem>();
	public AdvencedItem this[int index] { get { return m_AItems[index]; } set { m_AItems[index] = value; } }
	public TextMeshProUGUI m_MoneyText;
	public TextMeshProUGUI m_HonerText;

	public CharacterBase m_Owner;
	public InventoryUIScript m_InventoryUIScript;

	public static Inventory main
	{
		get
		{
			Inventory t_Inventory = null;
			PlayerCharacter t_PC = FindObjectOfType<PlayerCharacter>();
			if (t_PC != null)
			{
				t_Inventory = t_PC.GetComponent<Inventory>();
			}
			return t_Inventory;
		}
		set {; }
	}

	[HideInInspector] public UnityEvent<PlayerCharacter, AdvencedItem> m_OnItemClick = new UnityEvent<PlayerCharacter, AdvencedItem>();

	// Start is called before the first frame update
	void Start()
	{
		m_Owner = gameObject.GetComponent<CharacterBase>();
		if(m_Owner != null)
		{
			if(m_Owner.m_Inventory == null)
			{
				m_Owner.m_Inventory = this;
			}
		}

		/*
		//Canvas canvas = FindObjectOfType<Canvas>();
		//if (canvas != null)
		//{
		//	//if(m_InventoryUIPrefab != null)
		//	//{
		//	//	if (m_InventoryUIScript == null)
		//	//	{
		//	//		m_InventoryUIScript = Instantiate(m_InventoryUIPrefab, canvas.transform).GetComponent<InventoryUIScript>();
		//	//	}
		//	//}
		//
			//if (inventoryPanelPrefab != null)
			//{
			//	if(inventoryPanel == null)
			//	{
			//		inventoryPanel = Instantiate(inventoryPanelPrefab, canvas.transform);
			//	}
			//
			//}
			//
			//if(inventoryPanel != null)
			//{
			//	inventoryPanel.SetActive(false);
			//
			//	FlexibleGridLayout t_ItemPanelLayout = inventoryPanel.GetComponent<FlexibleGridLayout>();
			//	if(t_ItemPanelLayout == null)
			//	{
			//		for(int i = 0; i < inventoryPanel.transform.childCount; i = i + 1)
			//		{
			//			t_ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetComponent<FlexibleGridLayout>();
			//			if(t_ItemPanelLayout == null)
			//			{
			//				for (int j = 0; j < inventoryPanel.transform.GetChild(i).childCount; j = j + 1)
			//				{
			//					t_ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetChild(j).GetComponent<FlexibleGridLayout>();
			//					if (t_ItemPanelLayout != null)
			//					{
			//						break;
			//					}
			//				}
			//			}
			//
			//			if(t_ItemPanelLayout != null)
			//			{
			//				break;
			//			}
			//		}
			//	}
			//
			//	if(t_ItemPanelLayout != null)
			//	{
			//		itemPanel = t_ItemPanelLayout.gameObject;
			//
			//		for(int i = 0; i < itemPanel.transform.childCount; i = i + 1)
			//		{
			//			Button t_Button = itemPanel.transform.GetChild(i).GetComponent<Button>();
			//			if (t_Button != null)
			//			{
			//				Image t_Image = null;
			//				TextMeshProUGUI t_Text = null;
			//				for (int j = 0; j < t_Button.transform.childCount; j = j + 1)
			//				{
			//					t_Image = t_Button.transform.GetChild(j).GetComponent<Image>();
			//					if (t_Image != null) 
			//					{
			//						for (int k = 0; k < t_Image.transform.childCount; k = k + 1)
			//						{
			//							t_Text = t_Image.transform.GetChild(k).GetComponent<TextMeshProUGUI>();
			//							if (t_Text != null) { break; }
			//						}
			//						break;
			//					}
			//				}
			//
			//
			//				m_SelectedItems.Add(new SelectedItem(i, 0));
			//				int value = i;
			//				t_Button.onClick.AddListener(() => 
			//				{
			//					SelectedItem t_SelectItem = m_SelectedItems[value];
			//					if(Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
			//					{
			//						t_SelectItem.itemAmount = t_SelectItem.itemAmount + 1;
			//						if (t_SelectItem.itemAmount > m_Items[value].itemAmount)
			//						{
			//							t_SelectItem.itemAmount = m_Items[value].itemAmount;
			//						}
			//					}
			//					if(Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
			//					{
			//						t_SelectItem.itemAmount = t_SelectItem.itemAmount - 1;
			//						if(t_SelectItem.itemAmount < 0)
			//						{
			//							t_SelectItem.itemAmount = 0;
			//						}
			//					}
			//
			//					if(t_Image != null)
			//					{
			//						if(t_SelectItem.itemAmount > 0)
			//						{
			//							t_Image.gameObject.SetActive(true);
			//							if(t_Text != null)
			//							{
			//								t_Text.text = t_SelectItem.itemAmount + "";
			//							}
			//						}
			//						else if(t_SelectItem.itemAmount <= 0)
			//						{
			//							t_Image.gameObject.SetActive(false);
			//						}
			//					}
			//					m_SelectedItems[value] = t_SelectItem;
			//
			//					itemSelectEvent.Invoke();
			//				});
			//			}
			//		}
			//	}
			//}
			//
			//if (moneyPanelPrefab != null)
			//{
			//	moneyPanel = Instantiate(moneyPanelPrefab, canvas.transform);
			//	moneyPanel.SetActive(true);
			//
			//	moneyText = UniFunc.GetChildComponent<TextMeshProUGUI>(moneyPanel);
			//	if (moneyText == null)
			//	{
			//		for(int i = 0; i < moneyPanel.transform.childCount; i = i + 1)
			//		{
			//			moneyText = UniFunc.GetChildComponent<TextMeshProUGUI>(moneyPanel.transform.GetChild(i));
			//			if (moneyText != null)
			//			{
			//				break;
			//			}
			//		}
			//	}
			//}
		//}
		*/

		if(m_InventoryUIScript == null)
		{
			m_InventoryUIScript = FindObjectOfType<InventoryUIScript>();
		}
		RefreshInventory();
	}

	public void AddAItem(AdvencedItem p_AItem)
	{
		int count = 0;
		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			if (m_AItems[i].IsAddable(p_AItem) == true)
			{
				m_AItems[i] = m_AItems[i] + p_AItem;

				count = count + 1;
				break;
			}
		}

		if (count < 1)
		{
			m_AItems.Add(p_AItem);
			m_AItems.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
		}

		if(p_AItem.itemCode == 1000)
		{
			PlayerCharacter t_PlayerCharacter = m_Owner as PlayerCharacter;
			if(t_PlayerCharacter != null)
			{
				if(t_PlayerCharacter.m_PlayerCharacterUIScript != null)
				{
					t_PlayerCharacter.m_PlayerCharacterUIScript.m_CurrencyUIScript.AddMoneyText(p_AItem.itemAmount);
				}
			}
		}
		else if (p_AItem.itemCode == 1001)
		{
			PlayerCharacter t_PlayerCharacter = m_Owner as PlayerCharacter;
			if (t_PlayerCharacter != null)
			{
				if (t_PlayerCharacter.m_PlayerCharacterUIScript != null)
				{
					t_PlayerCharacter.m_PlayerCharacterUIScript.m_CurrencyUIScript.AddHonerText(p_AItem.itemAmount);
				}
			}
		}
		
		if (m_Owner as PlayerCharacter)
		{
			IllustratedGuideComponent illustratedGuide = m_Owner.GetComponent<IllustratedGuideComponent>();
			if (illustratedGuide != null)
			{
				BasicItemData basicItemData = UniFunc.FindItemData(p_AItem.itemCode);
				if (basicItemData != null)
				{
					if (basicItemData.itemType == ItemType.Materials)
					{
						IllustratedGuideComponent.main.RegistItem(p_AItem.itemCode);
					}
				}
			}
		}
			
		RefreshInventory();
	}
	public void AddAItem(int p_ItemCode = 0, float p_ItemProgress = 0.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{ AddAItem(new AdvencedItem(p_ItemCode, p_ItemProgress, p_ItemAmount, p_SelectCount)); }

	public AdvencedItem PopAItem(AdvencedItem p_AItem)
	{
		AdvencedItem t_AItem = new AdvencedItem();

		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			if (m_AItems[i].IsAddable(p_AItem) == true)
			{
				if (m_AItems[i].itemAmount <= p_AItem.itemAmount)
				{
					t_AItem = new AdvencedItem(p_AItem.itemCode, p_AItem.itemProgress, m_AItems[i].itemAmount, m_AItems[i].selectCount);
					m_AItems.RemoveAt(i);
					m_AItems.TrimExcess();
				}
				else if (m_AItems[i].itemAmount > p_AItem.itemAmount)
				{
					t_AItem = p_AItem;
					m_AItems[i] = new AdvencedItem (p_AItem.itemCode, p_AItem.itemProgress, m_AItems[i].itemAmount - p_AItem.itemAmount,
						(m_AItems[i].itemAmount - p_AItem.itemAmount) < m_AItems[i].selectCount ? (m_AItems[i].itemAmount - p_AItem.itemAmount) - m_AItems[i].selectCount : 0);
				}

				break;
			}
		}

		if (p_AItem.itemCode == 1000)
		{
			PlayerCharacter t_PlayerCharacter = m_Owner as PlayerCharacter;
			if (t_PlayerCharacter != null)
			{
				if (t_PlayerCharacter.m_PlayerCharacterUIScript != null)
				{
					t_PlayerCharacter.m_PlayerCharacterUIScript.m_CurrencyUIScript.AddMoneyText(-p_AItem.itemAmount);
				}
			}
		}
		else if (p_AItem.itemCode == 1001)
		{
			PlayerCharacter t_PlayerCharacter = m_Owner as PlayerCharacter;
			if (t_PlayerCharacter != null)
			{
				if (t_PlayerCharacter.m_PlayerCharacterUIScript != null)
				{
					t_PlayerCharacter.m_PlayerCharacterUIScript.m_CurrencyUIScript.AddHonerText(-p_AItem.itemAmount);
				}
			}
		}

		RefreshInventory();
		return t_AItem;
	}
	public AdvencedItem PopAItem(int p_ItemCode = 0, float p_ItemProgress = 0.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{ return PopAItem(new AdvencedItem(p_ItemCode, p_ItemProgress, p_ItemAmount, p_SelectCount)); }

	public AdvencedItem PopAItemAt(int p_Index)
	{
		AdvencedItem t_AItem = m_AItems[p_Index];
		m_AItems.RemoveAt(p_Index);
		m_AItems.TrimExcess();
		return t_AItem;
	}

	public List<AdvencedItem> GetAItems() { return new List<AdvencedItem>(m_AItems); }

	public bool FindAItem(AdvencedItem p_AItem)
	{
		bool t_Bool = false;
		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			if (m_AItems[i].IsAddable(p_AItem) == true)
			{
				if (m_AItems[i].itemAmount >= p_AItem.itemAmount)
				{
					t_Bool = true;
					break;
				}
			}
		}
		return t_Bool;
	}
	public bool FindAItem(int p_ItemCode = 0, float p_ItemProgress = 0.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{ return FindAItem(new AdvencedItem(p_ItemCode, p_ItemProgress, p_ItemAmount, p_SelectCount)); }

	public void SelectionReset()
	{
		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			m_AItems[i] = new AdvencedItem(m_AItems[i].itemCode, m_AItems[i].itemProgress, m_AItems[i].itemAmount, 0);
		}
		RefreshInventory();
	}

	public void TakeData(Inventory p_Inventory)
	{
		if(p_Inventory != null)
		{
			int count = p_Inventory.GetAItems().Count;
			for (int i = 0; i < count; i = i + 1)
			{
				AddAItem(p_Inventory.PopAItemAt(p_Inventory.GetAItems().Count - 1));
			}
			p_Inventory.CleanInventory();
		}
	}

	public void CleanInventory()
	{
		m_AItems.Clear();
		m_AItems.TrimExcess();
	}

	public void UpdateMoney()
	{
		if(m_MoneyText != null)
		{
			int count = 0;
			for (int i = 0; i < m_AItems.Count; i = i + 1)
			{
				if (m_AItems[i].itemCode == 1000)
				{
					m_MoneyText.text = m_AItems[i].itemAmount + "";
					count = count + 1;
			
					break;
				}
			}
			
			if (count <= 0)
			{
				m_MoneyText.text = "0";
			}
		}
	}

	public void UpdateHoner()
	{
		if (m_HonerText != null)
		{
			int count = 0;
			for (int i = 0; i < m_AItems.Count; i = i + 1)
			{
				if (m_AItems[i].itemCode == 1001)
				{
					m_HonerText.text = m_AItems[i].itemAmount + "";
					count = count + 1;

					break;
				}
			}

			if (count <= 0)
			{
				m_HonerText.text = "0";
			}
		}
	}

	public void RefreshInventory()
	{
		if (m_InventoryUIScript != null)
		{
			m_InventoryUIScript.m_Inventory = this;
			m_InventoryUIScript.ReFindButton();
			m_InventoryUIScript.ReGenerateButton();
			m_InventoryUIScript.ResetButtonAction();
		}

		UpdateMoney();
		UpdateHoner();
	}

	public void PopUpInventory(bool param)
	{ 
		if (m_InventoryUIScript != null)
		{
			m_InventoryUIScript.gameObject.SetActive(param);
			RefreshInventory();
		}
	}

	public void OnItemClick(PlayerCharacter p_PC,AdvencedItem p_Item)
	{
		m_OnItemClick.Invoke(p_PC, p_Item);
	}
}
