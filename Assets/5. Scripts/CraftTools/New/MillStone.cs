using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace RavenCraftCore
{
    public class MillStone : MonoBehaviour
    {
        [SerializeField]
        private bool isGrab;

        [Header("MillStion Setting")]
        [SerializeField]
        private GameObject handle;
        [SerializeField]
        private Transform millStoneCenterPos;
        [SerializeField]
        private float r;
        [SerializeField]
        private float prevTheta;
        [SerializeField]
        private float theta;
        [SerializeField]
        private float deg;

        Cinemachine.DollyCartMove millStoneTrack;        

        [Header("Crefting Tools")]
        [SerializeField]
        private Cup cup;

        [Header("Debug")]
        [SerializeField]
        int insertedItemID;
        [SerializeField]
        int itemCount;

        [SerializeField]
        List<float> insertedItemProgress;
        [SerializeField]
        Transform[] colliders;

        [SerializeField]
        Vector3 colliderPos;

        [SerializeField]
        float grindingValue;
        [SerializeField]
        float resultValue;
        [SerializeField]
        float grindingSpeed;
        [SerializeField]
        float flowSpeed;

        [Header("SpineAni")]
        [SerializeField]
        SkeletonAnimation skAni;

        [SerializeField] private Animator topAni;
        [SerializeField] private Animator bottomAni;
        [SerializeField] private Material materialColor;

        [SerializeField] private float topFloatValue;
        [SerializeField] private float bottomFloatValue;
        [SerializeField] private float liquidFlowSpeed;
        [SerializeField] private const string aniFLoatProperty = "Velocity";

        private void Start()
        {
            millStoneTrack = GetComponentInChildren<Cinemachine.DollyCartMove>();
            skAni = GetComponentInChildren<SkeletonAnimation>();
            EventManager.Subscribe(EventType.CreateComplate, ResetMillStone);
        }

        void ResetMillStone()
        {
            InitCollPos();
            insertedItemProgress.Clear();
        }

        void InitCollPos()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].position = colliderPos;
            }
        }

        void SetInputItem()
        {
            Color newColor;
            Debug.Log(GameManager.Instance.ItemManager.GetBasicItemData(insertedItemID).accessoryColor);
            ColorUtility.TryParseHtmlString("#"+
                GameManager.Instance.ItemManager.GetBasicItemData(insertedItemID).accessoryColor,
                out newColor);
            materialColor.color = newColor;
            cup.SetInputItemID(insertedItemID);
        }

        public void EnterHandle()
        {
            if (isGrab || CursorManager.isUsed)
                return;

            skAni.AnimationName = "HandOn";
        }

        public void ExitHandle()
        {
            if (isGrab)
                return;

            skAni.AnimationName = "HandOff";
        }

        public void GrabHandle(bool isGrab)
        {
            this.isGrab = isGrab;
            /*GetMousePosToDeg();
            millStoneTrack.m_Position = deg / 360f;
            */
            deg = prevTheta;
            
            StartCoroutine(InitMousePosition());

        }

        void GetMousePosToDeg()
        {
            var mPos = CursorManager.GetCursorPosition() - millStoneCenterPos.position;

            theta = Mathf.Atan2(mPos.y, mPos.x);
            deg = (theta * Mathf.Rad2Deg) + 180;
        }

        IEnumerator InitMousePosition()
        {
            while (isGrab)
            {
                if (deg - prevTheta > 3f)
                {
                    CursorManager.SetCursorPosition(handle.transform.position);
                    //prevTheta = deg;
                    deg = prevTheta;
                }
                else if (Vector2.Distance(CursorManager.GetCursorPosition(), handle.transform.position) > 3f)
                {
                    CursorManager.SetCursorPosition(handle.transform.position);
                    deg = prevTheta;
                }
                    
                yield return new WaitForSeconds(0.2f);
            }
        }

        // Update is called once per frame
        void Update()
        {
            FlowLiquid();

            if (resultValue > 0 && grindingValue > 0)
            {
                FlowMaterialSolution();
            }
            else if (resultValue == 0)
                cup.SetUse(true);

            if (itemCount == 0)
                return;
            

            if (isGrab)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    isGrab = false;
                    skAni.AnimationName = "HandOff";
                    deg = prevTheta;
                }

                GetMousePosToDeg();
                
                if(deg > prevTheta)
                {
                    if (deg - prevTheta > 355f) ;
                    else
                    {
                        return;
                    }
                }
            }
            
            // 360 -> 0
            //CursorManager.SetCursorPosition(handle.transform.position);            
            
            //if(deg - prevTheta > 5f)
                
            
            /*if (Mathf.Abs(deg - prevTheta) > 10f)
                return;

            if (deg == prevTheta)
                return;*/

            grindingValue = 0f;
            var tv = 0f;
            
            for (int i = 0; i < insertedItemProgress.Count; i++)
            {
                if (insertedItemProgress[i] <= 0)
                {
                    grindingValue += 33.33333333f;
                    continue;
                }

                var gv = (deg - prevTheta) * grindingSpeed;
                if (gv < 0)
                {
                    tv = gv * -0.1f;
                    insertedItemProgress[i] += gv;
                }
                insertedItemProgress[i] = Mathf.Max(0, insertedItemProgress[i]);
                colliders[i].position = colliderPos - new Vector3(0, Mathf.Lerp(0.5f, 0, insertedItemProgress[i] * 0.01f), 0);
                grindingValue += Mathf.Lerp(33.33333f, 0, insertedItemProgress[i] * 0.01f);
            }

            topFloatValue += tv;

            /*for (int i = 0; i < insertedItemProgress.Count; i++)
            {
                if (insertedItemProgress[i] <= 0)
                {
                    continue;
                }
                
                insertedItemProgress[i] = Mathf.Max(0, insertedItemProgress[i]);
                resultValue -= insertedItemProgress[i];
                grindingValue += Mathf.Lerp(33.33333f, 0, insertedItemProgress[i] * 0.01f);
            }*/

            /*Vector3 newPos;

            newPos.x = (r * Mathf.Cos(theta)) + millStoneCenterPos.position.x;
            newPos.y = (r * Mathf.Sin(theta)) + millStoneCenterPos.position.y;
            newPos.z = 3;

            Debug.DrawLine(handle.transform.position, newPos, Color.blue, 100f);
            handle.transform.position = newPos;*/

            if (isGrab)
            {
                millStoneTrack.m_Position = deg / 360f;

                prevTheta = deg;
            }
        }

        void FlowLiquid()
        {
            topAni.SetFloat(aniFLoatProperty, topFloatValue);
            bottomAni.SetFloat(aniFLoatProperty, bottomFloatValue);

            if (topFloatValue > 0)
            {
                var tv = Time.deltaTime * liquidFlowSpeed;
                bottomFloatValue += tv * 2;
                topFloatValue = Mathf.Max(0, topFloatValue - tv);
            }
            if (bottomFloatValue > 0)
            {
                var bv = Time.deltaTime * liquidFlowSpeed * 1.8f;
                bottomFloatValue = Mathf.Max(0, bottomFloatValue - bv);
            }
        }

        public void FlowMaterialSolution()
        {
            if (100 - grindingValue > resultValue)
            {
                cup.SetUse(true);
                return;
            }

            cup.SetUse(false);
            resultValue -= Time.deltaTime * flowSpeed;

            resultValue = Mathf.Max(0, resultValue);
            
            cup.FillMaterialSoultion(resultValue);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(millStoneCenterPos.position, 0.3f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out BBB interactionMaterial))
            {
                if (!interactionMaterial.IsRelese())
                    return;
                if (interactionMaterial.IsInMillStone())
                    return;
                insertedItemID = interactionMaterial.GetComponentInParent<AAA>().m_ItemCode;
                interactionMaterial.InMillstone();
                SetInputItem();
                insertedItemProgress.Add(100);
            }
        }
    }
}