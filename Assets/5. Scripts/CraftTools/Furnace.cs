using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
	public float m_Progress = 0.0f;
	public float m_ProgressPerSecond = 10.0f;
	public List<Ingredient> m_Ingredients = null;
	public float[] m_Elements = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	public bool m_bProgress = false;

	public Inventory m_Inventory;
	[SerializeField] private TMPro.TextMeshPro m_Text;
	public CraftedItem m_CraftedItem;

	// Start is called before the first frame update
	void Start()
    {
		PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if(t_PlayerCharacter != null)
		{
			m_Inventory = t_PlayerCharacter.GetComponent<Inventory>();
		}
	}

    // Update is called once per frame
    void Update()
    {
		float DeltaTime = Time.deltaTime;

		if (m_bProgress == true)
		{
			m_Progress = m_Progress + ((1 / m_ProgressPerSecond) * DeltaTime);
		}
		if (m_Progress > 1.0f)
		{
			CraftItem();
			//m_Ingredients = null;
			for (int i = 0; i < m_Elements.Length; i = i + 1) { m_Elements[i] = 0.0f; }
			m_Progress = 0.0f;
			m_bProgress = false;
		}

		if (m_Text != null)
		{
			if(m_Progress <= 0.0f)
			{
				m_Text.gameObject.SetActive(false);
			}
			else
			{
				m_Text.gameObject.SetActive(true);
				m_Text.text = (int)(m_Progress * m_ProgressPerSecond) + "";
			}
		}
	}

	private void CraftItem()
	{
		List<GemRecipe> t_GemRecipes = null;
		if (GameManager.Instance.ItemManager != null) { t_GemRecipes = GameManager.Instance.ItemManager.GetGemRecipe(); }

		if(t_GemRecipes != null)
		{
			int t_Element1 = -1;
			int t_Element2 = -1;
			int t_Element3 = -1;
			float t_ElementPercent1 = 0.0f;
			float t_ElementPercent2 = 0.0f;
			float t_ElementPercent3 = 0.0f;

			int count = 0;

			//첫번째로 많은 속성
			for (int i = 0; i < m_Elements.Length; i = i + 1)
			{
				if(t_ElementPercent1 < m_Elements[i])
				{
					t_Element1 = i + 1;
					t_ElementPercent1 = m_Elements[i];
				}
			}
			if (t_Element1 != -1) { m_Elements[t_Element1 - 1] = 0.0f; }
			else if (t_Element1 == -1) { count = count + 1; }

			//두번째로 많은 속성
			for (int i = 0; i < m_Elements.Length; i = i + 1)
			{
				if (t_ElementPercent2 < m_Elements[i])
				{
					t_Element2 = i + 1;
					t_ElementPercent2 = m_Elements[i];
				}
			}
			if (t_Element2 != -1) { m_Elements[t_Element2 - 1] = 0.0f; }
			else if (t_Element2 == -1) { count = count + 1; }

			//세번째로 많은 속성
			for (int i = 0; i < m_Elements.Length; i = i + 1)
			{
				if (t_ElementPercent3 < m_Elements[i])
				{
					t_Element3 = i + 1;
					t_ElementPercent3 = m_Elements[i];
				}
			}
			if (t_Element3 != -1) { m_Elements[t_Element3 - 1] = 0.0f; }
			else if (t_Element3 == -1) { count = count + 1; }

			if (t_Element1 == t_Element2 || t_Element1 == t_Element3) { count = count + 1; }
			for(int i = 0; i < m_Elements.Length; i = i + 1) { if (t_ElementPercent3 == m_Elements[i]) { count = count + 1; } }

			int t_ItemCode = 21;
			float t_Progress = 1.0f;
			int t_ItemAmount = 1;
			if (count < 1)
			{
				List<GemRecipe> t_GRecipes = UniFunc.FindRecipesOfElement(UniFunc.FindRecipesOfElement(UniFunc.FindRecipesOfElement(t_GemRecipes, 1, t_Element1), 2, t_Element2), 3, t_Element3);
				if(t_GRecipes != null)
				{
					t_ElementPercent1 = 1.0f - ((t_GRecipes[0].materialPercent1 - t_ElementPercent1) / t_GRecipes[0].materialPercent1);
					t_ElementPercent2 = 1.0f - ((t_GRecipes[0].materialPercent2 - t_ElementPercent2) / t_GRecipes[0].materialPercent2);
					t_ElementPercent3 = 1.0f - ((t_GRecipes[0].materialPercent3 - t_ElementPercent3) / t_GRecipes[0].materialPercent3);

					t_ItemCode = t_GRecipes[0].itemID;
					if((t_ElementPercent1 + t_ElementPercent2 + t_ElementPercent3) / 3 > 1.0f)
					{ t_Progress = 1.0f / ((t_ElementPercent1 + t_ElementPercent2 + t_ElementPercent3) / 3); }
					else
					{ t_Progress = (t_ElementPercent1 + t_ElementPercent2 + t_ElementPercent3) / 3; }
					t_ItemAmount = 1;
				}
			}

			if(m_CraftedItem != null)
			{
				m_CraftedItem.m_CompleteItem = new AdvencedItem(t_ItemCode, t_Progress, t_ItemAmount);
				m_CraftedItem.RefreshItemDisplay();
				m_CraftedItem.m_IsGrabable = true;

				if(m_Inventory != null)
				{
					if(m_Inventory.m_Owner != null)
					{
						if(((PlayerCharacter)m_Inventory.m_Owner).m_RecipeBook != null)
						{
							((PlayerCharacter)m_Inventory.m_Owner).m_RecipeBook.RegistItem(t_ItemCode, t_Progress, m_Ingredients);
						}
					}
				}
			}
		}

		/*
		//List<float> t_Elements = new List<float>(new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f });
		//List<GemRecipe> t_ItemElements = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();
		//
		//for (int i = 0; i < t_ItemElements.Count; i = i + 1)
		//{
		//	for (int j = 0; j < m_Ingredients.Count; j = j + 1)
		//	{
		//		if (t_ItemElements[i].itemNameEg == m_Ingredients[j].m_Input.ToString())
		//		{
		//			t_Elements[t_ItemElements[i].material1] = t_ItemElements[i].materialPercent1 * m_Ingredients[j].m_Progress;
		//			t_Elements[t_ItemElements[i].material2] = t_ItemElements[i].materialPercent2 * m_Ingredients[j].m_Progress;
		//			t_Elements[t_ItemElements[i].material3] = t_ItemElements[i].materialPercent3 * m_Ingredients[j].m_Progress;
		//		}
		//	}
		//}
		//
		//float t_Value = 0.0f;
		//for(int i = 0; i < t_Elements.Count; i = i + 1)
		//{
		//	if (t_Elements[i] > t_Value)
		//	{
		//		t_Value = t_Elements[i];
		//	}
		//}
		//
		//int count = 0;
		//for(int i = 0; i < t_Elements.Count; i = i + 1)
		//{
		//	if (t_Elements[i] == t_Value)
		//	{
		//		count = count + 1;
		//	}
		//}
		//
		//if(count > 1)
		//{
		//	//돌
		//}
		//else if(count <= 1)
		//{
		//	for (int i = 0; i < t_Elements.Count; i = i + 1)
		//	{
		//		if (t_Elements[i] == t_Value)
		//		{
		//
		//
		//			break;
		//		}
		//	}
		//}
		//
		//List<GemRecipe> GemRecipes = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();
		*/
	}

	

	//public GemRecipe FindItemData(List<GemRecipe> p_GemRecipes, string p_ItemCode)
	//{
	//	GemRecipe t_GemRecipes = null;
	//	for (int i = 0; i < p_GemRecipes.Count; i = i + 1)
	//	{
	//		if ((p_GemRecipes[i].itemNameEg == p_ItemCode) || (p_GemRecipes[i].itemNameKo == p_ItemCode))
	//		{
	//			t_GemRecipes = p_GemRecipes[i];
	//		}
	//	}
	//
	//	return t_GemRecipes;
	//}
	//public int FindItemDataIndex(List<GemRecipe> p_GemRecipes, string p_ItemCode)
	//{
	//	int t_Index = -1;
	//	for (int i = 0; i < p_GemRecipes.Count; i = i + 1)
	//	{
	//		if ((p_GemRecipes[i].itemNameEg == p_ItemCode) || (p_GemRecipes[i].itemNameKo == p_ItemCode))
	//		{
	//			t_Index = i;
	//		}
	//	}
	//
	//	return t_Index;
	//}
}
