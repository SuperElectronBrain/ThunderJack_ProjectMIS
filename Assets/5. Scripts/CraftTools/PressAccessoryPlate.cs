using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAccessoryPlate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetPress()
    {
        spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;
    }

    public void CompleteCraft(int completeItemID)
    {
        var completeItem = GameManager.Instance.ItemManager.GetBasicItemData(completeItemID);
        
        if (completeItem == null)
        {
            //실패 이펙트
            return;
        }

        //이펙트
        spriteRenderer.sprite = completeItem.itemResourceImage;
        spriteRenderer.enabled = true;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
