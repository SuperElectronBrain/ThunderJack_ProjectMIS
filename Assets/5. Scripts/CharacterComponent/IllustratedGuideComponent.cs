using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IllustratedGuideComponent : MonoBehaviour
{
	public int this[int index]
	{ 
		get 
		{
			int value = 0;
			if(items.Count > index)
			{
				value = items[index];
			}
			return value;
		} 
		set {; } 
	}
	[SerializeField] private List<int> items = new List<int>();
	[HideInInspector] public IllustratedGuideUIScript m_IllustratedGuideUIScript;

	public static IllustratedGuideComponent main
	{
		get
		{
			IllustratedGuideComponent t_IllustratedGuideComponent = null;
			PlayerCharacter t_PC = FindObjectOfType<PlayerCharacter>();
			if (t_PC != null)
			{
				t_IllustratedGuideComponent = t_PC.GetComponent<IllustratedGuideComponent>();
			}
			return t_IllustratedGuideComponent;
		}
		set {; }
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	public int GetItemCount()
	{
		int count = 0;
		if (items != null) { items.TrimExcess(); count = items.Count; }
		return count;
	}

	public bool FindItem(int itemCode)
	{
		if(items!= null)
		{
			if(items.Count > 0)
			{
				if(itemCode == items.Find((int x) => { return x == itemCode; }))
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool RegistItem(int itemCode) 
	{
		if(items == null) { items = new List<int>(); }
		if (items != null)
		{
			if (itemCode != items.Find((int x) => { return x == itemCode; }))
			{
				items.Add(itemCode);
				items.Sort((int a, int b) => { return (a < b) ? -1 : 1; });
				if(m_IllustratedGuideUIScript != null) { m_IllustratedGuideUIScript.RefresfAction(); }
				return true;
			}
		}
		return false;
	}

	public void TakeData(IllustratedGuideComponent p_IllustratedGuide)
	{
		if (p_IllustratedGuide != null)
		{
			int count = p_IllustratedGuide.items.Count;
			for (int i = 0; i < count; i = i + 1)
			{
				RegistItem(p_IllustratedGuide.items[p_IllustratedGuide.items.Count - 1]);
				p_IllustratedGuide.items.Remove(p_IllustratedGuide.items.Count - 1);
				p_IllustratedGuide.items.TrimExcess();
			}
			p_IllustratedGuide.items.Clear();
			p_IllustratedGuide.items.TrimExcess();
		}
	}
}
