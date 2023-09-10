using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
	public float m_Progress = 0.0f;
	public float m_ProgressPerSecond = 0.1f;
	public List<Ingredient> m_Ingredients = null;

	public Inventory m_Inventory;

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

		if(m_Ingredients != null)
		{
			m_Progress = m_Progress + (m_ProgressPerSecond * DeltaTime);
		}
		if (m_Progress > 1.0f)
		{
			CraftItem();
			m_Ingredients = null;
			m_Progress = 0.0f;
		}
	}

	private void CraftItem()
	{
		List<float> t_Elements = new List<float>(new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f });
		List<GemRecipe> t_ItemElements = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();

		for (int i = 0; i < t_ItemElements.Count; i = i + 1)
		{
			for (int j = 0; j < m_Ingredients.Count; j = j + 1)
			{
				if (t_ItemElements[i].itemNameEg == m_Ingredients[j].m_Input.ToString())
				{
					t_Elements[t_ItemElements[i].material1] = t_ItemElements[i].materialPercent1 * m_Ingredients[j].m_Progress;
					t_Elements[t_ItemElements[i].material2] = t_ItemElements[i].materialPercent2 * m_Ingredients[j].m_Progress;
					t_Elements[t_ItemElements[i].material3] = t_ItemElements[i].materialPercent3 * m_Ingredients[j].m_Progress;
				}
			}
		}

		float t_Value = 0.0f;
		for(int i = 0; i < t_Elements.Count; i = i + 1)
		{
			if (t_Elements[i] > t_Value)
			{
				t_Value = t_Elements[i];
			}
		}

		int count = 0;
		for(int i = 0; i < t_Elements.Count; i = i + 1)
		{
			if (t_Elements[i] == t_Value)
			{
				count = count + 1;
			}
		}

		if(count > 1)
		{
			//µ¹
		}
		else if(count <= 1)
		{
			for (int i = 0; i < t_Elements.Count; i = i + 1)
			{
				if (t_Elements[i] == t_Value)
				{


					break;
				}
			}
		}

		List<GemRecipe> GemRecipes = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();

	}
}
