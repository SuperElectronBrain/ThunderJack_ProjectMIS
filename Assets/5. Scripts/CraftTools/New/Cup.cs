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
        private Camera mainCam;
        [SerializeField]
        private float zOffset;
        private float yOffset;

        [SerializeField]
        private SkeletonAnimation skAni;

        [SerializeField] private float delta;

        private Vector3 originPos;

        [SerializeField, Range(0f, 1f)] private float speed;
        private TrackEntry track;
        [SerializeField] private Transform inlet;
        [SerializeField] private float accuracy;

        [SerializeField]
        float AmountValue;

        private void Start()
        {
            mainCam = Camera.main;
            originPos = transform.position;
            zOffset = originPos.z;
            yOffset = originPos.y;
            
            track = skAni.state.SetAnimation(0, "animation", false);
        }

        private void OnMouseDown()
        {
            CursorManager.SetCursorPosition(transform.position);
            CursorManager.onActive?.Invoke(true);
            CursorManager.onActiveComplate.AddListener(() => isGrab = true);
            isUsed = true;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!isUsed)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                transform.position = originPos;
                isUsed = false;
                isGrab = false;
                track.TrackTime = 0;
                return;
            }
            
            var dis = Vector2.Distance(transform.position, inlet.position) * accuracy;
            speed = Mathf.Lerp(1, 0, dis);
            
            track.TrackTime = speed;

            var newPos = mainCam.ScreenToWorldPoint(CursorManager.GetCursorPosition());

            newPos.z = zOffset;

            transform.position = newPos;
        }

        public void FillMaterialSoultion(float flowValue)
        {
            AmountValue += flowValue;
        }
    }
}