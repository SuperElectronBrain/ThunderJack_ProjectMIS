using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RavenCraftCore
{
    public class Cup : MonoBehaviour
    {
        private bool isUsed;
        private bool isGrab;
        private bool canUse;
        private Camera mainCam;
        [SerializeField]
        private float zOffset;

        [SerializeField]
        private SkeletonAnimation skAni;

        private Vector3 originPos;

        [SerializeField, Range(0f, 1f)] 
        private float speed;
        [SerializeField, Range(0f, 1f)] 
        private float inputAngle;
        [SerializeField]
        private float inputSpeed;

        private TrackEntry track;

        [SerializeField] private Transform inlet;
        [SerializeField] private float accuracy;

        [SerializeField]
        float amountValue;

        [Header("CraftingTools")]
        [SerializeField]
        Press press;

        private void Start()
        {
            mainCam = Camera.main;
            originPos = transform.position;
            zOffset = originPos.z;
            
            track = skAni.state.SetAnimation(2, "Tilting", false);
        }

        public void SetUse(bool canUse)
        {
            this.canUse = canUse;
        }

        private void OnMouseEnter()
        {
            if (isUsed || amountValue == 0)
                return;
            
            skAni.skeleton.SetSkin("Hand");
            skAni.skeleton.SetSlotsToSetupPose();
            skAni.timeScale = 1;
            skAni.AnimationName = "HandOn";
        }

        private void OnMouseExit()
        {
            if (isUsed || amountValue == 0)
                return;
            skAni.timeScale = 1;
            skAni.AnimationName = "HandOff";
        }

        private void OnMouseDown()
        {
            if (isUsed || amountValue == 0)
                return;
            CursorManager.SetCursorPosition(transform.position);
            CursorManager.onActiveComplate.AddListener(() => isGrab = true);
            CursorManager.onActive?.Invoke(true);
            skAni.timeScale = 0;
            skAni.AnimationName = "Tilting";
            isUsed = true;
        }

        public void SetInputItemID(int itemID)
        {
            press.SetPutInItemID(itemID);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!canUse)
                return;
            if (!isUsed)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                transform.position = originPos;
                isUsed = false;
                isGrab = false;
                track.TrackTime = 0;
                track.TimeScale = 0;

                skAni.skeleton.SetSkin("NoHand");
                skAni.skeleton.SetSlotsToSetupPose();
                return;
            }
            
            var dis = Vector2.Distance(transform.position, inlet.position) * accuracy;
            speed = Mathf.Lerp(1, 0, dis);
            
            track.TrackTime = speed;

            if(speed >= inputAngle)
            {
                InputSoultion();
            }

            var newPos = CursorManager.GetCursorPosition();

            newPos.z = zOffset;

            transform.position = newPos;
        }

        public void InputSoultion()
        {
            if (amountValue < 0)
                return;

            var putInValue = Time.deltaTime * inputSpeed;

            if (amountValue - putInValue < 0)
                putInValue = amountValue;

            amountValue -= putInValue;

            press.PutInSoultion(putInValue);
        }

        public void FillMaterialSoultion(float flowValue)
        {
            amountValue = 100 - flowValue;
        }
    }
}