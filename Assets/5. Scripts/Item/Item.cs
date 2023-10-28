using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        public int itemID;
        [SerializeField]
        public int itemAmount;
        [SerializeField] 
        public ItemType itemType;
        [SerializeField]
        public string itemName;
        [SerializeField]
        public string itemDescription;
    }

    public class JewelryItem : Item
    {
        [SerializeField]
        private float itemQuality;
    }
}