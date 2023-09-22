using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ingredient
{
	public int itemCode;
	public float progress;

	public Ingredient(string p_String = null) { itemCode = 0; progress = 0.0f; }
	public Ingredient(int p_Input, float p_Progress) { itemCode = p_Input; progress = p_Progress; }

	public static implicit operator Ingredient(string p_String) { return new Ingredient(); }

	public static implicit operator string(Ingredient p_Ingredient)
	{
		string t_String = null;
		if (p_Ingredient.itemCode != 0 || p_Ingredient.progress != 0.0f) { t_String = p_Ingredient.ToString(); }
		return t_String;
	}
}

public class MixingBowl : MonoBehaviour, IGrabable
{
	public float m_MaxDistance = 3.0f;
	public List<Ingredient> m_Ingredients = new List<Ingredient>();
	public float[] m_Elements = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	[HideInInspector] public bool m_GrabState = false; public void SetGrabState(bool p_State) { m_GrabState = p_State; }
	[HideInInspector] public bool m_IsGrabable = false; public bool IsGrabable() { return m_IsGrabable; }
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
				m_Furnace.m_Ingredients = m_Ingredients;
				m_Ingredients = null;
				m_Furnace.m_bProgress = true;
				m_IsGrabable = false;
				RefreshGraph();
			}

			transform.position = m_OriginPosition;
			m_GrabState = false;
		}
		if (m_GrabState == true)
		{
			GrabMoving();
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
				//Debug.Log("("+ t_MaterialItemData.itemID + ", " + t_MaterialItemData.itemNameKo + ", " + t_MaterialItemData.itemNameEg + ", " + t_MaterialItemData.elementType1 + ", " + t_MaterialItemData.elementType2 + ")");
				if (m_IsGrabable == false) { m_IsGrabable = true; }
				m_Elements[t_MaterialItemData.elementType1 - 1] = m_Elements[t_MaterialItemData.elementType1 - 1] + (t_MaterialItemData.elementPercent1 * 0.01f * p_Progress);
				m_Elements[t_MaterialItemData.elementType2 - 1] = m_Elements[t_MaterialItemData.elementType2 - 1] + (t_MaterialItemData.elementPercent2 * 0.01f * p_Progress);
				//m_Elements[t_MaterialItemData.elementType3 - 1] = m_Elements[t_MaterialItemData.elementType3 - 1] + (t_MaterialItemData.elementPercent3 * 0.01f * p_Progress);
			}
		}

		/*
		*/
		int count = 0;
		for (int i = 0; i < m_Ingredients.Count; i = i + 1)
		{
			if (m_Ingredients[i].itemCode == p_Input)
			{
				m_Ingredients[i] = new Ingredient(m_Ingredients[i].itemCode, m_Ingredients[i].progress + p_Progress);
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

	public void GrabMoving()
	{
		Vector3 t_Vector = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		float t_Value0 = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
		float t_VerticalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.y, t_Vector.z));
		float t_HorizontalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.x, t_Vector.z));
		float t_Value1 = Mathf.Sqrt(Mathf.Pow(Mathf.Tan(t_VerticalAngle) * t_Value0, 2) + Mathf.Pow(Mathf.Tan(t_HorizontalAngle) * t_Value0, 2));
		transform.position = (t_Vector * Mathf.Sqrt(Mathf.Pow(t_Value0, 2) + Mathf.Pow(t_Value1, 2))) + Camera.main.transform.position;

		/*
		Vector3 t_MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
		transform.position = new Vector3(t_MousePosition.x, t_MousePosition.y, Camera.main.orthographic ? transform.position.z : t_MousePosition.z);
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
