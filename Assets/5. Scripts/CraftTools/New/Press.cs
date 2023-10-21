using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    public class Press : MonoBehaviour
    {
        [SerializeField]
        float inputValue;
        [SerializeField]
        MaterialItemData inputMaterialItem;

        [Header("CraftingTools")]
        [SerializeField]
        Book book;

        [Header("Debug")]
        [SerializeField]
        int d_ItemData;

        [SerializeField, Range(0, 100)]
        float value;
        [SerializeField, Range(0, 100)]
        float value2;
        [SerializeField, Range(0, 100)]
        float value3;
        [SerializeField, Range(0, 100)]
        float value4;
        [SerializeField, Range(0, 100)]
        float value5;


        // Start is called before the first frame update
        void Start()
        {
            SetItemData(d_ItemData);
        }

        public void SetItemData(int itemID)
        {
            inputMaterialItem = GameManager.Instance.ItemManager.GetMaterialItem(itemID);
        }

        // Update is called once per frame
        void Update()
        {
            book.UpdateElementCircle(ElementType.Justice, value);
            book.UpdateElementCircle(ElementType.Wisdom, value2);
            book.UpdateElementCircle(ElementType.Nature, value3);
            book.UpdateElementCircle(ElementType.Mystic, value4);
            book.UpdateElementCircle(ElementType.Insight, value5);
        }
    }
}