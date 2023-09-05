using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item Recipe", fileName = "Item Recipe", order = int.MinValue)]
public class ItemRecipe : ScriptableObject
{
	[SerializeField] public List<Item> ingredients = new List<Item>();
	[SerializeField] public Item result;
}
