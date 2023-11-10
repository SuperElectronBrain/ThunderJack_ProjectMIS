using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace RavenCraftCore
{
    public class Press : MonoBehaviour
    {
        bool isGrab;
        bool isUsed;
        bool isCreate;

        [SerializeField]
        private float putInValue;

        [Header("CraftingTools")]
        [SerializeField]
        private Book book;

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
        SkeletonAnimation skAni;

        [SerializeField]
        ParticleSystem createEffect;
        [SerializeField]
        ParticleSystem steam;
        
        // Start is called before the first frame update
        void Start()
        {
            originHandlePos = buttonPosition.position;
            skAni = GetComponentInChildren<SkeletonAnimation>();
            EventManager.Subscribe(EventType.CreateComplete, ResetPress);
        }

        public void EnterHandle()
        {
            if (putInValue <= 0)
                return;
            
            skAni.skeleton.SetSkin("Hand");
            skAni.skeleton.SetSlotsToSetupPose();            
            skAni.AnimationName = "HandOn";
            skAni.timeScale = 1;
            skAni.state.Complete += Grab;
        }

        public void ExitHandle()
        {
            if (isGrab)
                return;
            skAni.AnimationName = "HandOff";
        }

        public void GrabHandle()
        {
            if (!isUsed)
                return;
            isGrab = true;
            CursorManager.SetCursorPosition(handleObject.position);
            prevPos = CursorManager.GetCursorPosition();
        }

        void Grab(Spine.TrackEntry te)
        {
            isUsed = true;
        }

        void Put(Spine.TrackEntry te)
        {
            isUsed = false;
        }

        void ResetPress()
        {
            track.m_Position = 0;
            putInValue = 0;
            isCreate = false;
            
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = 0f;
            }
        }

        public void PutInSoultion(float inputValue)
        {
            putInValue += inputValue;

            value[putInItemData.elementType1 - 1] = Mathf.Lerp(0, 100 * putInItemData.elementPercent1, putInValue * 0.01f);
            value[putInItemData.elementType2 - 1] = Mathf.Lerp(0, 100 * putInItemData.elementPercent2, putInValue * 0.01f);
        }

        public void SetAccessoryData(int accessoryID)
        {
            //d_AccessoryData = accessoryID;
        }

        public void SetItemData(int itemID)
        {
            putInItemData = GameManager.Instance.ItemManager.GetMaterialItem(itemID);
        }

        Vector3 prevPos;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < value.Length; i++)
            {
                book.UpdateElementCircle((ElementType)i, value[i]);
            }

            if (!isGrab)
                return;

            if(Input.GetMouseButtonUp(0))
            {
                isGrab = false;
                track.m_Position = 0;
                //handleObject.position = originHandlePos;
                buttonPosition.position = originHandlePos;

                skAni.skeleton.SetSkin("NoHand");
                skAni.skeleton.SetSlotsToSetupPose();
                skAni.timeScale = 0;
                skAni.AnimationName = "HandOff";
            }

            if (track.m_Position >= 0.99f && !isCreate)
            {
                isCreate = true;
                PressHandle();                
            }
            
            var handleDir = buttonPosition.forward;
            var mPos = CursorManager.GetCursorPosition();
            var hm = mPos - prevPos;
            hm.z = 0f;

            float progress = Vector3.Dot(handleDir, hm);

            //handleObject.position = CursorManager.GetCursorPosition();
            track.m_Position += progress;
            prevPos = mPos;
        }
        
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
            steam.Play();
            createEffect.Play();
            var gem = GameManager.Instance.ItemManager.GetGemRecipe(rankElement[0], rankElement[1], rankElement[2]);
            
            var completeItem =
                GameManager.Instance.ItemManager.GetCombinationItem(gem.itemID, accessoryPlate.GetAccessory());
            
            accessoryPlate.CompleteCraft(completeItem);
            
            print(GameManager.Instance.ItemManager.GetItemName(gem.itemID));
        }
    }
}