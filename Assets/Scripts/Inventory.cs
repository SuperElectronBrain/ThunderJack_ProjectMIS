using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
	private List<Item> items = new List<Item>();
	private List<SelectedItem> selectedItems = new List<SelectedItem>();
	[SerializeField] private GameObject inventoryPanelPrefab;
	private GameObject inventoryPanel;
	private GameObject itemPanel;
	[SerializeField] private GameObject moneyPanelPrefab;
	private GameObject moneyPanel;
	private TextMeshProUGUI moneyText;

	[SerializeField] private InventoryInitializeData initializeData;
	[HideInInspector] public UnityEvent itemSelectEvent = new UnityEvent();

	// Start is called before the first frame update
	void Start()
	{
		if(initializeData != null)
		{
			items = new List<Item>(initializeData.Items);
		}

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (inventoryPanelPrefab != null)
			{
				inventoryPanel = Instantiate(inventoryPanelPrefab, canvas.transform);
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


							selectedItems.Add(new SelectedItem(i, 0));
							int value = i;
							t_Button.onClick.AddListener(() => 
							{
								SelectedItem t_SelectItem = selectedItems[value];
								if(Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount + 1;
									if (t_SelectItem.itemAmount > items[value].itemAmount)
									{
										t_SelectItem.itemAmount = items[value].itemAmount;
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
								selectedItems[value] = t_SelectItem;

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
			DisplayItems(!inventoryPanel.activeSelf);
		}
	}

	public void AddItem(Item p_Item)
	{
		int count = 0;
		for (int i = 0; i < items.Count; i = i + 1)
		{
			if (items[i].itemCode == p_Item.itemCode)
			{
				Item temp = items[i];
				temp.itemAmount = temp.itemAmount + p_Item.itemAmount;
				items[i] = temp;

				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			items.Add(p_Item);
			items.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
		}

		RefreshInventory();
	}
	public void AddItem(ItemCode p_ItemCode, int p_ItemAmount) { AddItem(new Item(p_ItemCode, p_ItemAmount)); }

	public Item PopItem(ItemCode p_ItemCode, int p_ItemAmount) 
	{
		for (int i = 0; i < items.Count; i = i + 1)
		{
			if (items[i].itemCode == p_ItemCode)
			{
				Item temp = items[i];

				if(temp.itemAmount <= p_ItemAmount)
				{
					items.RemoveAt(i);
					return new Item(p_ItemCode, temp.itemAmount);
				}
				else if(temp.itemAmount > p_ItemAmount)
				{
					temp.itemAmount = temp.itemAmount - p_ItemAmount;
					items[i] = temp;
					return new Item(p_ItemCode, p_ItemAmount);
				}
			}
		}
		
		return new Item(p_ItemCode, 0);
	}
	public Item PopItem(Item p_Item) { return PopItem(p_Item.itemCode, p_Item.itemAmount); }

	public List<Item> GetItems() 
	{
		return new List<Item>(items);
	}

	public bool FindItem(ItemCode p_ItemCode, int p_ItemAmount)
	{
		for (int i = 0; i < items.Count; i = i + 1)
		{
			if (items[i].itemCode == p_ItemCode)
			{
				if(items[i].itemAmount >= p_ItemAmount)
				{
					return true;
				}
			}
		}
		return false;
	}
	public bool FindItem(Item p_Item) { return FindItem(p_Item.itemCode, p_Item.itemAmount); }

	public void SelectionReset()
	{
		for (int i = 0; i < selectedItems.Count; i = i + 1)
		{
			SelectedItem temp = selectedItems[i];
			temp.itemAmount = 0;
			selectedItems[i] = temp;
		}
	}

	public List<SelectedItem> GetSelectedItems()
	{
		return new List<SelectedItem>(selectedItems);
	}

	public void UpdateMoney()
	{
		if(moneyPanel != null)
		{
			int count = 0;
			for (int i = 0; i < items.Count; i = i + 1)
			{
				if (items[i].itemCode == ItemCode.Money)
				{
					moneyText.text = items[i].itemAmount + "";
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
		int count = 0;
		if (items.Count <= itemPanel.transform.childCount)
		{
			count = items.Count;
		}
		else if (items.Count > itemPanel.transform.childCount)
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
					text.text = items[i].itemCode + " " + items[i].itemAmount;
				}
				else if (i >= count)
				{
					text.enabled = false;
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
