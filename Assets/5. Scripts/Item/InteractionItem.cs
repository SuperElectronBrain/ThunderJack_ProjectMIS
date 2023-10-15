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

    [SerializeField]
    bool isActive;

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

    RaycastHit2D hit;
    Ray2D ray;

    private void Update()
    {
        if(!isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ItemInteraction(itemID);
            }
        }
        else
        {
            var mPos = Input.mousePosition;
            mPos = Camera.main.ScreenToWorldPoint(mPos);
            ray = new Ray2D(mPos, Vector2.zero);

            int layer = 1 << LayerMask.GetMask("MaterialObj1");

            hit = Physics2D.Raycast(mPos, ray.direction, Mathf.Infinity);

            if (hit)
            {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
        
    }
}
