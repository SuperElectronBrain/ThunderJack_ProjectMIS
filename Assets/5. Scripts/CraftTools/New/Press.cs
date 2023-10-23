using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    public class Press : MonoBehaviour
    {
        [SerializeField]
        private float inputValue;
        [SerializeField]
        private MaterialItemData inputMaterialItem;

        [Header("CraftingTools")]
        [SerializeField]
        private Book book;

        [Header("Debug")]
        [SerializeField]
        private int d_ItemData;

        [SerializeField] 
        private int d_AccessoryData;

        [SerializeField, Range(0, 100)]
        private float[] value;

        [SerializeField]
        private ElementType[] rankElement;

        [SerializeField]
        private PressAccessoryPlate accessoryPlate;
        
        // Start is called before the first frame update
        void Start()
        {
            SetItemData(d_ItemData);
        }

        void ResetPress()
        {
            
        }

        public void SetItemData(int itemID)
        {
            inputMaterialItem = GameManager.Instance.ItemManager.GetMaterialItem(itemID);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < value.Length; i++)
            {
                book.UpdateElementCircle((ElementType)i, value[i]);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                PressHandle();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void PressHandle()
        {
            GetRankElement();
            CheckGemRecipe();
            ResetPress();
        }

        void GetRankElement()
        {
            ElementType curElementType = ElementType.End;
            
            for (int i = 0; i < 3; i++)
            {
                var curElementValue = 0f;
                rankElement[i] = ElementType.End;
                
                for (int j = 0; j < value.Length; j++)
                {
                    if (curElementType != ElementType.End && ((ElementType)j == rankElement[0] ||
                                                              (ElementType)j == rankElement[1] ||
                                                              (ElementType)j == rankElement[2]))
                        continue;
                    if (value[j] > curElementValue)
                    {
                        curElementValue = value[j];
                        curElementType = (ElementType)j;
                    }
                }

                rankElement[i] = curElementType;
            }
        }

        void CheckGemRecipe()
        {
            var gem = GameManager.Instance.ItemManager.GetGemRecipe(rankElement[0], rankElement[1], rankElement[2]);

            accessoryPlate.CompleteCraft(GameManager.Instance.ItemManager.GetCombinationItem(gem.itemID, d_AccessoryData));
            print(GameManager.Instance.ItemManager.GetItemName(gem.itemID));
        }
    }
}