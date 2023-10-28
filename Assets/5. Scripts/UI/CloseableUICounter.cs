using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseableUICounter : MonoBehaviour
{
	[HideInInspector] public List<UICloserComponent> UICloserComponents = new List<UICloserComponent>();

	// Start is called before the first frame update
	//void Start()
	//{
	//    
	//}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	public bool AddOpenUI(UICloserComponent param)
	{
		if(UICloserComponents != null)
		{
			if(param != UICloserComponents.Find((UICloserComponent x) => { return x == param; }))
			{
				UICloserComponents.Add(param);
				return true;
			}
		}
		return false;
	}

	public bool RemoveCloseUI(UICloserComponent param)
	{
		bool bSuccessed = false;
		if (UICloserComponents != null)
		{
			bSuccessed = UICloserComponents.Remove(param);
			UICloserComponents.TrimExcess();
		}
		return bSuccessed;
	}

	public UICloserComponent GetRecentlyOpenedUI()
	{
		UICloserComponent t_UICloserComponent = null;
		if (UICloserComponents != null)
		{
			if (UICloserComponents.Count > 0)
			{
				t_UICloserComponent = UICloserComponents[UICloserComponents.Count - 1];
			}
		}
		return t_UICloserComponent;
	}

	public static CloseableUICounter GetCloseableUICounter()
	{
		CloseableUICounter t_CloseableUICounter = FindObjectOfType<CloseableUICounter>();
		if (t_CloseableUICounter == null)
		{
			GameObject t_GO = new GameObject();
			t_CloseableUICounter = t_GO.AddComponent<CloseableUICounter>();
		}
		return t_CloseableUICounter;
	}
}
