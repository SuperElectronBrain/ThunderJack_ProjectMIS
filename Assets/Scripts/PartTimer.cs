using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTimer : Character
{
    [HideInInspector] public Character master;
	private Inventory masterInventory;
	public ItemCode productionItem;
	[SerializeField] private RecipeBook recipeBook;

	[SerializeField] private float productionTime = 60.0f;
    [SerializeField] private float timeLeftForCompletion = 0.0f;
	private bool workState = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (workState == true)
		{
			timeLeftForCompletion = timeLeftForCompletion + DeltaTime;
			if (timeLeftForCompletion > productionTime)
			{
				CraftItem();
				timeLeftForCompletion = 0;
			}
		}
	}

	public void SetMaster(Character p_Master)
	{
		master = p_Master;
		if (p_Master != null)
		{
			masterInventory = p_Master.gameObject.GetComponent<Inventory>();
		}
		else if(p_Master == null)
		{
			masterInventory = null;
		}
	}

	private void CraftItem()
	{
		if (master != null)
		{
			if (masterInventory != null)
			{
				if(recipeBook != null)
				{
					List<ItemRecipe> tempRecipes = new List<ItemRecipe>(recipeBook.Recipes);
					for(int i = 0; i < tempRecipes.Count; i = i + 1)
					{
						if (tempRecipes[i].result.itemCode == productionItem)
						{
							break;
						}
					}
				}
			}
		}
	}

	public void StartProduction()
	{
		if (master != null)
		{
			if (masterInventory != null)
			{
				if (workState == false)
				{
					workState = true;
					timeLeftForCompletion = 0;
				}
			}
		}
	}

	public void EndProduction()
	{
		if (master != null)
		{
			if (masterInventory != null)
			{
				if (workState == true)
				{
					workState = false;
					timeLeftForCompletion = 0;
				}
			}
		}
	}
}
