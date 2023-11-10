using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : UIScript
{
	public GameObject m_ButtonsPrefab;
	public GameObject m_ButtonsParent;
	public List<Button> m_Buttons;
	public Inventory m_Inventory;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
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
				Button t_Button = m_ButtonsParent.transform.GetChild(i).GetComponent<Button>();
				if (t_Button != null)
				{
					m_Buttons.Add(t_Button);
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
				int count0 = m_Buttons.Count;
				for (int i = 0; i < count0; i = i + 1)
				{
					Button t_Button = m_Buttons[m_Buttons.Count - 1];
					m_Buttons.RemoveAt(m_Buttons.Count - 1);
					m_Buttons.TrimExcess();
					if (t_Button != null)
					{
						Destroy(t_Button.gameObject);
					}
				}

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
				if(m_Buttons[i] as CustomButton != null)
				{
					int t_Number = i;

					if (t_AItems[t_Number].itemCode <= 0 || t_AItems[t_Number].itemCode == 1000 || t_AItems[t_Number].itemCode == 1001)
					{
						m_Buttons[i].gameObject.SetActive(false);
					}

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
					//t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "Text (TMP)");
					//if(t_GO != null)
					//{
					//	TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
					//	if (t_Text != null)
					//	{
					//		t_Text.text = t_AItems[t_Number].itemCode + " " + (((int)(t_AItems[t_Number].itemProgress * 100.0f)) / 100.0f);
					//	}
					//}
					t_GO = UniFunc.GetChildOfName(m_Buttons[i].transform, "ItemAmountText (TMP)");
					if (t_GO != null)
					{
						TextMeshProUGUI t_Text = t_GO.GetComponent<TextMeshProUGUI>();
						if (t_Text != null)
						{
							t_Text.text = t_AItems[t_Number].itemAmount + "";
						}
					}

					CustomButton t_Buttons = m_Buttons[i] as CustomButton;
					if(t_Buttons != null)
					{
						t_Buttons.onDown.RemoveAllListeners();
						t_Buttons.onDown.AddListener(() =>
						{
							if (m_Inventory != null)
							{
								if (m_Inventory.m_Owner != null)
								{
									PlayerCharacter t_PlayerCharacter = m_Inventory.m_Owner as PlayerCharacter;
									if (t_PlayerCharacter != null)
									{
										AdvencedItem t_AItem = m_Inventory.GetAItems()[t_Number];
										if(t_AItem.itemCode > 0)
										{
											BasicItemData basicItemData = UniFunc.FindItemData(t_AItem.itemCode);
											if (basicItemData != null)
											{
												if (basicItemData.itemType == ItemType.Materials)
												{
													List<BBB> t_BBB = FindObjectsOfType<BBB>().ToList<BBB>();
													for (int i = 0; i < t_BBB.Count; i = i + 1)
													{
														t_BBB[i].DestroyObject();
													}
												}

												if (basicItemData.itemType == ItemType.Materials || basicItemData.itemType == ItemType.Accessory)
												{
													MillStone t_MillStone = FindObjectOfType<MillStone>();
													if (t_MillStone != null)
													{
														if (t_MillStone.m_Progress <= 0)
														{
															t_PlayerCharacter.SetPlayerGrabItem(t_PlayerCharacter.m_Inventory.PopAItem(t_AItem.itemCode, t_AItem.itemProgress, 1));
														}
													}
													else if (t_MillStone == null)
													{
														t_PlayerCharacter.SetPlayerGrabItem(t_PlayerCharacter.m_Inventory.PopAItem(t_AItem.itemCode, t_AItem.itemProgress, 1));
													}
												}
											}
										}
										m_Inventory.OnItemClick(t_PlayerCharacter, t_AItem);
									}
								}
							}
						});

						t_Buttons.onEnter.RemoveAllListeners();
						t_Buttons.onEnter.AddListener(() => 
						{
							if (m_Inventory != null)
							{
								if (m_Inventory.m_Owner != null)
								{
									PlayerCharacter t_PlayerCharacter = m_Inventory.m_Owner as PlayerCharacter;
									if (t_PlayerCharacter != null) 
									{
										AdvencedItem t_AItem = m_Inventory.GetAItems()[t_Number];
										t_PlayerCharacter.m_HoverItemCode = new AdvencedItem(t_AItem.itemCode, t_AItem.itemProgress, 1);
										if(t_PlayerCharacter.m_ItemInfoDisplay != null)
										{
											if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO != null)
											{
												if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO.activeSelf == false)
												{
													t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO.SetActive(true);
												}
											}

											BasicItemData t_BasicItemData = UniFunc.FindItemData(t_AItem.itemCode);
											if (t_BasicItemData != null)
											{
												if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemNameText != null)
												{
													t_PlayerCharacter.m_ItemInfoDisplay.m_ItemNameText.text = t_BasicItemData.itemNameKo;
												}
												if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoText != null)
												{
													t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoText.text = t_BasicItemData.itemText;
												}
											}
										}
									}
								}
							}
						});

						t_Buttons.onExit.RemoveAllListeners();
						t_Buttons.onExit.AddListener(() =>
						{
							if (m_Inventory != null)
							{
								if (m_Inventory.m_Owner != null)
								{
									PlayerCharacter t_PlayerCharacter = m_Inventory.m_Owner as PlayerCharacter;
									if (t_PlayerCharacter != null)
									{
										if (t_PlayerCharacter.m_ItemInfoDisplay != null)
										{
											if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO != null)
											{
												if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO.activeSelf == true)
												{
													t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoDisplayGO.SetActive(false);
												}
											}

											if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemNameText != null)
											{
												t_PlayerCharacter.m_ItemInfoDisplay.m_ItemNameText.text = "Null";
											}
											if (t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoText != null)
											{
												t_PlayerCharacter.m_ItemInfoDisplay.m_ItemInfoText.text = "Null";
											}
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

	public override void RefresfAction()
	{
		base.RefresfAction();

		ReFindButton();
		ReGenerateButton();
		ResetButtonAction();
	}
}
