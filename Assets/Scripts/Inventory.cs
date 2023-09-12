using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ItemCode
{
	None = 0,

	Ruby,
	Sapphire,
	Topaz,
	Diamond,
	Spinel,

	Ring,
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
	[SerializeField] public string itemCode;
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
	public AdvencedItem(string p_ItemCode = "", float p_ItemProgress = 1.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
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

	//public static bool operator ==(AdvencedItem p_AdvencedItem0, AdvencedItem p_AdvencedItem1)
	//{
	//	int count = 0;
	//	if (p_AdvencedItem0.itemCode != p_AdvencedItem1.itemCode) { count = count + 1; }
	//	if (p_AdvencedItem0.itemProgress != p_AdvencedItem1.itemProgress) { count = count + 1; }
	//
	//	return count < 1 ? true : false;
	//}
	//public static bool operator !=(AdvencedItem p_AdvencedItem0, AdvencedItem p_AdvencedItem1)
	//{
	//	int count = 0;
	//	if (p_AdvencedItem0.itemCode != p_AdvencedItem1.itemCode) { count = count + 1; }
	//	if (p_AdvencedItem0.itemProgress != p_AdvencedItem1.itemProgress) { count = count + 1; }
	//
	//	return count > 0 ? true : false;
	//}
}

[Serializable]
public struct Item
{
	[SerializeField] public ItemCode itemCode;
	[SerializeField] public int itemAmount;

	public Item(ItemCode p_itemCode = ItemCode.None, int p_itemAmount = 0)
	{
		itemCode = p_itemCode;
		itemAmount = p_itemAmount;
	}
}

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
	private List<Item> m_Items = new List<Item>();
	private List<SelectedItem> m_SelectedItems = new List<SelectedItem>();
	[SerializeField]private List<AdvencedItem> m_AItems = new List<AdvencedItem>();
	public AdvencedItem this[int index] { get { return m_AItems[index]; } set { m_AItems[index] = value; } }
	[SerializeField] private GameObject inventoryPanelPrefab;
	[SerializeField] private GameObject inventoryPanel;
	private GameObject itemPanel;
	[SerializeField] private GameObject moneyPanelPrefab;
	private GameObject moneyPanel;
	private TextMeshProUGUI moneyText;

	[SerializeField] private InventoryInitializeData initializeData;
	[HideInInspector] public UnityEvent itemSelectEvent = new UnityEvent();
	[SerializeField] private GameObject m_InventoryUIPrefab;
	[SerializeField] private InventoryUIScript m_InventoryUIScript;
	public CharacterBase m_Owner;

	// Start is called before the first frame update
	void Start()
	{
		if(initializeData != null)
		{
			m_Items = new List<Item>(initializeData.Items);
		}

		m_Owner = gameObject.GetComponent<CharacterBase>();

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if(m_InventoryUIPrefab != null)
			{
				if (m_InventoryUIScript == null)
				{
					m_InventoryUIScript = Instantiate(m_InventoryUIPrefab, canvas.transform).GetComponent<InventoryUIScript>();
				}
			}

			if(m_InventoryUIScript != null)
			{
				m_InventoryUIScript.m_Inventory = this;
				m_InventoryUIScript.ReFindButton();
				m_InventoryUIScript.ReGenerateButton();
				m_InventoryUIScript.ResetButtonAction();
			}

			if (inventoryPanelPrefab != null)
			{
				if(inventoryPanel == null)
				{
					inventoryPanel = Instantiate(inventoryPanelPrefab, canvas.transform);
				}

			}

			if(inventoryPanel != null)
			{
				inventoryPanel.SetActive(false);

				FlexibleGridLayout t_ItemPanelLayout = inventoryPanel.GetComponent<FlexibleGridLayout>();
				if(t_ItemPanelLayout == null)
				{
					for(int i = 0; i < inventoryPanel.transform.childCount; i = i + 1)
					{
						t_ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetComponent<FlexibleGridLayout>();
						if(t_ItemPanelLayout == null)
						{
							for (int j = 0; j < inventoryPanel.transform.GetChild(i).childCount; j = j + 1)
							{
								t_ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetChild(j).GetComponent<FlexibleGridLayout>();
								if (t_ItemPanelLayout != null)
								{
									break;
								}
							}
						}

						if(t_ItemPanelLayout != null)
						{
							break;
						}
					}
				}

				if(t_ItemPanelLayout != null)
				{
					itemPanel = t_ItemPanelLayout.gameObject;

					for(int i = 0; i < itemPanel.transform.childCount; i = i + 1)
					{
						Button t_Button = itemPanel.transform.GetChild(i).GetComponent<Button>();
						if (t_Button != null)
						{
							Image t_Image = null;
							TextMeshProUGUI t_Text = null;
							for (int j = 0; j < t_Button.transform.childCount; j = j + 1)
							{
								t_Image = t_Button.transform.GetChild(j).GetComponent<Image>();
								if (t_Image != null) 
								{
									for (int k = 0; k < t_Image.transform.childCount; k = k + 1)
									{
										t_Text = t_Image.transform.GetChild(k).GetComponent<TextMeshProUGUI>();
										if (t_Text != null) { break; }
									}
									break;
								}
							}


							m_SelectedItems.Add(new SelectedItem(i, 0));
							int value = i;
							t_Button.onClick.AddListener(() => 
							{
								SelectedItem t_SelectItem = m_SelectedItems[value];
								if(Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount + 1;
									if (t_SelectItem.itemAmount > m_Items[value].itemAmount)
									{
										t_SelectItem.itemAmount = m_Items[value].itemAmount;
									}
								}
								if(Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount - 1;
									if(t_SelectItem.itemAmount < 0)
									{
										t_SelectItem.itemAmount = 0;
									}
								}

								if(t_Image != null)
								{
									if(t_SelectItem.itemAmount > 0)
									{
										t_Image.gameObject.SetActive(true);
										if(t_Text != null)
										{
											t_Text.text = t_SelectItem.itemAmount + "";
										}
									}
									else if(t_SelectItem.itemAmount <= 0)
									{
										t_Image.gameObject.SetActive(false);
									}
								}
								m_SelectedItems[value] = t_SelectItem;

								itemSelectEvent.Invoke();
							});
						}
					}
				}
			}

			if (moneyPanelPrefab != null)
			{
				moneyPanel = Instantiate(moneyPanelPrefab, canvas.transform);
				moneyPanel.SetActive(true);

				moneyText = UniFunc.GetChildComponent<TextMeshProUGUI>(moneyPanel);
				if (moneyText == null)
				{
					for(int i = 0; i < moneyPanel.transform.childCount; i = i + 1)
					{
						moneyText = UniFunc.GetChildComponent<TextMeshProUGUI>(moneyPanel.transform.GetChild(i));
						if (moneyText != null)
						{
							break;
						}
					}
				}
			}
		}

		RefreshInventory();
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.E))
		{
			m_InventoryUIScript.gameObject.SetActive(!m_InventoryUIScript.gameObject.activeSelf);

			if(inventoryPanel != null)
			{
				DisplayItems(!inventoryPanel.activeSelf);
			}
		}
	}

	public void AddItem(Item p_Item)
	{
		int count = 0;
		for (int i = 0; i < m_Items.Count; i = i + 1)
		{
			if (m_Items[i].itemCode == p_Item.itemCode)
			{
				Item temp = m_Items[i];
				temp.itemAmount = temp.itemAmount + p_Item.itemAmount;
				m_Items[i] = temp;

				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			m_Items.Add(p_Item);
			m_Items.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
		}

		RefreshInventory();
	}
	public void AddItem(ItemCode p_ItemCode, int p_ItemAmount) { AddItem(new Item(p_ItemCode, p_ItemAmount)); }

	public Item PopItem(ItemCode p_ItemCode, int p_ItemAmount) 
	{
		for (int i = 0; i < m_Items.Count; i = i + 1)
		{
			if (m_Items[i].itemCode == p_ItemCode)
			{
				Item temp = m_Items[i];

				if(temp.itemAmount <= p_ItemAmount)
				{
					m_Items.RemoveAt(i);
					m_Items.TrimExcess();
					return new Item(p_ItemCode, temp.itemAmount);
				}
				else if(temp.itemAmount > p_ItemAmount)
				{
					temp.itemAmount = temp.itemAmount - p_ItemAmount;
					m_Items[i] = temp;
					return new Item(p_ItemCode, p_ItemAmount);
				}
			}
		}
		
		return new Item(p_ItemCode, 0);
	}
	public Item PopItem(Item p_Item) { return PopItem(p_Item.itemCode, p_Item.itemAmount); }

	public List<Item> GetItems() { return new List<Item>(m_Items); }

	public bool FindItem(ItemCode p_ItemCode, int p_ItemAmount)
	{
		for (int i = 0; i < m_Items.Count; i = i + 1)
		{
			if (m_Items[i].itemCode == p_ItemCode)
			{
				if(m_Items[i].itemAmount >= p_ItemAmount)
				{
					return true;
				}
			}
		}
		return false;
	}
	public bool FindItem(Item p_Item) { return FindItem(p_Item.itemCode, p_Item.itemAmount); }

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
			//m_AItems.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
		}

		RefreshInventory();
	}
	public void AddAItem(string p_ItemCode = "", float p_ItemProgress = 1.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
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
					m_AItems[i] = m_AItems[i] + new AdvencedItem
					(
						p_AItem.itemCode,
						p_AItem.itemProgress,
						-p_AItem.itemAmount,
						(m_AItems[i].itemAmount - p_AItem.itemAmount) < m_AItems[i].selectCount ? (m_AItems[i].itemAmount - p_AItem.itemAmount) - m_AItems[i].selectCount : 0
					);
				}

				break;
			}
		}

		return t_AItem;
	}
	public AdvencedItem PopAItem(string p_ItemCode = "", float p_ItemProgress = 1.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{ return PopAItem(new AdvencedItem(p_ItemCode, p_ItemProgress, p_ItemAmount, p_SelectCount)); }

	public List<AdvencedItem> GetAItems() { return new List<AdvencedItem>(m_AItems); }

	public bool FindAItem(AdvencedItem p_AItem)
	{
		bool t_Bool = false;
		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			if (m_AItems[i].IsAddable(p_AItem) == true)
			{
				if (m_Items[i].itemAmount >= p_AItem.itemAmount)
				{
					t_Bool = true;
					break;
				}
			}
		}
		return t_Bool;
	}
	public bool FindAItem(string p_ItemCode = "", float p_ItemProgress = 1.0f, int p_ItemAmount = 0, int p_SelectCount = 0)
	{ return FindAItem(new AdvencedItem(p_ItemCode, p_ItemProgress, p_ItemAmount, p_SelectCount)); }

	public void SelectionReset()
	{
		for (int i = 0; i < m_SelectedItems.Count; i = i + 1)
		{
			m_SelectedItems[i] = new SelectedItem(m_SelectedItems[i].itemCode, 0);
		}

		for (int i = 0; i < m_AItems.Count; i = i + 1)
		{
			m_AItems[i] = new AdvencedItem(m_AItems[i].itemCode, m_AItems[i].itemProgress, m_AItems[i].itemAmount, 0);
		}
	}

	public List<SelectedItem> GetSelectedItems()
	{
		return new List<SelectedItem>(m_SelectedItems);
	}

	public void UpdateMoney()
	{
		if(moneyPanel != null)
		{
			int count = 0;
			for (int i = 0; i < m_Items.Count; i = i + 1)
			{
				if (m_Items[i].itemCode == ItemCode.Money)
				{
					moneyText.text = m_Items[i].itemAmount + "";
					count = count + 1;

					break;
				}
			}

			if (count <= 0)
			{
				moneyText.text = "0";
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

		if(itemPanel != null)
		{
			int count = 0;
			if (m_Items.Count <= itemPanel.transform.childCount)
			{
				count = m_Items.Count;
			}
			else if (m_Items.Count > itemPanel.transform.childCount)
			{
				count = itemPanel.transform.childCount;
			}

			for (int i = 0; i < itemPanel.transform.childCount; i = i + 1)
			{
				Button button = itemPanel.transform.GetChild(i).GetComponent<Button>();
				Image image = itemPanel.transform.GetChild(i).GetComponent<Image>();
				Image selectionCounter = null;
				TextMeshProUGUI text = null;
				if (button != null)
				{
					for (int j = 0; j < button.transform.childCount; j = j + 1)
					{
						text = button.transform.GetChild(j).GetComponent<TextMeshProUGUI>();
						if (text != null) { break; }
					}
					for (int j = 0; j < button.transform.childCount; j = j + 1)
					{
						selectionCounter = button.transform.GetChild(j).GetComponent<Image>();
						if (selectionCounter != null) { selectionCounter.gameObject.SetActive(false); break; }
					}
				}

				if (image != null)
				{
					if (i < count)
					{
						image.enabled = true;
					}
					else if (i >= count)
					{
						image.enabled = false;
					}
				}
				if (text != null)
				{
					if (i < count)
					{
						text.enabled = true;
						text.text = m_Items[i].itemCode + " " + m_Items[i].itemAmount;
					}
					else if (i >= count)
					{
						text.enabled = false;
					}
				}
			}
		}

		UpdateMoney();
	}

	public void DisplayItems(bool param)
	{
		if (inventoryPanel != null)
		{
			if(param == true)
			{
				if (inventoryPanel.activeSelf == false)
				{
					inventoryPanel.SetActive(true);
				}

				RefreshInventory();
			}
			else if(param == false)
			{
				if (inventoryPanel.activeSelf == true)
				{
					inventoryPanel.SetActive(false);
				}
				SelectionReset();
			}
		}
	}
}
