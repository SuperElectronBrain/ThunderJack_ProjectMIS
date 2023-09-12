using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ingredient
{
	public string m_Input;
	public float m_Progress;

	public Ingredient(string p_Input, float p_Progress) { m_Input = p_Input; m_Progress = p_Progress; }
}

public class MixingBowl : MonoBehaviour
{
	public float m_MaxDistance = 3.0f;
	public List<Ingredient> m_Ingredients = new List<Ingredient>();
	public float[] m_Elements = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

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
				if (m_Ingredients[i].m_Input != "")
				{
					t_String = t_String + m_Ingredients[i].m_Input + " " + (int)(m_Ingredients[i].m_Progress * 100.0f) + "\n";
				}
			}

			m_ProgressText.text = t_String;
		}
	}

	public void AddIngredient(string p_Input, float p_Progress)
	{
		List<GemRecipe> GemRecipes = null;
		if (GameManager.Instance.ItemManager != null)
		{
			GemRecipes = GameManager.Instance.ItemManager.GetGemRecipe();
		}
		if (GemRecipes != null)
		{
			GemRecipe t_GemRecipe = FindItemData(GameManager.Instance.ItemManager.GetGemRecipe(), p_Input);
			if (t_GemRecipe != null)
			{
				m_Elements[t_GemRecipe.material1 - 1] = m_Elements[t_GemRecipe.material1 - 1] + (t_GemRecipe.materialPercent1 * p_Progress);
				m_Elements[t_GemRecipe.material2 - 1] = m_Elements[t_GemRecipe.material2 - 1] + (t_GemRecipe.materialPercent2 * p_Progress);
				m_Elements[t_GemRecipe.material3 - 1] = m_Elements[t_GemRecipe.material3 - 1] + (t_GemRecipe.materialPercent3 * p_Progress);
			}
		}

		/*
		//int count = 0;
		//for (int i = 0; i < m_Ingredients.Count; i = i + 1)
		//{
		//	if (m_Ingredients[i].m_Input == p_Input)
		//	{
		//		m_Ingredients[i] = new Ingredient(m_Ingredients[i].m_Input, m_Ingredients[i].m_Progress + p_Progress);
		//		count = count + 1;
		//		break;
		//	}
		//}
		//
		//if(count < 1)
		//{
		//	m_Ingredients.Add(new Ingredient(p_Input, p_Progress));
		//}
		*/

		RefreshGraph();
	}

	private void RefreshGraph()
	{
		for (int i = 0; i < m_MagicCircleGraph.Count; i = i + 1)
		{
			m_MagicCircleGraph[i].transform.localScale = new Vector3(1.0f, m_Elements[i], 1.0f);
		}
		/*
		//m_MagicCircleGraph[1].transform.localScale = new Vector3(1.0f, m_Elements[1], 1.0f);
		//m_MagicCircleGraph[2].transform.localScale = new Vector3(1.0f, m_Elements[2], 1.0f);
		//m_MagicCircleGraph[3].transform.localScale = new Vector3(1.0f, m_Elements[3], 1.0f);
		//m_MagicCircleGraph[4].transform.localScale = new Vector3(1.0f, m_Elements[4], 1.0f);

		//List<GemRecipe> GemRecipes = new List<GemRecipe>();//GameManager.Instance.ItemManager.GetGemRecipe();
		//List<GemRecipe> GemRecipes = GameManager.Instance.ItemManager.GetGemRecipe();
		//
		//for (int i = 0; i < m_Ingredients.Count; i = i + 1)
		//{
		//	GemRecipe t_GemRecipe = FindItemData(GemRecipes, m_Ingredients[i].m_Input);
		//
		//	if(t_GemRecipe != null)
		//	{
		//		m_MagicCircleGraph[t_GemRecipe.material1 - 1].transform.localScale = new Vector3(1.0f, t_GemRecipe.materialPercent1 * m_Ingredients[i].m_Progress, 1.0f);
		//		m_MagicCircleGraph[t_GemRecipe.material2 - 1].transform.localScale = new Vector3(1.0f, t_GemRecipe.materialPercent2 * m_Ingredients[i].m_Progress, 1.0f);
		//		m_MagicCircleGraph[t_GemRecipe.material3 - 1].transform.localScale = new Vector3(1.0f, t_GemRecipe.materialPercent3 * m_Ingredients[i].m_Progress, 1.0f);
		//	}
		//}
		*/
	}

	public GemRecipe FindItemData(List<GemRecipe> p_GemRecipes, string p_ItemCode)
	{
		GemRecipe t_GemRecipes = null;
		for (int i = 0; i < p_GemRecipes.Count; i = i + 1)
		{
			if (p_GemRecipes[i].itemNameEg == p_ItemCode)
			{
				t_GemRecipes = p_GemRecipes[i];
			}
		}

		return t_GemRecipes;
	}
	public int FindItemDataIndex(List<GemRecipe> p_GemRecipes, string p_ItemCode)
	{
		int t_Index = -1;
		for (int i = 0; i < p_GemRecipes.Count; i = i + 1)
		{
			if (p_GemRecipes[i].itemNameEg == p_ItemCode)
			{
				t_Index = i;
			}
		}

		return t_Index;
	}
}
