using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(RavenCraftCore.ItemSlot))]
public class ItemSlotEditor : ButtonEditor
{
    RavenCraftCore.ItemSlot itemSlot;
    SerializedProperty itemImage;
    SerializedProperty itemAmountText;
    SerializedProperty item;

    protected override void OnEnable()
    {
        itemSlot = target as RavenCraftCore.ItemSlot;
        base.OnEnable();
        itemImage = serializedObject.FindProperty("itemImage");
        itemAmountText = serializedObject.FindProperty("itemAmountText");
        item = serializedObject.FindProperty("item");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(itemImage);
        EditorGUILayout.PropertyField(itemAmountText);
        EditorGUILayout.PropertyField(item);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif

namespace RavenCraftCore
{
    public class ItemSlot : CustomButton
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TextMeshProUGUI itemAmountText;
        [SerializeField]
        private Item item;
        
        void Start()
        {
            itemImage = GetComponent<Image>();
            
            onDown.AddListener(OnClick);
            onEnter.AddListener(OnEnter);
            onExit.AddListener(OnExit);
        }

        public void InitItem(Item newItem)
        {
            var itemData = GameManager.Instance.ItemManager.GetBasicItemData(newItem.itemID);
            itemImage.sprite = itemData.itemResourceImage;
            itemImage.gameObject.SetActive(true);
            item = newItem;
            itemAmountText.text = newItem.itemAmount.ToString();
        }

        public void AddItem(int addItemAmount)
        {
            item.itemAmount += addItemAmount;
            itemAmountText.text = item.itemAmount.ToString();
        }

        public void UseItem()
        {
            if (item.itemAmount == 1)
            {
                Destroy(gameObject);
            }
            
            item.itemAmount--;
        }

        public bool IsSameItem(int itemID)
        {
            return item.itemID == itemID;
        }

        void OnClick()
        {
            if (item == null)
                return;

            Inventory.onClick?.Invoke(item.itemID);
        }

        void OnEnter()
        {
            if (item == null)
                return;
        }

        void OnExit()
        {
            if (item == null)
                return;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            onClick.RemoveAllListeners();
            onEnter.RemoveAllListeners();
            onExit.RemoveAllListeners();
        }
    }
}