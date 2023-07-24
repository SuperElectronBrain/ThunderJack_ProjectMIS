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

[Serializable]
public struct Item
{
	[SerializeField] public int ItemCode;
	[SerializeField] public int ItemAmount;

	public Item(int itemCode = 0, int itemAmount = 0)
	{
		ItemCode = itemCode;
		ItemAmount = itemAmount;
	}
}

public class Inventory : MonoBehaviour
{
	[SerializeField] private GameObject InventoryPanelPrefab;
	private GameObject InventoryPanel;
	private GameObject ItemPanel;
	private List<Item> Items = new List<Item>();
	private List<Item> SelectedItems = new List<Item>();

	[SerializeField] private InventoryInitializeData InitializeData;
	[HideInInspector] public UnityEvent ItemSelectEvent = new UnityEvent();

	// Start is called before the first frame update
	void Start()
	{
		if(InitializeData != null)
		{
			Items = new List<Item>(InitializeData.Items);
		}

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (InventoryPanelPrefab != null)
			{
				InventoryPanel = Instantiate(InventoryPanelPrefab, canvas.transform);
				InventoryPanel.SetActive(false);

				FlexibleGridLayout ItemPanelLayout = InventoryPanel.GetComponent<FlexibleGridLayout>();
				if(ItemPanelLayout == null)
				{
					for(int i = 0; i < InventoryPanel.transform.childCount; i = i + 1)
					{
						ItemPanelLayout = InventoryPanel.transform.GetChild(i).GetComponent<FlexibleGridLayout>();
						if(ItemPanelLayout == null)
						{
							for (int j = 0; j < InventoryPanel.transform.GetChild(i).childCount; j = j + 1)
							{
								ItemPanelLayout = InventoryPanel.transform.GetChild(i).GetChild(j).GetComponent<FlexibleGridLayout>();
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
					ItemPanel = ItemPanelLayout.gameObject;

					for(int i = 0; i < ItemPanel.transform.childCount; i = i + 1)
					{
						Button button = ItemPanel.transform.GetChild(i).GetComponent<Button>();
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


							SelectedItems.Add(new Item(i, 0));
							int value = i;
							button.onClick.AddListener(() => 
							{
								Item temp = SelectedItems[value];
								if(Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									temp.ItemAmount = temp.ItemAmount + 1;
									if (temp.ItemAmount > Items[value].ItemAmount)
									{
										temp.ItemAmount = Items[value].ItemAmount;
									}
								}
								if(Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
								{
									temp.ItemAmount = temp.ItemAmount - 1;
									if(temp.ItemAmount < 0)
									{
										temp.ItemAmount = 0;
									}
								}

								if(image != null)
								{
									if(temp.ItemAmount > 0)
									{
										image.gameObject.SetActive(true);
										if(text != null)
										{
											text.text = temp.ItemAmount + "";
										}
									}
									else if(temp.ItemAmount <= 0)
									{
										image.gameObject.SetActive(false);
									}
								}
								SelectedItems[value] = temp;

								ItemSelectEvent.Invoke();
							});
						}
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			DisplayItems(!InventoryPanel.activeSelf);
		}
	}

	public void AddItem(Item value)
	{
		for (int i = 0; i < Items.Count; i = i + 1)
		{
			if (Items[i].ItemCode == value.ItemCode)
			{
				Item temp = Items[i];
				temp.ItemAmount = temp.ItemAmount + value.ItemAmount;
				Items[i] = temp;

				return;
			}
		}

		Items.Add(value);
		Items.Sort((a, b) => { return (a.ItemCode < b.ItemCode) ? -1 : 1; });
	}

	public Item PopItem(int ItemCode, int ItemAmount) 
	{
		for (int i = 0; i < Items.Count; i = i + 1)
		{
			if (Items[i].ItemCode == ItemCode)
			{
				Item temp = Items[i];

				if(temp.ItemAmount <= ItemAmount)
				{
					Items.RemoveAt(i);
					return new Item(ItemCode, temp.ItemAmount);
				}
				else if(temp.ItemAmount > ItemAmount)
				{
					temp.ItemAmount = temp.ItemAmount - ItemAmount;
					Items[i] = temp;
					return new Item(ItemCode, ItemAmount);
				}
			}
		}

		return new Item(ItemCode, 0);
	}

	public List<Item> GetItems() 
	{
		return new List<Item>(Items);
	}

	public void SelectionReset()
	{
		for (int i = 0; i < SelectedItems.Count; i = i + 1)
		{
			Item temp = SelectedItems[i];
			temp.ItemAmount = 0;
			SelectedItems[i] = temp;
		}
	}

	public List<Item> GetSelectedItems()
	{
		return new List<Item>(SelectedItems);
	}

	public void RefreshInventory()
	{
		int count = 0;
		if (Items.Count <= ItemPanel.transform.childCount)
		{
			count = Items.Count;
		}
		else if (Items.Count > ItemPanel.transform.childCount)
		{
			count = ItemPanel.transform.childCount;
		}

		for (int i = 0; i < ItemPanel.transform.childCount; i = i + 1)
		{
			Button button = ItemPanel.transform.GetChild(i).GetComponent<Button>();
			Image image = ItemPanel.transform.GetChild(i).GetComponent<Image>();
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
					text.text = Items[i].ItemCode + " " + Items[i].ItemAmount;
				}
				else if (i >= count)
				{
					text.enabled = false;
				}
			}
		}
	}

	public void DisplayItems(bool param)
	{
		if (InventoryPanel != null)
		{
			if(param == true)
			{
				if (InventoryPanel.activeSelf == false)
				{
					InventoryPanel.SetActive(true);
				}

				RefreshInventory();
			}
			else if(param == false)
			{
				if (InventoryPanel.activeSelf == true)
				{
					InventoryPanel.SetActive(false);
				}
				SelectionReset();
			}
		}
	}
}
