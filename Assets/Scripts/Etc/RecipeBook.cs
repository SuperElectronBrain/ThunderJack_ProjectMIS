using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/RecipeBook", fileName = "RecipeBook", order = int.MinValue)]
public class RecipeBook : ScriptableObject
{
	[SerializeField] public List<ItemRecipe> Recipes = new List<ItemRecipe>();
}
