using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RecipeBookUIScript : MonoBehaviour
{
	[HideInInspector] public ItemRecipe m_ItemRecipe;
	private int m_RecipeBookIndex = 0;

	public GameObject m_RecipePanel;
    public GameObject m_ItemRegistPanel;
    public RecipeBook m_RecipeBook;
    public Inventory m_Inventory;
	private UnityEngine.UI.Image m_RecipeItemImage;
	private TextMeshProUGUI m_RecipeItemNameText;
	private TextMeshProUGUI m_RecipeItemPerfectionText;
	private TextMeshProUGUI m_RecipeItemIngredientsText;
	private UnityEngine.UI.Image m_RegistItemImage;
	private TextMeshProUGUI m_RegistItemNameText;
	private TextMeshProUGUI m_RegistItemPerfectionText;

	// Start is called before the first frame update
	void Start()
    {
		InitializeUIAction();
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	private void InitializeUIAction()
	{
		if (m_RecipePanel != null)
		{
			GameObject t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "ItemImage");
			if (t_GO != null) { m_RecipeItemImage = t_GO.GetComponent<UnityEngine.UI.Image>(); }
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "ItemNameText (TMP)");
			if (t_GO != null) { m_RecipeItemNameText = t_GO.GetComponent<TextMeshProUGUI>(); }
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "ItemPerfectionText (TMP)");
			if (t_GO != null) { m_RecipeItemPerfectionText = t_GO.GetComponent<TextMeshProUGUI>(); }
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "ItemIngredientsText (TMP)");
			if (t_GO != null) { m_RecipeItemIngredientsText = t_GO.GetComponent<TextMeshProUGUI>(); }

			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "PrevButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_RecipeBookIndex = m_RecipeBookIndex - 1;
						if (m_RecipeBookIndex < 0) { m_RecipeBookIndex = 0; }
						RefreshRecipePanel();
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "NextButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_RecipeBookIndex = m_RecipeBookIndex + 1;
						if(m_RecipeBook != null)
						{
							if (m_RecipeBookIndex > m_RecipeBook.GetItemRecipes().Count) { m_RecipeBookIndex = m_RecipeBook.GetItemRecipes().Count; }
						}
						RefreshRecipePanel();
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "CraftButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						if(m_Inventory != null)
						{
							int count = 0;
							for(int i = 0; i < m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients.Count; i = i + 1)
							{
								if (m_Inventory.FindAItem(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients[i].itemCode, 1.0f, 1) == false)
								{
									count = count + 1;
									break;
								}
							}

							if(count < 1)
							{
								for (int i = 0; i < m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients.Count; i = i + 1)
								{
									m_Inventory.PopAItem(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients[i].itemCode, 1.0f, 1);
								}

								m_Inventory.AddAItem(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].itemCode, m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].progress, 1);
								if (m_RecipePanel != null) { m_RecipePanel.SetActive(false); }
							}
						}
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_RecipePanel.transform, "CloseButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_RecipeBookIndex = 0;
						if (m_RecipePanel != null) { m_RecipePanel.SetActive(false); }
					});
				}
			}
		}

		if (m_ItemRegistPanel != null)
		{
			GameObject t_GO = UniFunc.GetChildOfName(m_ItemRegistPanel.transform, "ItemImage");
			if (t_GO != null)
			{
				m_RegistItemImage = t_GO.GetComponent<UnityEngine.UI.Image>();
				//t_Image.sprite = UniFunc.FindSprite(p_ItemCode);
			}
			t_GO = UniFunc.GetChildOfName(m_ItemRegistPanel.transform, "ItemNameText (TMP)");
			if (t_GO != null)
			{
				m_RegistItemNameText = t_GO.GetComponent<TextMeshProUGUI>();
				//t_Text.text = UniFunc.FindItemData(p_ItemCode).itemNameKo;
			}
			t_GO = UniFunc.GetChildOfName(m_ItemRegistPanel.transform, "ItemPerfectionText (TMP)");
			if (t_GO != null)
			{
				m_RegistItemPerfectionText = t_GO.GetComponent<TextMeshProUGUI>();
				//t_Text.text = (int)(p_Progress * 100.0f) + "";
			}

			t_GO = UniFunc.GetChildOfName(m_ItemRegistPanel.transform, "RegistButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						if (m_RecipeBook != null)
						{
							if (m_ItemRecipe != null) { m_RecipeBook.AddRecipe(m_ItemRecipe.itemCode, m_ItemRecipe.progress, m_ItemRecipe.ingredients); }
							m_ItemRecipe = null;
							if (m_ItemRegistPanel != null) { m_ItemRegistPanel.SetActive(false); }
						}
					});
				}
			}
			t_GO = UniFunc.GetChildOfName(m_ItemRegistPanel.transform, "CancelButton");
			if (t_GO != null)
			{
				UnityEngine.UI.Button t_Button = t_GO.GetComponent<UnityEngine.UI.Button>();
				if (t_Button != null)
				{
					t_Button.onClick.AddListener(() =>
					{
						m_ItemRecipe = null;
						if (m_ItemRegistPanel != null) { m_ItemRegistPanel.SetActive(false); }
					});
				}
			}
		}
	}

	public void RefreshRecipePanel()
	{
		if(m_RecipePanel != null)
		{
			if(m_RecipeBook != null)
			{
				if(m_RecipeItemImage != null)
				{
					m_RecipeItemImage.sprite = UniFunc.FindSprite(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].itemCode);
				}
				if (m_RecipeItemNameText != null)
				{
					m_RecipeItemNameText.text = UniFunc.FindItemData(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].itemCode).itemNameKo;
				}
				if (m_RecipeItemPerfectionText != null)
				{
					m_RecipeItemPerfectionText.text = (int)(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].progress * 100.0f) + "";
				}
				if (m_RecipeItemIngredientsText != null)
				{
					string t_String = "";
					for(int i = 0; i < m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients.Count; i = i + 1)
					{
						t_String = t_String + UniFunc.FindItemData(m_RecipeBook.GetItemRecipes()[m_RecipeBookIndex].ingredients[i].itemCode).itemNameKo + "\n";
					}
					m_RecipeItemIngredientsText.text = t_String;
				}
			}
		}
	}

	public void RefreshRegistPanel()
	{
		if (m_ItemRegistPanel != null)
		{
			if (m_RegistItemImage != null)
			{
				m_RegistItemImage.sprite = UniFunc.FindSprite(m_ItemRecipe.itemCode);
			}
			if (m_RegistItemNameText != null)
			{
				m_RegistItemNameText.text = UniFunc.FindItemData(m_ItemRecipe.itemCode).itemNameKo;
			}
			if (m_RegistItemPerfectionText != null)
			{
				m_RegistItemPerfectionText.text = (int)(m_ItemRecipe.progress * 100.0f) + "";
			}
		}
	}

	public void DisplayRecipePanel(bool p_Bool)
	{
		if (m_RecipePanel != null)
		{
			m_RecipePanel.SetActive(p_Bool);
			RefreshRecipePanel();
		}
	}

	public void DisplayRegistPanel(int p_ItemCode, float p_Progress, List<Ingredient> p_Ingredients)
	{
		if (m_ItemRegistPanel != null)
		{
			if(p_ItemCode != 0)
			{
				m_ItemRecipe = new ItemRecipe(p_ItemCode, p_Progress, p_Ingredients);
				m_ItemRegistPanel.SetActive(true);
				RefreshRegistPanel();
			}
			else if(p_ItemCode == 0)
			{
				m_ItemRecipe = null;
				m_ItemRegistPanel.SetActive(false);
			}
		}
	}
}
