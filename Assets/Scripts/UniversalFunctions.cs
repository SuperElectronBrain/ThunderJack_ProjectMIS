using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalFunctions : MonoBehaviour
{

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
