using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ingredient
{
	public ItemCode m_Input;
	public float m_Progress;

	public Ingredient(ItemCode p_Input, float p_Progress) { m_Input = p_Input; m_Progress = p_Progress; }
}

public class MixingBowl : MonoBehaviour
{
	public float m_MaxDistance = 3.0f;
	public List<Ingredient> m_Ingredients = new List<Ingredient>();

	[SerializeField] private TMPro.TextMeshPro m_ProgressText;
	[SerializeField] private List<GameObject> m_MagicCircleGraph;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (m_ProgressText != null)
		{
			string t_String = "";
			for(int i = 0; i < m_Ingredients.Count; i = i + 1)
			{
				if (m_Ingredients[i].m_Input != ItemCode.None)
				{
					t_String = t_String + m_Ingredients[i].m_Input + " " + (int)(m_Ingredients[i].m_Progress * 100.0f) + "\n";
				}
			}

			m_ProgressText.text = t_String;
		}
	}

	public void AddIngredient(ItemCode p_Input, float p_Progress)
	{
		int count = 0;
		for (int i = 0; i < m_Ingredients.Count; i = i + 1)
		{
			if (m_Ingredients[i].m_Input == p_Input)
			{
				m_Ingredients[i] = new Ingredient(m_Ingredients[i].m_Input, m_Ingredients[i].m_Progress + p_Progress);
				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			m_Ingredients.Add(new Ingredient(p_Input, p_Progress));
		}

		RefreshGraph();
	}

	private void RefreshGraph()
	{
		List<GemRecipe> GemRecipes = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();

		for (int i = 0; i < GemRecipes.Count; i = i + 1)
		{
			for (int j = 0; j < m_Ingredients.Count; j = j + 1)
			{
				if (GemRecipes[i].itemNameEg == m_Ingredients[j].m_Input.ToString())
				{
					m_MagicCircleGraph[GemRecipes[i].material1].transform.localScale = new Vector3(1.0f, GemRecipes[i].materialPercent1 * m_Ingredients[j].m_Progress, 1.0f);
					m_MagicCircleGraph[GemRecipes[i].material2].transform.localScale = new Vector3(1.0f, GemRecipes[i].materialPercent2 * m_Ingredients[j].m_Progress, 1.0f);
					m_MagicCircleGraph[GemRecipes[i].material3].transform.localScale = new Vector3(1.0f, GemRecipes[i].materialPercent3 * m_Ingredients[j].m_Progress, 1.0f);
				}
			}
		}
	}
}
