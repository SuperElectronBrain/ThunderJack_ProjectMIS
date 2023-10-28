using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RavenCraftCore
{
    public class Press : MonoBehaviour
    {
        bool isGrab;

        [SerializeField]
        private int putInItemID;
        [SerializeField]
        private float putInValue;
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

        [SerializeField]
        MaterialItemData putInItemData;

        [SerializeField]
        Transform buttonPosition;
        [SerializeField]
        Transform handleObject;
        Vector3 originHandlePos;
        [SerializeField]
        Cinemachine.DollyCartMove track;
        
        // Start is called before the first frame update
        void Start()
        {
            SetItemData(d_ItemData);
            originHandlePos = handleObject.position;
        }

        public void EnterHandle()
        {

        }

        public void ExitHandle()
        {

        }

        public void GrabHandle()
        {
            isGrab = true;
            CursorManager.SetCursorPosition(handleObject.position);
            prevPos = CursorManager.GetCursorPosition();
        }

        void ResetPress()
        {
            
        }

        public void SetPutInItemID(int itemID)
        {
            putInItemID = itemID;
            putInItemData = GameManager.Instance.ItemManager.GetMaterialItem(itemID);
        }

        public void PutInSoultion(float inputValue)
        {
            putInValue += inputValue;

            value[putInItemData.elementType1 - 1] = Mathf.Lerp(0, 100 * putInItemData.elementPercent1, putInValue * 0.01f);
            value[putInItemData.elementType2 - 1] = Mathf.Lerp(0, 100 * putInItemData.elementPercent2, putInValue * 0.01f);
            //value[putInItemData.elementType3 - 1] = Mathf.Lerp(0, 100 * putInItemData.elementPercent3, inputValue);
        }

        public void SetItemData(int itemID)
        {
            inputMaterialItem = GameManager.Instance.ItemManager.GetMaterialItem(itemID);
        }

        Vector3 prevPos;

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

            if (!isGrab)
                return;

            if(Input.GetMouseButtonUp(0))
            {
                isGrab = false;
                handleObject.position = originHandlePos;
            }

            var handleDir = buttonPosition.forward;
            var mPos = CursorManager.GetCursorPosition();
            var hm = mPos - prevPos;
            hm.z = 0f;

            float progress = Vector3.Dot(handleDir, hm);

            handleObject.position = CursorManager.GetCursorPosition();
            track.m_Position += progress;
            prevPos = mPos;
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