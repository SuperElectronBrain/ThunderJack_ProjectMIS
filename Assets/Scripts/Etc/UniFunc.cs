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
}
