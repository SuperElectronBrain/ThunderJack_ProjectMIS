using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessState : State<NPC>
{
    public override void Enter(NPC entity)
    {
        entity.agent.isStopped = true;
        entity.agent.enabled = false;
        entity.gameObject.transform.position = entity.destinationPos;

        GetComponent<NPCShop>().enabled = true;

        var shopItems = GameManager.Instance.ItemManager.GetShopItemDataBySalesType(SalesItemType.Materials);

        Inventory inventory = GetComponent<Inventory>();

        for(int i = 0; i < shopItems.Count; i++)
        {
            AdvencedItem newItem = new AdvencedItem
            {
                itemCode = shopItems[i].itemId,
                itemAmount = 99,
                itemProgress = 1
            };
            inventory.AddAItem(newItem);
        }        
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        entity.agent.enabled = true;
        entity.agent.isStopped = false;
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}
