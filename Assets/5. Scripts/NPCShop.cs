using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public struct SalesItem
//{
//	[SerializeField] public Item reward;
//	[SerializeField] public List<Item> costs;
//	[SerializeField] public List<Item> unlockConditions;
//
//	public SalesItem(Item p_Reward, Item[] p_Costs, Item[] p_UConditions = null)
//	{
//		reward = p_Reward;
//		costs = new List<Item>(p_Costs);
//		unlockConditions = new List<Item>(p_UConditions);
//	}
//}

public class NPCShop : MonoBehaviour
{
	//[SerializeField] List<SalesItem> salesItems = new List<SalesItem>();
	public Inventory m_Inventory;

    // Start is called before the first frame update
    void Start()
    {
        if(m_Inventory == null)
		{
			m_Inventory = GetComponent<Inventory>();
		}
		//ResetInventory();
		//EventManager.Subscribe(EventType.Day, ResetInventory);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ResetInventory()
	{
		if (m_Inventory != null)
		{
			List<ShopItemData> t_ShopItemDatas = UniFunc.GetShopItemData();
			if (t_ShopItemDatas != null)
			{
				m_Inventory.CleanInventory();
				for (int i = 0; i < t_ShopItemDatas.Count; i = i + 1)
				{
					m_Inventory.AddAItem(t_ShopItemDatas[i].itemId, 1.0f, t_ShopItemDatas[i].buyValue, t_ShopItemDatas[i].sellValue);
				}
			}
		}
	}

	//public List<SalesItem> GetSalesItems()
	//{
	//	return new List<SalesItem>(salesItems);
	//}
}
