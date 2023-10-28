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
    private int itemID;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!isActive)
            return;
        
        isSelect = true;
        var item = interactionItem.ItemInteraction(itemID);
        item.GetComponent<InteractionAccessory>().Init(this);
        spriteRenderer.enabled = false;
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

    public void CompleteCraft(int completeItemID)
    {
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
}
