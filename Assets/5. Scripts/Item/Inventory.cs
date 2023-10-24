using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RavenCraftCore
{
    public class Inventory : MonoBehaviour
    {
        private List<ItemSlot> itemSlots = new();
        [SerializeField]
        private GameObject itemSlot;
        [SerializeField]
        private Transform inventory;
        [SerializeField]
        InteractionItem interactionItem; 

        [SerializeField]
        private int selectItemID;
        
        public static UnityEvent<int> onClick = new();
        public static UnityEvent<int> onEnter = new();
        public static UnityEvent<int> onExit = new();

        [SerializeField]
        private Item item;
        
        // Start is called before the first frame update
        void Start()
        {
            onClick.AddListener(OnClick);
            onEnter.AddListener(OnEnter);
            onExit.AddListener(OnExit);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                AddItem(new Item
                {
                    itemID = item.itemID,
                    itemAmount = item.itemAmount
                });    
            }
        }

        public void AddItem(Item addItem)
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (!itemSlots[i].IsSameItem(addItem.itemID)) continue;

                itemSlots[i].AddItem(addItem.itemAmount);
                return;
            }

            var newItemSlot = Instantiate(itemSlot, inventory).GetComponent<ItemSlot>();
            newItemSlot.InitItem(addItem);
            
            itemSlots.Add(newItemSlot);
        }

        void OnClick(int itemID)
        {
            selectItemID = itemID;

            interactionItem.ItemInteraction(selectItemID);
        }
        
        void OnEnter(int itemID)
        {
            // 아이템 설명
        }

        void OnExit(int itemID)
        {
            //아이템 설명창X
        }

        //Todo Inventory Save Load
    }
}

