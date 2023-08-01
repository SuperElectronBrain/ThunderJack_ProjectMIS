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

				FlexibleGridLayout ItemPanelLayout = inventoryPanel.GetComponent<FlexibleGridLayout>();
				if(ItemPanelLayout == null)
				{
					for(int i = 0; i < inventoryPanel.transform.childCount; i = i + 1)
					{
						ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetComponent<FlexibleGridLayout>();
						if(ItemPanelLayout == null)
						{
							for (int j = 0; j < inventoryPanel.transform.GetChild(i).childCount; j = j + 1)
							{
								ItemPanelLayout = inventoryPanel.transform.GetChild(i).GetChild(j).GetComponent<FlexibleGridLayout>();
								if (ItemPanelLayout != null)
								{
									break;
								}
							}
						}

						if(ItemPanelLayout != null)
						{
							break;
						}
					}
				}

				if(ItemPanelLayout != null)
				{
					itemPanel = ItemPanelLayout.gameObject;

					for(int i = 0; i < itemPanel.transform.childCount; i = i + 1)
					{
						Button button = itemPanel.transform.GetChild(i).GetComponent<Button>();
						if (button != null)
						{
							Image image = null;
							TextMeshProUGUI text = null;
							for (int j = 0; j < button.transform.childCount; j = j + 1)
							{
								image = button.transform.GetChild(j).GetComponent<Image>();
								if (image != null) 
								{
									for (int k = 0; k < image.transform.childCount; k = k + 1)
									{
										text = image.transform.GetChild(k).GetComponent<TextMeshProUGUI>();
										if (text != null) { break; }
									}
									break;
								}
							}


							selectedItems.Add(new SelectedItem(i, 0));
							int value = i;
							button.onClick.AddListener(() => 
							{
								SelectedItem temp = selectedItems[value];
								if(Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									temp.itemAmount = temp.itemAmount + 1;
									if (temp.itemAmount > items[value].itemAmount)
									{
										temp.itemAmount = items[value].itemAmount;
									}
								}
								if(Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
								{
									temp.itemAmount = temp.itemAmount - 1;
									if(temp.itemAmount < 0)
									{
										temp.itemAmount = 0;
									}
								}

								if(image != null)
								{
									if(temp.itemAmount > 0)
									{
										image.gameObject.SetActive(true);
										if(text != null)
										{
											text.text = temp.itemAmount + "";
										}
									}
									else if(temp.itemAmount <= 0)
									{
										image.gameObject.SetActive(false);
									}
								}
								selectedItems[value] = temp;

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

				moneyText = UniversalFunctions.GetChildOfType<TextMeshProUGUI>(moneyPanel);
				if (moneyText == null)
				{
					for(int i = 0; i < moneyPanel.transform.childCount; i = i + 1)
					{
						moneyText = UniversalFunctions.GetChildOfType<TextMeshProUGUI>(moneyPanel.transform.GetChild(i));
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
		if(Input.GetKeyDown(KeyCode.E))
		{
			DisplayItems(!inventoryPanel.activeSelf);
		}
	}

	public void AddItem(Item value)
	{
		for (int i = 0; i < items.Count; i = i + 1)
		{
			if (items[i].itemCode == value.itemCode)
			{
				Item temp = items[i];
				temp.itemAmount = temp.itemAmount + value.itemAmount;
				items[i] = temp;

				return;
			}
		}

		items.Add(value);
		items.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
	}

	public Item PopItem(ItemCode itemCode, int ItemAmount) 
	{
		for (int i = 0; i < items.Count; i = i + 1)
		{
			if (items[i].itemCode == itemCode)
			{
				Item temp = items[i];

				if(temp.itemAmount <= ItemAmount)
				{
					items.RemoveAt(i);
					return new Item(itemCode, temp.itemAmount);
				}
				else if(temp.itemAmount > ItemAmount)
				{
					temp.itemAmount = temp.itemAmount - ItemAmount;
					items[i] = temp;
					return new Item(itemCode, ItemAmount);
				}
			}
		}
		
		return new Item(itemCode, 0);
	}

	public List<Item> GetItems() 
	{
		return new List<Item>(items);
	}

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
