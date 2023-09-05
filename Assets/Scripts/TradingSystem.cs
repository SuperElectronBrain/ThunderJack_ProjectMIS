using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingSystem : MonoBehaviour
{
	[SerializeField] private GameObject TradingPanelPrefab;
	private GameObject TradingPanel;
	[SerializeField] private GameObject SalesPanelPrefab;
	private GameObject SalesPanel;
	private PlayerCharacter playerCharacter;
	private Inventory inventory;
	private NPCShop m_NPCShop = null;
	private List<SelectedItem> selectedItems = new List<SelectedItem>();

	delegate void VFunc();
	VFunc RefreshSalesPanel = null;
	VFunc RefreshTradingPanel = null;

	// Start is called before the first frame update
	void Start()
	{
		playerCharacter = gameObject.GetComponent<PlayerCharacter>();
		inventory = gameObject.GetComponent<Inventory>();

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (SalesPanelPrefab != null)
			{
				SalesPanel = Instantiate(SalesPanelPrefab, canvas.transform);
				SalesPanel.SetActive(false);

				FlexibleGridLayout t_PanelLayout = UniFunc.GetChildComponent<FlexibleGridLayout>(SalesPanel);
				if (t_PanelLayout != null)
				{
					List<Image> t_Images = UniFunc.GetChildsComponent<Image>(t_PanelLayout.gameObject);
					if (t_Images != null)
					{
						RefreshSalesPanel = () =>
						{
							List<Item> t_CostItems = new List<Item>();
							if (m_NPCShop != null)
							{
								for (int i = 0; i < selectedItems.Count; i = i + 1)
								{
									if (selectedItems[i].itemAmount > 0)
									{
										List<Item> t_Costs = m_NPCShop.GetSalesItems()[i].costs;
										for(int j = 0; j < t_Costs.Count; j = j + 1)
										{
											t_CostItems = UniFunc.AddItem(t_CostItems, new Item(t_Costs[j].itemCode, t_Costs[j].itemAmount * selectedItems[i].itemAmount));

											//int count = 0;
											//for (int k = 0; k < t_CostItems.Count; k = k + 1)
											//{
											//	if (t_CostItems[k].itemCode == t_Costs[j].itemCode)
											//	{
											//		Item t_Item = t_CostItems[k];
											//		t_Item.itemAmount = t_Item.itemAmount + (t_Costs[j].itemAmount * selectedItems[i].itemAmount);
											//		t_CostItems[k] = t_Item;
											//
											//		count = count + 1;
											//		break;
											//	}
											//}
											//
											//if (count < 1) { t_CostItems.Add(new Item(t_Costs[j].itemCode, t_Costs[j].itemAmount * selectedItems[i].itemAmount)); }
										}
									}
								}
							}

							for (int i = 0; i < t_Images.Count; i = i + 1)
							{
								if (i < t_CostItems.Count)
								{
									if (t_Images[i].gameObject.activeSelf == false)
									{
										t_Images[i].gameObject.SetActive(true);
									}

									TextMeshProUGUI t_Text = UniFunc.GetChildComponent<TextMeshProUGUI>(t_Images[i].gameObject);
									if (t_Text != null)
									{
										t_Text.text = t_CostItems[i].itemCode + " " + t_CostItems[i].itemAmount;
									}
								}
								else if (i >= t_CostItems.Count)
								{
									if(t_Images[i].gameObject.activeSelf == true)
									{
										t_Images[i].gameObject.SetActive(false);
									}
								}
							}
						};
					}

				}

				Button t_Button = UniFunc.GetChildComponent<Button>(SalesPanel);
				if(t_Button != null)
				{
					t_Button.onClick.AddListener(() => 
					{
						List<Item> t_CostItems = new List<Item>();
						if (m_NPCShop != null)
						{
							for (int i = 0; i < selectedItems.Count; i = i + 1)
							{
								if (selectedItems[i].itemAmount > 0)
								{
									List<Item> t_Costs = m_NPCShop.GetSalesItems()[i].costs;
									for (int j = 0; j < t_Costs.Count; j = j + 1)
									{
										t_CostItems = UniFunc.AddItem(t_CostItems, new Item(t_Costs[j].itemCode, t_Costs[j].itemAmount * selectedItems[i].itemAmount));

										//int count = 0;
										//for (int k = 0; k < t_CostItems.Count; k = k + 1)
										//{
										//	if (t_CostItems[k].itemCode == t_Costs[j].itemCode)
										//	{
										//		Item t_Item = t_CostItems[k];
										//		t_Item.itemAmount = t_Item.itemAmount + (t_Costs[j].itemAmount * selectedItems[i].itemAmount);
										//		t_CostItems[k] = t_Item;
										//
										//		count = count + 1;
										//		break;
										//	}
										//}
										//
										//if (count < 1) { t_CostItems.Add(new Item(t_Costs[j].itemCode, t_Costs[j].itemAmount * selectedItems[i].itemAmount)); }
									}
								}
							}
						}

						if(inventory != null)
						{
							int count = 0;
							for(int i = 0; i < t_CostItems.Count; i = i + 1)
							{
								if(inventory.FindItem(t_CostItems[i]) == false)
								{
									count = count + 1;
									break;
								}
							}

							for (int i = 0; i < selectedItems.Count; i = i + 1)
							{
								if (selectedItems[i].itemAmount > 0)
								{
									List<Item> t_UnlockConditions = m_NPCShop.GetSalesItems()[i].unlockConditions;
									for (int j = 0; j < t_UnlockConditions.Count; j = j + 1)
									{
										if(t_UnlockConditions[j].itemCode != ItemCode.None)
										{
											if (inventory.FindItem(t_UnlockConditions[j]) == false)
											{
												count = count + 1;
												break;
											}
										}
									}
								}
							}

							if(count < 1)
							{
								for (int i = 0; i < t_CostItems.Count; i = i + 1)
								{
									inventory.PopItem(t_CostItems[i]);
								}

								List<SalesItem> t_SalesItems = m_NPCShop.GetSalesItems();
								for (int i = 0; i < selectedItems.Count; i = i + 1)
								{
									if (selectedItems[i].itemAmount > 0)
									{
										inventory.AddItem(t_SalesItems[i].reward.itemCode, t_SalesItems[i].reward.itemAmount * selectedItems[i].itemAmount);
									}
								}

								RefreshSalesPanel();
								inventory.RefreshInventory();
							}
						}
					});

					TextMeshProUGUI t_Text = UniFunc.GetChildComponent<TextMeshProUGUI>(t_Button.gameObject);
					if(t_Text != null)
					{
						t_Text.text = "Buy";
					}
				}
			}

			if (TradingPanelPrefab != null)
			{
				TradingPanel = Instantiate(TradingPanelPrefab, canvas.transform);
				TradingPanel.SetActive(false);

				FlexibleGridLayout t_PanelLayout = UniFunc.GetChildComponent<FlexibleGridLayout>(TradingPanel);
				if (t_PanelLayout != null)
				{
					List<Button> t_Buttons = UniFunc.GetChildsComponent<Button>(t_PanelLayout.gameObject);
					if (t_Buttons != null)
					{
						RefreshTradingPanel = () =>
						{
							List<SalesItem> t_SalesItems = null;
							if (m_NPCShop != null)
							{
								t_SalesItems = m_NPCShop.GetSalesItems();
							}

							for (int i = 0; i < t_Buttons.Count; i = i + 1)
							{
								if(i < (t_SalesItems != null ? t_SalesItems.Count : 0))
								{
									if (t_Buttons[i].gameObject.activeSelf == false)
									{
										t_Buttons[i].gameObject.SetActive(true);
									}

									Image t_Image = t_Buttons[i].GetComponent<Image>();
									TextMeshProUGUI t_Text = t_Image != null ? UniFunc.GetChildComponent<TextMeshProUGUI>(t_Image.gameObject) : null;
									if (t_Image != null)
									{
										if (t_Text != null)
										{
											t_Text.text = t_SalesItems[i].reward.itemCode + " " + t_SalesItems[i].reward.itemAmount;
										}
									}

									t_Image = UniFunc.GetChildComponent<Image>(t_Buttons[i].gameObject);
									t_Text = t_Image != null ? UniFunc.GetChildComponent<TextMeshProUGUI>(t_Image.gameObject) : null;
									if (t_Image != null)
									{
										if (t_Image != null)
										{
											if (selectedItems[i].itemAmount > 0)
											{
												if (t_Image.gameObject.activeSelf == false)
												{
													t_Image.gameObject.SetActive(true);
												}
												if (t_Text != null)
												{
													t_Text.text = selectedItems[i].itemAmount + "";
												}
											}
											else if (selectedItems[i].itemAmount <= 0)
											{
												if (t_Image.gameObject.activeSelf == true)
												{
													t_Image.gameObject.SetActive(false);
												}
											}
										}
									}
								}
								else
								{
									if (t_Buttons[i].gameObject.activeSelf == true)
									{
										t_Buttons[i].gameObject.SetActive(false);
									}
								}
							}
						};

						for (int i = 0; i < t_Buttons.Count; i = i + 1)
						{
							Image t_Image = UniFunc.GetChildComponent<Image>(t_Buttons[i].gameObject);
							TextMeshProUGUI t_Text = t_Image != null ? UniFunc.GetChildComponent<TextMeshProUGUI>(t_Image.gameObject) : null;

							selectedItems.Add(new SelectedItem(i, 0));
							int value = i;
							t_Buttons[i].onClick.AddListener(() =>
							{
								SelectedItem t_SelectItem = selectedItems[value];
								if (Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount + 1;
								}
								if (Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount - 1;
									if (t_SelectItem.itemAmount < 0)
									{
										t_SelectItem.itemAmount = 0;
									}
								}

								if (t_Image != null)
								{
									if (t_SelectItem.itemAmount > 0)
									{
										if (t_Image.gameObject.activeSelf == false)
										{
											t_Image.gameObject.SetActive(true);
										}
										if (t_Text != null)
										{
											t_Text.text = t_SelectItem.itemAmount + "";
										}
									}
									else if (t_SelectItem.itemAmount <= 0)
									{
										if (t_Image.gameObject.activeSelf == true)
										{
											t_Image.gameObject.SetActive(false);
										}
									}
								}

								selectedItems[value] = t_SelectItem;
								if(RefreshSalesPanel != null)
								{
									RefreshSalesPanel();
								}
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
		float DeltaTime = Time.deltaTime;

		if (Input.GetMouseButtonDown(0) == true)
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
			{
				m_NPCShop = null;

				Vector3 mousePosition = Input.mousePosition;
				//mousePosition.z = Camera.main.farClipPlane;
				//mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				mousePosition = Camera.main.ScreenPointToRay(mousePosition).direction;

				RaycastHit hit;
				if (Physics.Raycast(Camera.main.transform.position, mousePosition, out hit, Mathf.Infinity) == true)
				{
					if (hit.transform.gameObject.GetComponent<NonPlayerCharacter>() != null)
					{
						m_NPCShop = hit.transform.gameObject.GetComponent<NPCShop>();
					}
				}

				if (m_NPCShop != null)
				{
					if (TradingPanel != null)
					{
						if (TradingPanel.activeSelf == false)
						{
							TradingPanel.SetActive(true);
							for(int i = 0; i < selectedItems.Count; i = i + 1) { selectedItems[i] = new SelectedItem(selectedItems[i].itemCode, 0); }
							RefreshSalesPanel();
							RefreshTradingPanel();
						}
					}
					if (SalesPanel != null)
					{
						if (SalesPanel.activeSelf == false)
						{
							SalesPanel.SetActive(true);
						}
					}
				}
				else if(m_NPCShop == null)
				{
					if (TradingPanel != null)
					{
						if (TradingPanel.activeSelf == true)
						{
							TradingPanel.SetActive(false);
						}
					}
					if (SalesPanel != null)
					{
						if (SalesPanel.activeSelf == true)
						{
							SalesPanel.SetActive(false);
						}
					}
				}
			}
		}
	}
}
