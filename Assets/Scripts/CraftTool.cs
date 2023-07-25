using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CraftTool : MonoBehaviour
{
	[SerializeField] private GameObject CraftPanelPrefab;
	private GameObject CraftPanel;
	private GameObject ItemPanel;
	private Inventory inventory;
	[SerializeField] private RecipeBook recipeBook;

	// Start is called before the first frame update
	void Start()
	{
		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (CraftPanelPrefab != null)
			{
				CraftPanel = Instantiate(CraftPanelPrefab, canvas.transform);
				CraftPanel.SetActive(false);

				FlexibleGridLayout ItemPanelLayout = UniversalFunctions.GetChildOfType<FlexibleGridLayout>(CraftPanel);
				if(ItemPanelLayout != null)
				{
					ItemPanel = ItemPanelLayout.gameObject;
				}

				Button button = UniversalFunctions.GetChildOfType<Button>(CraftPanel);
				if(button != null)
				{
					button.onClick.AddListener(()=>
					{
						if (inventory != null)
						{
							if (recipeBook != null)
							{
								List<SelectedItem> selectedItems = inventory.GetSelectedItems();
								List<Item> items = inventory.GetItems();
								string compareString0 = ".";

								for(int i = 0; i < selectedItems.Count; i = i + 1)
								{
									if (selectedItems[i].ItemAmount > 0)
									{
										compareString0 = compareString0 + items[i].itemCode + ".";
									}
								}

								for(int i = 0; i < recipeBook.Recipes.Count; i = i + 1)
								{
									string compareString1 = ".";
									for (int j = 0; j < recipeBook.Recipes[i].ingredients.Count; j = j + 1)
									{
										compareString1 = compareString1 + recipeBook.Recipes[i].ingredients[j].itemCode + ".";
									}

									if(compareString0 == compareString1)
									{
										for (int j = 0; j < selectedItems.Count; j = j + 1)
										{
											if (selectedItems[j].ItemAmount > 0)
											{
												inventory.PopItem(items[j].itemCode, selectedItems[j].ItemAmount);
											}
										}
										inventory.SelectionReset();

										for (int j = 0; j < ItemPanel.transform.childCount; j = j + 1)
										{
											if (ItemPanel.transform.GetChild(j).gameObject.activeSelf == true)
											{
												ItemPanel.transform.GetChild(j).gameObject.SetActive(false);
											}
										}

										inventory.AddItem(recipeBook.Recipes[i].result);
										
										inventory.RefreshInventory();
										break;
									}
								}
							}
						}
					});
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void ItemSelectEvent()
	{
		if (inventory != null)
		{
			int count = 0;
			List<SelectedItem> selectedItems = inventory.GetSelectedItems();
			List<Item> items = inventory.GetItems();
			for (int i = 0; i < (ItemPanel.transform.childCount < selectedItems.Count ? ItemPanel.transform.childCount : selectedItems.Count); i = i + 1)
			{
				if (selectedItems[i].ItemAmount > 0)
				{
					if(ItemPanel.transform.GetChild(count).gameObject.activeSelf == false)
					{
						ItemPanel.transform.GetChild(count).gameObject.SetActive(true);
					}

					Image image = ItemPanel.transform.GetChild(count).GetComponent<Image>();
					TextMeshProUGUI text = null;
					if (image != null)
					{
						text = UniversalFunctions.GetChildOfType<TextMeshProUGUI>(ItemPanel.transform.GetChild(count));

						//
					}
					if (text != null)
					{
						text.text = items[i].itemCode + " " + selectedItems[i].ItemAmount;
					}

					count = count + 1;
				}
			}

			for(int i = count; i < ItemPanel.transform.childCount; i = i + 1)
			{
				if (ItemPanel.transform.GetChild(i).gameObject.activeSelf == true)
				{
					ItemPanel.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != this.gameObject)
		{
			if(other.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				inventory = other.gameObject.GetComponent<Inventory>();
				if (inventory != null)
				{
					inventory.DisplayItems(true);
					inventory.itemSelectEvent.AddListener(ItemSelectEvent);
				}
				if (CraftPanel != null)
				{
					CraftPanel.SetActive(true);
					
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				if(inventory == other.gameObject.GetComponent<Inventory>())
				{
					if (inventory != null)
					{
						inventory.DisplayItems(false);
						inventory.itemSelectEvent.RemoveListener(ItemSelectEvent);
					}

					if (CraftPanel != null)
					{
						CraftPanel.SetActive(false);
					}
				}
			}
		}
	}
}
