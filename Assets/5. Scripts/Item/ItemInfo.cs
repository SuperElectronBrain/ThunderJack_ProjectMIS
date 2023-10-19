using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescription;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowItemInfo(RectTransform rt, string itemName, string itemDescription)
    {
        rectTransform.position = rt.position;
        
        this.itemName.text = itemName;
        this.itemDescription.text = itemDescription;
        
        gameObject.SetActive(true);
    }
}
