using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTimeState : State<NPC>
{
    private bool isInit;
    
    public override void Enter(NPC entity)
    {
        entity.agent.isStopped = true;
        entity.agent.enabled = false;
        entity.gameObject.transform.position = entity.destinationPos;
        entity.isSales = true;
        entity.curBehaviour = NPCBehaviour.Business;

        entity.SkAni.AnimationName = "A_idle_F";

        GetComponent<NPCShop>().enabled = true;

        List<ShopItemData> shopItems = null;

        switch(entity.CharacterData.characterEgName)
        {
            case "Harry":
                entity.shopDialog = "Furniture_Shop_Text_Master";
                shopItems = GameManager.Instance.ItemManager.GetShopItemDataBySalesType(SalesItemType.Accessory);
                break;
            case "Dokan":
                entity.shopDialog = "Material_Shop_Text_Master";
                shopItems = GameManager.Instance.ItemManager.GetShopItemDataBySalesType(SalesItemType.Materials);
                break;
        }

        /*Todo
         
         Init 함수 만들고 하루가 끝나면  isInit false로 바꿔주기
         
         */
        
        if (isInit)
        {
            Inventory inventory = GetComponent<Inventory>();

            inventory.CleanInventory();
            for (int i = 0; i < shopItems.Count; i = i + 1)
            {
                inventory.AddAItem(shopItems[i].itemId, 1.0f, shopItems[i].buyValue, shopItems[i].sellValue);
            }

            isInit = true;
        }
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        entity.agent.enabled = true;
        entity.agent.isStopped = false;
        entity.isSales = false;
        entity.prevBehaviour = NPCBehaviour.Business;
    }

    public override void OnTransition(NPC entity)
    {
        
    }
}