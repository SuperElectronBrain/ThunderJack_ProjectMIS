using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFunc : MonoBehaviour
{
	public static T GetParentComponent<T>(Transform go) where T : Object
	{
		Transform t_Parent = go.parent;
		T t_ParentComponent = null;
		int count = 0;
		while(count < 1)
		{
			if(t_Parent == null)
			{
				count = count + 1;
				break;
			}
			else
			{
				t_ParentComponent = t_Parent.GetComponent<T>();
			}

			if (t_ParentComponent != null)
			{
				count = count + 1;
				break;
			}
			else 
			{
				t_Parent = t_Parent.parent;
			}
		}

		return t_ParentComponent;
	}

	public static T GetParentComponent<T>(GameObject go) where T : Object { return GetParentComponent<T>(go.transform); }

	/// <summary>
	/// Do not using on Update.
	/// </summary>
	/// <param name="go"></param>
	/// <param name="p_Name"></param>
	/// <returns></returns>
	public static GameObject GetChildOfName(Transform go, string p_Name)
	{
		if(go != null)
		{
			if(go.name == p_Name)
			{
				return go.gameObject;
			}
			else 
			{
				GameObject t_Go = null;
				for (int i = 0; i < go.childCount; i = i + 1)
				{
					t_Go = GetChildOfName(go.GetChild(i), p_Name);

					if(t_Go != null)
					{
						break;
					}
				}
				return t_Go;
			}
		}
		else 
		{ 
			return null;
		}
	}

	/// <summary>
	/// Do not using on Update.
	/// </summary>
	/// <param name="go"></param>
	/// <param name="p_Name"></param>
	/// <returns></returns>
	public static GameObject GetChildOfName(GameObject go, string p_Name) { return GetChildOfName(go.transform, p_Name); }

	public static T GetChildComponent<T>(Transform go) where T : Object
	{
		if (go != null)
		{
			for (int i = 0; i < go.childCount; i = i + 1)
			{
				T temp = go.GetChild(i).GetComponent<T>();
				if (temp != null)
				{
					return temp;
				}
			}
		}

		return null;
	}

	public static T GetChildComponent<T>(GameObject go) where T : Object { return GetChildComponent<T>(go.transform); }

	public static List<T> GetChildsComponent<T>(Transform go) where T : Object
	{
		List<T> tempList = null;

		if (go != null)
		{
			tempList = new List<T>();
			for (int i = 0; i < go.childCount; i = i + 1)
			{
				T temp = go.GetChild(i).GetComponent<T>();
				if (temp != null)
				{
					tempList.Add(temp);
				}
			}

			if (tempList.Count <= 0) { tempList = null; }
		}

		return tempList;
	}

	public static List<T> GetChildsComponent<T>(GameObject go) where T : Object { return GetChildsComponent<T>(go.transform); }

	public static List<Item> AddItem(List<Item> p_Items, Item p_Item)
	{
		int count = 0;
		for (int i = 0; i < p_Items.Count; i = i + 1)
		{
			if (p_Items[i].itemCode == p_Item.itemCode)
			{
				Item temp = p_Items[i];
				temp.itemAmount = temp.itemAmount + p_Item.itemAmount;
				p_Items[i] = temp;

				count = count + 1;
				break;
			}
		}

		if (count < 1)
		{
			p_Items.Add(p_Item);
			p_Items.Sort((a, b) => { return (a.itemCode < b.itemCode) ? -1 : 1; });
		}

		return p_Items;
	}

	public static List<Item> AddItem(List<Item> p_Items, ItemCode p_ItemCode, int p_ItemAmount) { return AddItem(p_Items, new Item(p_ItemCode, p_ItemAmount)); }

	public static List<GemRecipe> FindRecipesOfElement(List<GemRecipe> p_GemRecipes, int p_OrderNumber, int p_Element)
	{
		List<GemRecipe> t_GemRecipes = null;

		if (p_GemRecipes != null)
		{
			for (int i = 0; i < p_GemRecipes.Count; i = i + 1)
			{
				int t_Material = -1;
				if (p_OrderNumber == 1) { t_Material = p_GemRecipes[i].material1; }
				else if (p_OrderNumber == 2) { t_Material = p_GemRecipes[i].material2; }
				else if (p_OrderNumber == 3) { t_Material = p_GemRecipes[i].material3; }
				if (t_Material != -1)
				{
					if (t_Material == p_Element)
					{
						if (t_GemRecipes == null) { t_GemRecipes = new List<GemRecipe>(); }
						t_GemRecipes.Add(p_GemRecipes[i]);
					}
				}
			}
		}

		return t_GemRecipes;
	}

	public static Sprite FindSprite(string p_ItemCode)
	{
		if(p_ItemCode == "1")
		{
			p_ItemCode = "Icon";
		}
		else if(p_ItemCode == "2")
		{
			p_ItemCode = "Icon_Item_Flower_2";
		}
		else if(p_ItemCode == "3")
		{
			p_ItemCode = "Icon_Item_Flower_3";
		}
		else if (p_ItemCode == "4")
		{
			p_ItemCode = "Icon_Item_Flower_4";
		}
		else if (p_ItemCode == "5")
		{
			p_ItemCode = "Icon_Item_Flower_5";
		}
		else if (p_ItemCode == "16")
		{
			p_ItemCode = "Icon_Item_gem_1";
		}
		else if (p_ItemCode == "17")
		{
			p_ItemCode = "Icon_Item_gem_2";
		}
		else if (p_ItemCode == "18")
		{
			p_ItemCode = "Icon_Item_gem_3";
		}
		else if (p_ItemCode == "19")
		{
			p_ItemCode = "Icon_Item_gem_4";
		}
		else if (p_ItemCode == "20")
		{
			p_ItemCode = "Icon_Item_gem_5";
		}
		else if (p_ItemCode == "22")
		{
			p_ItemCode = "Icon_Item_Accessory_1";
		}
		else if (p_ItemCode == "23")
		{
			p_ItemCode = "Icon_Item_Accessory_2";
		}
		else if (p_ItemCode == "24")
		{
			p_ItemCode = "Icon_Item_Accessory_3";
		}
		else if (p_ItemCode == "25")
		{
			p_ItemCode = "Icon_Item_Accessory_4";
		}
		else if (p_ItemCode == "26")
		{
			p_ItemCode = "Icon_Item_Accessory_5";
		}
		else if (p_ItemCode == "27")
		{
			p_ItemCode = "Icon_Item_Accessory_6";
		}

		return AddressableManager.LoadObject<Sprite>(p_ItemCode);
	}
}
