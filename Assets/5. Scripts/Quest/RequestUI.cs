using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requestTitle;
    [SerializeField] private TextMeshProUGUI requestDescription;
    [SerializeField] private List<TextMeshProUGUI> requestItems;
    [SerializeField] private GameObject itemObject;

    public void SetRequestTitle(string title, string description)
    {
        requestTitle.text = title;
        requestDescription.text = description;
    }

    public void AddRequestItem(string itemName)
    {
        var item = Instantiate(itemObject, transform.Find("Items")).GetComponent<TextMeshProUGUI>();
        item.text = itemName;
        requestItems.Add(item);
    }

    public void CompleteRequestItem(int index)
    {
        requestItems[index].fontStyle = FontStyles.Strikethrough;
    }

    public void CompleteRequest()
    {
        
    }
}
