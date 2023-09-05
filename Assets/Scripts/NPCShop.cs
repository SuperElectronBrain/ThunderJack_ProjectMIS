using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SalesItem
{
	[SerializeField] public Item reward;
	[SerializeField] public List<Item> costs;
	[SerializeField] public List<Item> unlockConditions;

	public SalesItem(Item p_Reward, Item[] p_Costs, Item[] p_UConditions = null)
	{
		reward = p_Reward;
		costs = new List<Item>(p_Costs);
		unlockConditions = new List<Item>(p_UConditions);
	}
}

public class NPCShop : MonoBehaviour
{
	[SerializeField] List<SalesItem> salesItems = new List<SalesItem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public List<SalesItem> GetSalesItems()
	{
		return new List<SalesItem>(salesItems);
	}
}
