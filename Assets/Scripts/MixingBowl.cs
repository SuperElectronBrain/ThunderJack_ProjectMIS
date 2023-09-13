using Spine;
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
	[HideInInspector] public bool m_IsMouseGrab = false;
	[HideInInspector] public bool m_IsMouseGrabable = false;
	private Vector3 m_OriginPosition;

	[SerializeField] private Furnace m_Furnace;
	[SerializeField] private TMPro.TextMeshPro m_ProgressText;
	[SerializeField] private List<GameObject> m_MagicCircleGraph;

	// Start is called before the first frame update
	void Start()
	{
		m_OriginPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (Input.GetMouseButtonUp(0) == true)
		{
			if(m_Furnace != null)
			{
				for (int i = 0; i < m_Furnace.m_Elements.Length; i = i + 1) { m_Furnace.m_Elements[i] = m_Elements[i]; }
				//m_Furnace.m_Elements = m_Elements;
				//Debug.Log(m_Furnace.m_Elements[0] + ", " + m_Furnace.m_Elements[1] + ", " + m_Furnace.m_Elements[2] + ", " + m_Furnace.m_Elements[3] + ", " + m_Furnace.m_Elements[4] + ", " + m_Furnace.m_Elements[5]);
				for (int i = 0; i < m_Elements.Length; i = i + 1) { m_Elements[i] = 0.0f; }
				m_Furnace.m_bProgress = true;
				m_IsMouseGrabable = false;
				RefreshGraph();
			}

			transform.position = m_OriginPosition;
			m_IsMouseGrab = false;
		}
		if (m_IsMouseGrab == true)
		{
			Vector3 t_MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
			transform.position = new Vector3(t_MousePosition.x, t_MousePosition.y, Camera.main.orthographic ? transform.position.z : t_MousePosition.z);
		}

		/*
		//if (m_ProgressText != null)
		//{
		//	string t_String = "";
		//	for(int i = 0; i < m_Ingredients.Count; i = i + 1)
		//	{
		//		if (m_Ingredients[i].m_Input != "")
		//		{
		//			t_String = t_String + m_Ingredients[i].m_Input + " " + (int)(m_Ingredients[i].m_Progress * 100.0f) + "\n";
		//		}
		//	}
		//
		//	m_ProgressText.text = t_String;
		//}
		*/
	}

	public void AddIngredient(int p_Input, float p_Progress)
	{
		/*
		//List<GemRecipe> t_GemRecipes = null;
		//if (GameManager.Instance.ItemManager != null)
		//{
		//	t_GemRecipes = GameManager.Instance.ItemManager.GetGemRecipe();
		//}
		*/

		MaterialItemData t_MaterialItemData = GameManager.Instance.ItemManager.GetMaterialItem(p_Input);
		if (t_MaterialItemData != null)
		{
			/*
			//GemRecipe t_GemRecipe = FindItemData(GameManager.Instance.ItemManager.GetGemRecipe(), p_Input);

			//Debug.Log("(" + t_GemRecipe.itemNameEg + ", " + t_GemRecipe.itemNameKo + ", " + t_GemRecipe.material1 + ", " + t_GemRecipe.material2 + ", " + t_GemRecipe.material3 + ")");
			//Debug.Log("(" + t_MaterialItemData.itemNameEg + ", " + t_MaterialItemData.itemNameKo + ", " + t_MaterialItemData.elementType1 + ", " + t_MaterialItemData.elementType2 + ", " + t_MaterialItemData.elementType3 + ")");
			*/

			if (t_MaterialItemData != null)
			{
				if (m_IsMouseGrabable == false) { m_IsMouseGrabable = true; }
				m_Elements[t_MaterialItemData.elementType1 - 1] = m_Elements[t_MaterialItemData.elementType1 - 1] + (t_MaterialItemData.elementPercent1 * 0.01f * p_Progress);
				m_Elements[t_MaterialItemData.elementType2 - 1] = m_Elements[t_MaterialItemData.elementType2 - 1] + (t_MaterialItemData.elementPercent2 * 0.01f * p_Progress);
				m_Elements[t_MaterialItemData.elementType3 - 1] = m_Elements[t_MaterialItemData.elementType3 - 1] + (t_MaterialItemData.elementPercent3 * 0.01f * p_Progress);
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

	private void OnTriggerEnter(Collider other)
	{
		Furnace t_Furnace = other.GetComponent<Furnace>();
		if (t_Furnace != null)
		{
			m_Furnace = t_Furnace;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Furnace t_Furnace = collision.GetComponent<Furnace>();
		if (t_Furnace != null)
		{
			m_Furnace = t_Furnace;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Furnace t_Furnace = other.GetComponent<Furnace>();
		if (t_Furnace != null)
		{
			if (m_Furnace == t_Furnace)
			{
				m_Furnace = null;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Furnace t_Furnace = collision.GetComponent<Furnace>();
		if (t_Furnace != null)
		{
			if (m_Furnace == t_Furnace)
			{
				m_Furnace = null;
			}
		}
	}
}
