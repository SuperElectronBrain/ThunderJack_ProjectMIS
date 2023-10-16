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

        entity.SkAni.AnimationName = "A_idle_F";

        GetComponent<NPCShop>().enabled = true;

        List<ShopItemData> shopItems = null;

        switch(entity.CharacterData.characterEgName)
        {
            case "Gaga":
                shopItems = GameManager.Instance.ItemManager.GetShopItemDataBySalesType(SalesItemType.Accessory);
                break;
            case "Beil":
                shopItems = GameManager.Instance.ItemManager.GetShopItemDataBySalesType(SalesItemType.Materials);
                break;
        }

        Inventory inventory = GetComponent<Inventory>();

        inventory.CleanInventory();
        for (int i = 0; i < shopItems.Count; i = i + 1)
        {
            inventory.AddAItem(shopItems[i].itemId, 1.0f, shopItems[i].buyValue, shopItems[i].sellValue);
        }

        //for (int i = 0; i < shopItems.Count; i++)
        //{
        //    AdvencedItem newItem = new AdvencedItem
        //    {
        //        itemCode = shopItems[i].itemId,
        //        itemProgress = 1
        //    };
        //    inventory.AddAItem(newItem);
        //}
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
