using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAccessoryPlate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool isSelect;
    private bool isActive;

    [SerializeField] private InteractionItem interactionItem;
    [SerializeField] private int itemID;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        EventManager.Subscribe(EventType.SalesSuccess, ResetPlate);
        EventManager.Subscribe(EventType.SalesFailure, ResetPlate);
    }

    private void OnMouseDown()
    {
        if (itemID == 0)
            return;
        
        if (GameManager.Instance.ItemManager.GetItemType(itemID) != ItemType.Jewelry)
        {
            FindObjectOfType<PlayerCharacter>().SetPlayerGrabItem(new AdvencedItem(itemID, 1, 1));
            isSelect = true;
            spriteRenderer.enabled = false;
            return;
        }
        
        var item = interactionItem.ItemInteraction(itemID);
        item.GetComponent<InteractionAccessory>().Init(itemID,this);
    }

    public void RewindPlate()
    {
        spriteRenderer.enabled = true;
    }
    
    public void ResetPlate()
    {
        itemID = 0;
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
    }

    public void SetAccessory(int accessoryID)
    {
        if (itemID != 0)
        {
            RewindPlate();
            return;
        }
            
        itemID = accessoryID;
        //transform.root.GetComponent<RavenCraftCore.Press>().SetAccessoryData(accessoryID);
        spriteRenderer.sprite = GameManager.Instance.ItemManager.GetItemSprite(accessoryID);
    }

    public int GetAccessory()
    {
        return itemID;
    }

    public void CompleteCraft(int completeItemID)
    {
        EventManager.Publish(EventType.CreateComplete);
        itemID = completeItemID;
        var completeItem = GameManager.Instance.ItemManager.GetBasicItemData(completeItemID);
        
        if (completeItem == null)
        {
            //실패 이펙트
            return;
        }

        //이펙트
        spriteRenderer.sprite = completeItem.itemResourceImage;
        spriteRenderer.enabled = true;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        if (!isSelect)
            return;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.SalesSuccess, ResetPlate);
        EventManager.Unsubscribe(EventType.SalesFailure, ResetPlate);
    }
}
