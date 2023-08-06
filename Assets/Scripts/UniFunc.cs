using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFunc : MonoBehaviour
{
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

	public static T GetChildOfType<T>(Transform go) where T : Object
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

	public static T GetChildOfType<T>(GameObject go) where T : Object { return GetChildOfType<T>(go.transform); }

	public static List<T> GetChildsOfType<T>(Transform go) where T : Object
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

	public static List<T> GetChildsOfType<T>(GameObject go) where T : Object { return GetChildsOfType<T>(go.transform); }
}
