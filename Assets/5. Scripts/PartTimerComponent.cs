using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartTimerComponent : MonoBehaviour
{
//	[HideInInspector] private CharacterBase master;
//	private Inventory masterInventory;
//	public ItemCode productionItem = ItemCode.RubyRing;
//	[SerializeField] private RecipeBook recipeBook;
//	[HideInInspector] public TextMeshProUGUI m_TMP;

//	[SerializeField] private float productionTime = 5.0f;
//	private float timeLeftForCompletion = 0.0f;
//	private bool workState = false;

//	// Start is called before the first frame update
//	void Start()
//	{
		
//	}

//	// Update is called once per frame
//	void Update()
//	{
//		float DeltaTime = Time.deltaTime;

//		if (workState == true)
//		{
//			timeLeftForCompletion = timeLeftForCompletion + DeltaTime;

//			if(m_TMP != null)
//			{
//				m_TMP.text = (int)(productionTime - timeLeftForCompletion) + "";
//			}

//			if (timeLeftForCompletion > productionTime)
//			{
//				CraftItem();
//				timeLeftForCompletion = 0;
//			}
//		}
//	}

//	public void SetMaster(CharacterBase p_Master)
//	{
//		master = p_Master;
//		if (p_Master != null)
//		{
//			masterInventory = p_Master.gameObject.GetComponent<Inventory>();
//		}
//		else if(p_Master == null)
//		{
//			masterInventory = null;
//		}
//	}

//	private void CraftItem()
//	{
//		if (master != null)
//		{
//			if (masterInventory != null)
//			{
//				if(recipeBook != null)
//				{
//					Debug.Log("î");

//					ItemRecipe itemRecipe = null;
//					List<ItemRecipe> recipes = new List<ItemRecipe>(recipeBook.Recipes);
//					for(int i = 0; i < recipes.Count; i = i + 1)
//					{
//						if (recipes[i].result.itemCode == productionItem)
//						{
//							itemRecipe = recipes[i];
//							break;
//						}
//					}

//					if(itemRecipe != null)
//					{
//						bool isCraftable = true;
//						for (int i = 0; i < itemRecipe.ingredients.Count; i = i + 1)
//						{
//							if (masterInventory.FindItem(itemRecipe.ingredients[i].itemCode, itemRecipe.ingredients[i].itemAmount) == false)
//							{
//								isCraftable = false;
//								break;
//							}
//						}

//						if(isCraftable == true)
//						{
//							for (int i = 0; i < itemRecipe.ingredients.Count; i = i + 1)
//							{
//								masterInventory.PopItem(itemRecipe.ingredients[i].itemCode, itemRecipe.ingredients[i].itemAmount);
//							}

//							masterInventory.AddItem(itemRecipe.result);
//						}
//					}
//				}
//			}
//		}
//	}

//	public void StartProduction()
//	{
//		if (master != null)
//		{
//			if (masterInventory != null)
//			{
//				if (workState == false)
//				{
//					workState = true;
//					timeLeftForCompletion = 0;
//				}
//			}
//		}
//	}

//	public void EndProduction()
//	{
//		if (master != null)
//		{
//			if (masterInventory != null)
//			{
//				if (workState == true)
//				{
//					workState = false;
//					timeLeftForCompletion = 0;
//				}
//			}
//		}
//	}
}
