using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class InteractionItem : MonoBehaviour
{
    public int itemID;
    GameObject interactionMaterial;
    GameObject interactionAccessory;
    SkeletonAnimation skAni;

    private void Start()
    {
        interactionMaterial = AddressableManager.LoadObject<GameObject>("InteractionMaterial");
        interactionAccessory = AddressableManager.LoadObject<GameObject>("InteractionAccessory");
    }
    
    public void ItemInteraction(int itemID)
    {
        ItemType itemType = GameManager.Instance.ItemManager.GetItemType(itemID);
        GameObject item;
        switch (itemType)
        {
            case ItemType.Materials:
                item = Instantiate(interactionMaterial);
                Sprite itemSprite = AddressableManager.LoadObject<Sprite>(GameManager.Instance.ItemManager.GetBasicItemData(itemID).particlesName);
                for (int i = 0; i < 3; i++)
                {
                    item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = itemSprite;
                    item.transform.GetChild(i).gameObject.AddComponent<CircleCollider2D>();
                }

                AAA t_AAA = item.GetComponent<AAA>();
                if(t_AAA != null)  { t_AAA.m_ItemCode = itemID; }

				break;
            case ItemType.Accessory:
            case ItemType.Jewelry:
                item = Instantiate(interactionAccessory);
                //item.GetComponent<SkeletonAnimation>().Animat
                break;
            default:
                return;
        }                        
    }
}
