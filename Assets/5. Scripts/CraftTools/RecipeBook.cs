using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct ItemRecipe
{
	public int itemCode;
	public float progress;
	public List<Ingredient> ingredients;

	public ItemRecipe(string p_String = null)
	{
		itemCode = 0;
		progress = 0.0f;
		ingredients = null;
	}
	public ItemRecipe(ItemRecipe p_ItemRecipe)
	{
		itemCode = p_ItemRecipe.itemCode;
		progress = p_ItemRecipe.progress;
		ingredients = new List<Ingredient>(p_ItemRecipe.ingredients);
	}
	public ItemRecipe(int p_ItemCode, float p_Progress, List<Ingredient> p_Ingredients)
	{
		itemCode = p_ItemCode;
		progress = p_Progress;
		ingredients = new List<Ingredient>(p_Ingredients);
	}

	public static implicit operator ItemRecipe(string p_String)
	{
		//ItemRecipe t_ItemRecipe = new ItemRecipe();
		//if (p_String != null)
		//{
		//	t_ItemRecipe = p_String;
		//}
		return new ItemRecipe();
	}

	public static implicit operator string(ItemRecipe p_ItemRecipe)
	{
		string t_String = null;
		if(p_ItemRecipe.itemCode != 0 || p_ItemRecipe.ingredients != null)
		{
			t_String = p_ItemRecipe.ToString();
		}
		return t_String;
	}
}

public class RecipeBook : MonoBehaviour
{
	private List<ItemRecipe> m_ItemRecipes = new List<ItemRecipe>();
	public RecipeBookUIScript m_RecipeBookUIScript;

	private void Start()
	{
		
	}

	public List<ItemRecipe> GetItemRecipes()
	{
		return new List<ItemRecipe>(m_ItemRecipes);
	}

	public void RegistItem(int p_ItemCode, float p_Progress, List<Ingredient> p_Ingredients)
	{
		if(m_RecipeBookUIScript != null)
		{
			m_RecipeBookUIScript.DisplayRegistPanel(p_ItemCode, p_Progress, p_Ingredients);
		}
	}

	public bool FindRecipe(int p_ItemCode)
	{
		if(m_ItemRecipes != null)
		{
			for (int i = 0; i < m_ItemRecipes.Count; i = i + 1)
			{
				if (m_ItemRecipes[i].itemCode == p_ItemCode) { return true; }
			}
		}
		return false;
	}

	public void AddRecipe(ItemRecipe p_ItemRecipe)
	{
		int count = 0;
		if (m_ItemRecipes != null)
		{
			for (int i = 0; i < m_ItemRecipes.Count; i = i + 1)
			{
				if (m_ItemRecipes[i].itemCode == p_ItemRecipe.itemCode) { m_ItemRecipes[i] = p_ItemRecipe; count = count + 1; break; }
			}
		}

		if (count < 1) { m_ItemRecipes = m_ItemRecipes == null ? new List<ItemRecipe>() : m_ItemRecipes; m_ItemRecipes.Add(p_ItemRecipe); }
	}
	public void AddRecipe(int p_ItemCode, float p_Progress, List<Ingredient> p_Ingredients)
	{
		AddRecipe(new ItemRecipe(p_ItemCode, p_Progress, p_Ingredients));
	}

	public ItemRecipe PopRecipeAt(int p_Index)
	{
		ItemRecipe t_ItemRecipe = m_ItemRecipes[p_Index];
		m_ItemRecipes.RemoveAt(p_Index);
		m_ItemRecipes.TrimExcess();
		return t_ItemRecipe;
	}

	public void CleanRecipeBook()
	{
		m_ItemRecipes.Clear();
		m_ItemRecipes.TrimExcess();
	}

	public void TakeItemRecipes(RecipeBook p_RecipeBook)
	{
		if (p_RecipeBook != null)
		{
			int count = p_RecipeBook.GetItemRecipes().Count;
			for (int i = 0; i < count; i = i + 1)
			{
				AddRecipe(p_RecipeBook.PopRecipeAt(p_RecipeBook.GetItemRecipes().Count - 1));
			}
			p_RecipeBook.CleanRecipeBook();
		}
	}
}