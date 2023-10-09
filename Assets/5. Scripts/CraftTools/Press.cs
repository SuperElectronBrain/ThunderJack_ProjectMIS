using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour
{
	public float m_MaxDistance = 3.0f;
	[HideInInspector] public bool bProgress = false;
	private float m_Progress = 0.0f;
	public List<Ingredient> m_Ingredients = new List<Ingredient>();
	public float[] m_Elements = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	public AdvencedItem m_AccessoryInput = new AdvencedItem();
	public AdvencedItem m_CraftedItem = new AdvencedItem();
	private Vector3 m_PreviousHandlePosition;

	[SerializeField] private GameObject m_PressHandleBase;
	//[SerializeField] private TMPro.TextMeshPro m_Text;
	[SerializeField] private List<GameObject> m_MagicCircleGraph;
	[SerializeField] private MeshRenderer m_MagicCircleMaterial;
	[SerializeField] private SpriteRenderer m_AccessorySprite;
	[SerializeField] private SpriteRenderer m_OutputSprite;
	[SerializeField] private SkeletonAnimation m_SkeletonAnimation;
	private TrackEntry trackEntry;
	private Inventory m_Inventory;

	[SerializeField]
	AnimationCurve curve;

	// Start is called before the first frame update
	void Start()
    {
		m_PreviousHandlePosition = Vector3.left;
		if (m_SkeletonAnimation!= null)
		{
			trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "animation", false);
		}

		PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			m_Inventory = t_PlayerCharacter.GetComponent<Inventory>();
		}

		//if (m_MagicCircleMaterial != null)
		//{
		//	m_MagicCircleMaterial.material = Instantiate(m_MagicCircleMaterial.material);
		//}
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) == true)
		{
			m_PreviousHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint((m_PressHandleBase != null ? m_PressHandleBase : gameObject).transform.position)).normalized;
		}
		if (Input.GetMouseButtonUp(0) == true)
		{
			m_Progress = 0.0f;
			bProgress = false;
			if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
		}

		if (bProgress == true)
		{
			int count = 0;
			for (int i = 0; i < m_Elements.Length; i = i + 1) { if (m_Elements[i] > 0.0f) { count = count + 1; } }
			if (count > 0 && m_AccessoryInput != null)
			{
				Vector3 t_CurrentHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint((m_PressHandleBase != null ? m_PressHandleBase : gameObject).transform.position)).normalized;

				//transform.position을 기준으로 이전 프레임과 현재 프레임간의 마우스의 각도 변화량을 계산하는 식
				float t_X = Vector3.Dot(m_PreviousHandlePosition, t_CurrentHandlePosition);
				float t_Y = Vector3.Dot((t_CurrentHandlePosition - (m_PreviousHandlePosition * t_X)).normalized, t_CurrentHandlePosition);
				float t_Progress = (Vector3.Cross(m_PreviousHandlePosition, t_CurrentHandlePosition).z < 0 ? 1 : -1) * (Mathf.Atan2(t_Y, t_X) / Mathf.PI) / 2;
				if (t_Progress != 0.0f)
				{
					m_Progress = m_Progress + (t_Progress * 3.5f);
					m_Progress = m_Progress > 1.0f ? 1.0f : (m_Progress < 0.0f ? 0.0f : m_Progress);

					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 1.0f; }
				}
				else if (t_Progress == 0)
				{
					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}

				if (m_Progress >= 1.0f)
				{
					if(m_CraftedItem == null)
					{
						m_CraftedItem = CraftItem(CraftGem());

						if(m_CraftedItem.itemCode != 0)
						{
							if (m_Inventory != null)
							{
								if (m_Inventory.m_Owner != null)
								{
									if (((PlayerCharacter)m_Inventory.m_Owner).m_RecipeBook != null)
									{
										m_Ingredients.Add(new Ingredient(m_AccessoryInput.itemCode, m_AccessoryInput.itemProgress));
										((PlayerCharacter)m_Inventory.m_Owner).m_RecipeBook.RegistItem(m_CraftedItem.itemCode, m_CraftedItem.itemProgress, m_Ingredients);
									}
								}
							}
						}

						for (int i = 0; i < m_Elements.Length; i = i + 1) { m_Elements[i] = 0.0f; }
						for (int i = 0; i < m_Ingredients.Count; i = i + 1) { m_Ingredients[i] = null; }
						m_AccessoryInput = null;
					}
					RefreshOutput();
					m_Progress = 0.0f;
					bProgress = false;

					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}

				m_PreviousHandlePosition = t_CurrentHandlePosition;
			}
		}

		if (m_PressHandleBase != null)
		{
			m_PressHandleBase.transform.rotation = Quaternion.Euler(0f, 0f, 50.0f - (m_Progress * 100.0f));
		}

		if (trackEntry != null)
		{
			trackEntry.TrackTime = m_Progress * trackEntry.AnimationEnd;
		}
	}

	public void AddIngredient(int p_Input, float p_Progress)
	{
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.ItemManager != null)
			{
				MaterialItemData t_MaterialItemData = (MaterialItemData)GameManager.Instance.ItemManager.GetBasicItemData(p_Input);
				if (t_MaterialItemData != null)
				{
					if (t_MaterialItemData != null)
					{
						m_Elements[t_MaterialItemData.elementType1 - 1] = m_Elements[t_MaterialItemData.elementType1 - 1] + (t_MaterialItemData.elementPercent1 * 0.01f * p_Progress);
						m_Elements[t_MaterialItemData.elementType2 - 1] = m_Elements[t_MaterialItemData.elementType2 - 1] + (t_MaterialItemData.elementPercent2 * 0.01f * p_Progress);
						//m_Elements[t_MaterialItemData.elementType3 - 1] = m_Elements[t_MaterialItemData.elementType3 - 1] + (t_MaterialItemData.elementPercent3 * 0.01f * p_Progress);
					}
				}
			}
		}

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

		if (count < 1) { m_Ingredients.Add(new Ingredient(p_Input, p_Progress)); }

		RefreshGraph();
	}

	private AdvencedItem CraftGem()
	{
		AdvencedItem t_AItem = null;

		List<GemRecipe> t_GemRecipes = null;
		if (GameManager.Instance.ItemManager != null) { t_GemRecipes = GameManager.Instance.ItemManager.GetGemRecipe(); }

		if (t_GemRecipes != null)
		{
			int t_Element1 = -1; int t_Element2 = -1; int t_Element3 = -1;
			float t_ElementPercent1 = 0.0f; float t_ElementPercent2 = 0.0f; float t_ElementPercent3 = 0.0f;

			int count = 0;
			//첫번째로 많은 속성
			for (int i = 0; i < m_Elements.Length; i = i + 1)
			{
				if (t_ElementPercent1 < m_Elements[i])
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
			for (int i = 0; i < m_Elements.Length; i = i + 1) { if (t_ElementPercent3 == m_Elements[i]) { count = count + 1; } }

			int t_ItemCode = 21;
			float t_Progress = 1.0f;
			int t_ItemAmount = 1;
			if (count < 1)
			{
				List<GemRecipe> t_GRecipes = UniFunc.FindRecipesOfElement(UniFunc.FindRecipesOfElement(UniFunc.FindRecipesOfElement(t_GemRecipes, 1, t_Element1), 2, t_Element2), 3, t_Element3);
				if (t_GRecipes != null)
				{
					t_ElementPercent1 = Mathf.Abs(1.0f - (t_ElementPercent1 / t_GRecipes[0].materialPercent1)) / 3;
					t_ElementPercent2 = Mathf.Abs(1.0f - (t_ElementPercent2 / t_GRecipes[0].materialPercent2)) / 3;
					t_ElementPercent3 = Mathf.Abs(1.0f - (t_ElementPercent3 / t_GRecipes[0].materialPercent3)) / 3;

					t_ItemCode = t_GRecipes[0].itemID;
					t_Progress = 1 - (t_ElementPercent1 + t_ElementPercent2 + t_ElementPercent3);
					t_ItemAmount = 1;
				}
			}

			t_AItem = new AdvencedItem(t_ItemCode, t_Progress, t_ItemAmount);
		}
		return t_AItem;
	}

	public AdvencedItem CraftItem(AdvencedItem p_AItem)
	{
		AdvencedItem t_AItem = null;

		int t_ItemCode = 0;
		if(GameManager.Instance != null)
		{
			if (GameManager.Instance.ItemManager != null)
			{
				t_ItemCode = GameManager.Instance.ItemManager.GetCombinationItem(p_AItem.itemCode, m_AccessoryInput.itemCode);
			}
		}
		
		if (t_ItemCode != 0)
		{
			t_AItem = new AdvencedItem(t_ItemCode, p_AItem.itemProgress, 1);
			m_AccessoryInput = null;
			RefreshPlate();
		}

		return t_AItem;
	}

	private void RefreshGraph()
	{
		if(m_MagicCircleMaterial != null)
		{
			if (m_MagicCircleMaterial.material != null)
			{
				m_MagicCircleMaterial.material.SetFloat("_Power5", curve.Evaluate(m_Elements[0]) * 50.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power3", curve.Evaluate(m_Elements[1]) * 50.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power1", curve.Evaluate(m_Elements[2]) * 50.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power2", curve.Evaluate(m_Elements[3]) * 50.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power4", curve.Evaluate(m_Elements[4]) * 50.0f);
			}
		}
		
		for (int i = 0; i < m_MagicCircleGraph.Count; i = i + 1)
		{
			m_MagicCircleGraph[i].transform.localScale = new Vector3(1.0f, m_Elements[i], 1.0f);
		}
	}

	public void RefreshPlate()
	{
		if (m_AccessorySprite != null)
		{
			m_AccessorySprite.sprite = UniFunc.FindSprite(m_AccessoryInput.itemCode);
			if(m_AccessoryInput.itemCode == 0) { m_AccessorySprite.sprite = null; }
		}
		//if (m_Text != null)
		//{
		//	m_Text.text = m_AccessoryInput.itemCode + "";
		//}

	}
	public void RefreshOutput()
	{
		if (m_OutputSprite != null)
		{
			m_OutputSprite.sprite = UniFunc.FindSprite(m_CraftedItem.itemCode);
			if (m_CraftedItem.itemCode == 0) { m_OutputSprite.sprite = null; }
		}
		//if (m_Text != null)
		//{
		//	m_Text.text = m_CraftedItem.itemCode + "";
		//}
	}
}
