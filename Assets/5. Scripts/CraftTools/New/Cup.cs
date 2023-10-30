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
        [SerializeField]
        private bool isUsed;
        [SerializeField]
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
        }

        public void SetUse(bool canUse)
        {
            this.canUse = canUse;
        }

        private void OnMouseEnter()
        {
            if (isGrab || amountValue == 0)
                return;
            skAni.state.ClearTrack(1);
            skAni.skeleton.SetSkin("Hand");
            skAni.skeleton.SetSlotsToSetupPose();
            skAni.state.SetAnimation(0, "HandOn", false);
            skAni.timeScale = 1;
            skAni.state.Complete += Grab;
        }

        private void OnMouseExit()
        {
            if (isGrab || amountValue == 0)
                return;

            isUsed = false;
            skAni.state.SetAnimation(0, "HandOff", false);
            skAni.timeScale = 1;
            skAni.state.Complete += Put;
        }

        private void OnMouseDown()
        {
            if (!isUsed || amountValue == 0)
                return;
            CursorManager.SetCursorPosition(transform.position);
            CursorManager.onActiveComplate.AddListener(() => isGrab = true);
            CursorManager.onActive?.Invoke(true);            
            track = skAni.state.SetAnimation(1, "Tilting", false);
            skAni.timeScale = 0;
            isGrab = true;
            isUsed = false;
        }

        void Grab(Spine.TrackEntry te)
        {
            skAni.state.Complete -= Grab;
            isUsed = true;
        }

        void Put(Spine.TrackEntry te)
        {
            skAni.state.Complete -= Put;            
        }

        public void SetInputItemID(int itemID)
        {
            press.SetPutInItemID(itemID);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!canUse || amountValue == 100)
                return;
            if (!isGrab)
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