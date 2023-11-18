using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public enum AccessoryColor
{
    N, R, B, G, Y, F, V
}

public class InteractionItem : MonoBehaviour
{
    public int itemID;
    [SerializeField]
    GameObject interactionMaterial;
    [SerializeField]
    GameObject interactionAccessory;
    SkeletonAnimation skAni;

    private GameObject returnItem;

    private void Start()
    {
        interactionMaterial = AddressableManager.LoadObject<GameObject>("InteractionMaterial");
        interactionMaterial = AddressableManager.LoadObject<GameObject>("InteractionMaterial");
        interactionMaterial = AddressableManager.LoadObject<GameObject>("InteractionMaterial");
        interactionAccessory = AddressableManager.LoadObject<GameObject>("InteractionAccessory");
        interactionMaterial = AddressableManager.LoadObject<GameObject>("InteractionMaterial");
    }
    
    public GameObject ItemInteraction(int itemID)
    {
        this.itemID = itemID;
        ItemType itemType = GameManager.Instance.ItemManager.GetItemType(itemID);
        returnItem = null;
        switch (itemType)
        {
            case ItemType.Materials:
                returnItem = Instantiate(interactionMaterial);
                Sprite itemSprite = AddressableManager.LoadObject<Sprite>(GameManager.Instance.ItemManager.GetBasicItemData(itemID).particlesName);
                for (int i = 0; i < 3; i++)
                {
                    returnItem.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = itemSprite;
                    //returnItem.transform.GetChild(i).gameObject.AddComponent<CircleCollider2D>();
                }

                AAA t_AAA = returnItem.GetComponent<AAA>();
                if(t_AAA != null)  { t_AAA.m_ItemCode = itemID; }

				break;
            case ItemType.Accessory:
            case ItemType.Jewelry:               
                StartCoroutine(IsDataSetting());
                returnItem.GetComponent<InteractionAccessory>().Init(itemID);
                break;
            default: 
                return returnItem;
        } 
        return returnItem;
    }

    IEnumerator IsDataSetting()
    {
        returnItem = Instantiate(interactionAccessory);
        var itemData = GameManager.Instance.ItemManager.GetBasicItemData(itemID);
        skAni = returnItem.GetComponent<SkeletonAnimation>();
        skAni.skeletonDataAsset =
            AddressableManager.LoadObject<SkeletonDataAsset>(itemData.particlesName);
        yield return new WaitUntil(() => skAni.skeletonDataAsset != null);
        skAni.Initialize(true);
        skAni.skeleton.SetSkin(itemData.accessoryColor);
        skAni.skeleton.SetToSetupPose();
    }
}
