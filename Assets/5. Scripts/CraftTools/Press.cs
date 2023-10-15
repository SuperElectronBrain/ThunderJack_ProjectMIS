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
	[HideInInspector] public List<Ingredient> m_Ingredients = new List<Ingredient>();
	[HideInInspector] public float[] m_Elements = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
	[HideInInspector] public AdvencedItem m_AccessoryInput = new AdvencedItem();
	[HideInInspector] public AdvencedItem m_CraftedItem = new AdvencedItem();
	private Vector3 m_PreviousHandlePosition;
	[SerializeField] private float m_HandleMoveRange = 1.0f;

	[SerializeField] private GameObject m_PressHandleBase;
	[SerializeField] private Transform m_HandleDirection;
	[SerializeField] private List<GameObject> m_MagicCircleGraph;
	[SerializeField] private List<ParticleSystem> m_MagicCircleVFXs;
	[SerializeField] private ParticleSystem m_CompleteVFX;
	[SerializeField] private ParticleSystem m_FailVFX;
	[SerializeField] private ParticleSystem m_SmokeVFX;
	[SerializeField] private ParticleSystem m_MagicCircleResetVFX;
	[SerializeField] private SpriteRenderer m_MagicCircleMaterial;
	[SerializeField] private SpriteRenderer m_AccessorySprite;
	//[SerializeField] private SpriteRenderer m_OutputSprite;
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
			trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "Activate", false);
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

		m_CompleteVFX.Stop();
		m_FailVFX.Stop();
		m_SmokeVFX.Stop();
		m_MagicCircleResetVFX.loop = false;
		m_MagicCircleResetVFX.Stop();
		RefreshMagicCircleEffect();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) == true)
		{
			m_PreviousHandlePosition = Input.mousePosition;
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
			if (count > 0 || m_AccessoryInput != null)
			{
				Vector3 t_HandleDirection = m_HandleDirection.transform.forward;
				t_HandleDirection.z = 0.0f;
				Vector3 t_CurrentHandlePosition = Input.mousePosition;
				Vector3 t_HandleMovement = (t_CurrentHandlePosition - m_PreviousHandlePosition);
				t_HandleMovement.z = 0.0f;
				float t_Progress = Vector3.Dot(t_HandleDirection, t_HandleMovement);
				if (t_Progress != 0.0f)
				{
					m_Progress = m_Progress + (t_Progress / m_HandleMoveRange);
					m_Progress = m_Progress > 1.0f ? 1.0f : (m_Progress < -1.0f ? -1.0f : m_Progress);
					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 1.0f; }
				}
				else if (t_Progress == 0)
				{
					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}

				if (m_Progress >= 1.0f)
				{
					m_AccessoryInput = CraftItem(CraftGem());

					if(m_CraftedItem.itemCode != 0)
					{
						if (m_Inventory != null)
						{
							if (m_Inventory.m_Owner != null)
							{
								PlayerCharacter t_PlayerCharacter = m_Inventory.m_Owner as PlayerCharacter;
								if(t_PlayerCharacter != null)
								{
									if (t_PlayerCharacter.m_RecipeBook != null)
									{
										m_Ingredients.Add(new Ingredient(m_AccessoryInput.itemCode, m_AccessoryInput.itemProgress));
										t_PlayerCharacter.m_RecipeBook.RegistItem(m_CraftedItem.itemCode, m_CraftedItem.itemProgress, m_Ingredients);
									}
								}
							}
						}
					}

					for (int i = 0; i < m_Elements.Length; i = i + 1) { m_Elements[i] = 0.0f; }
					for (int i = 0; i < m_Ingredients.Count; i = i + 1) { m_Ingredients[i] = null; }
					//m_AccessoryInput = null;

					RefreshGraph();
					RefreshPlate();
					RefreshOutput();
					RefreshMagicCircleEffect();
					m_MagicCircleResetVFX.Stop();
					m_MagicCircleResetVFX.Play();

					m_Progress = 0.0f;
					bProgress = false;

					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}
				else if (m_Progress <= -1.0f)
				{
					for (int i = 0; i < m_Elements.Length; i = i + 1) { m_Elements[i] = 0.0f; }
					for (int i = 0; i < m_Ingredients.Count; i = i + 1) { m_Ingredients[i] = null; }

					if (m_Inventory != null)
					{
						if(m_AccessoryInput != null)
						{
							m_Inventory.AddAItem(m_AccessoryInput);
							m_AccessoryInput = null;
						}
					}
					RefreshGraph();
					RefreshPlate();
					RefreshOutput();
					RefreshMagicCircleEffect();
					m_MagicCircleResetVFX.Stop();
					m_MagicCircleResetVFX.Play();
				}

				m_PreviousHandlePosition = t_CurrentHandlePosition;
			}
		}
		
		if (trackEntry != null)
		{
			float t_AnimationProgress = (m_Progress + 1) / 2;
			trackEntry.TrackTime = t_AnimationProgress * trackEntry.AnimationEnd;
			/*
			float t_AnimationProgress = ((m_Progress >= 0 ? m_Progress : -m_Progress) + 1) / 2;
			if (m_Progress >= 0)
			{
				if (trackEntry.Animation.Name != "Activate")
				{ 
					if (m_SkeletonAnimation != null)
					{
						trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "Activate", false);
					}
				}
				trackEntry.TrackTime = t_AnimationProgress * trackEntry.AnimationEnd;
			}
			else if (m_Progress < 0)
			{
				if (trackEntry.Animation.Name != "End")
				{
					if (m_SkeletonAnimation != null) 
					{
						trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "End", false);
					}
				}
				trackEntry.TrackTime = t_AnimationProgress * trackEntry.AnimationEnd;
			}
			*/
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
						float t_PrevElement1 = m_Elements[t_MaterialItemData.elementType1 - 1];
						float t_PrevElement2 = m_Elements[t_MaterialItemData.elementType2 - 1];

						m_Elements[t_MaterialItemData.elementType1 - 1] = m_Elements[t_MaterialItemData.elementType1 - 1] + (t_MaterialItemData.elementPercent1 * 0.01f * p_Progress);
						m_Elements[t_MaterialItemData.elementType2 - 1] = m_Elements[t_MaterialItemData.elementType2 - 1] + (t_MaterialItemData.elementPercent2 * 0.01f * p_Progress);
						//m_Elements[t_MaterialItemData.elementType3 - 1] = m_Elements[t_MaterialItemData.elementType3 - 1] + (t_MaterialItemData.elementPercent3 * 0.01f * p_Progress);
						
						
						//0
						if (t_PrevElement1 <= 0.0f && m_Elements[t_MaterialItemData.elementType1 - 1] > 0.0f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1].Play();
						}
						if (t_PrevElement2 <= 0.0f && m_Elements[t_MaterialItemData.elementType2 - 1] > 0.0f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1].Play();
						}

						//20
						if (t_PrevElement1 <= 0.2f && m_Elements[t_MaterialItemData.elementType1 - 1] > 0.2f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 5].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 5].Play();
						}
						if (t_PrevElement2 <= 0.2f && m_Elements[t_MaterialItemData.elementType2 - 1] > 0.2f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 5].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 5].Play();
						}

						//40
						if (t_PrevElement1 <= 0.4f && m_Elements[t_MaterialItemData.elementType1 - 1] > 0.4f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 5].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 10].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 10].Play();
						}
						if (t_PrevElement2 <= 0.4f && m_Elements[t_MaterialItemData.elementType2 - 1] > 0.4f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 5].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 10].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 10].Play();
						}

						//60
						if (t_PrevElement1 <= 0.6f && m_Elements[t_MaterialItemData.elementType1 - 1] > 0.6f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 10].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 15].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 15].Play();
						}
						if (t_PrevElement2 <= 0.6f && m_Elements[t_MaterialItemData.elementType2 - 1] > 0.6f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 10].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 15].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 15].Play();
						}

						//80
						if (t_PrevElement1 <= 0.8f && m_Elements[t_MaterialItemData.elementType1 - 1] > 0.8f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 15].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 20].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType1 - 1 + 20].Play();
						}
						if (t_PrevElement2 <= 0.8f && m_Elements[t_MaterialItemData.elementType2 - 1] > 0.8f)
						{
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 15].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 20].Stop();
							m_MagicCircleVFXs[t_MaterialItemData.elementType2 - 1 + 20].Play();
						}
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
		RefreshGraph();
		return t_AItem;
	}

	public AdvencedItem CraftItem(AdvencedItem p_AItem)
	{
		AdvencedItem t_AItem = null;

		int t_ItemCode = 21;
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.ItemManager != null)
			{
				t_ItemCode = GameManager.Instance.ItemManager.GetCombinationItem(p_AItem.itemCode, m_AccessoryInput.itemCode);

				if(t_ItemCode == -1)
				{
					t_ItemCode = 21;
				}
			}
		}

		m_SmokeVFX.Stop();
		m_SmokeVFX.Play();

		if (t_ItemCode != 21)
		{
			t_AItem = new AdvencedItem(t_ItemCode, p_AItem.itemProgress, 1);
			m_AccessoryInput = null;

			m_CompleteVFX.Stop();
			m_CompleteVFX.Play();
		}
		else if (t_ItemCode == 21)
		{
			m_FailVFX.Stop();
			m_FailVFX.Play();
		}

		RefreshGraph();
		RefreshPlate();
		RefreshOutput();
		RefreshMagicCircleEffect();
		m_MagicCircleResetVFX.Stop();
		m_MagicCircleResetVFX.Play();
		return t_AItem;
	}

	private void RefreshGraph()
	{
		if(m_MagicCircleMaterial != null)
		{
			if (m_MagicCircleMaterial.material != null)
			{
				m_MagicCircleMaterial.material.SetFloat("_Power5", (1 - curve.Evaluate(m_Elements[0])) * 30.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power3", (1 - curve.Evaluate(m_Elements[1])) * 30.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power1", (1 - curve.Evaluate(m_Elements[2])) * 30.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power2", (1 - curve.Evaluate(m_Elements[3])) * 30.0f);
				m_MagicCircleMaterial.material.SetFloat("_Power4", (1 - curve.Evaluate(m_Elements[4])) * 30.0f);
			}
		}
		
		//for (int i = 0; i < m_MagicCircleGraph.Count; i = i + 1)
		//{
		//	m_MagicCircleGraph[i].transform.localScale = new Vector3(1.0f, m_Elements[i], 1.0f);
		//}
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
		//if (m_OutputSprite != null)
		//{
		//	m_OutputSprite.sprite = UniFunc.FindSprite(m_CraftedItem.itemCode);
		//	if (m_CraftedItem.itemCode == 0) { m_OutputSprite.sprite = null; }
		//}
		RefreshPlate();
		//if (m_Text != null)
		//{
		//	m_Text.text = m_CraftedItem.itemCode + "";
		//}
	}

	public void RefreshMagicCircleEffect()
	{
		if(m_MagicCircleVFXs != null)
		{
			for (int i = 0; i < m_MagicCircleVFXs.Count; i = i + 1)
			{
				if (m_MagicCircleVFXs[i] != null)
				{
					m_MagicCircleVFXs[i].Stop();
				}
			}
		}
	}
}
