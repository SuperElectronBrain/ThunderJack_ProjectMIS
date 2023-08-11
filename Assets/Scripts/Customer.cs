using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public struct Need
{
	[SerializeField] public Item need;
	[SerializeField] public Item reward;

	public Need(Item p_Need, Item p_Reward)
	{
		need = p_Need;
		reward = p_Reward;
	}
}

public class Customer : MonoBehaviour
{
	[SerializeField] private List<Need> needs = new List<Need>();

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public List<Need> GetNeeds()
	{
		return new List<Need>(needs);
	}

	public void CompleteNeed(ItemCode p_Need)
	{
		for(int i = 0; i < needs.Count; i = i + 1)
		{
			if (needs[i].need.itemCode == p_Need)
			{
				needs.RemoveAt(i);

				break;
			}
		}
	}
}
