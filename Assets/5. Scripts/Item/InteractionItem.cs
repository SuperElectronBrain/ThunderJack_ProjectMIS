using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{
    public int itemID;
    GameObject interactionItem;

    private void Start()
    {
        interactionItem = AddressableManager.LoadObject<GameObject>("InteractionItem");
    }
    
    public void ItemInteraction(int itemID)
    {
        ItemType itemType = GameManager.Instance.ItemManager.GetItemType(itemID);
        switch (itemType)
        {
            case ItemType.Materials:
                break;
            case ItemType.Accessory:
            case ItemType.Jewelry:
                break;
            default:
                return;
        }

        Sprite itemSprite = AddressableManager.LoadObject<Sprite>(GameManager.Instance.ItemManager.GetBasicItemData(itemID).particlesName);

        GameObject item = Instantiate(interactionItem);

        for(int i = 0; i < 3; i++)
        {
            item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = itemSprite;
            item.transform.GetChild(i).gameObject.AddComponent<PolygonCollider2D>();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        { 
            ItemInteraction(itemID);            
        }
    }
}
