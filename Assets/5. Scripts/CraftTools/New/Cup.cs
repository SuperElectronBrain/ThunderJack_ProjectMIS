using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

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
            isGrab = true;
            CursorManager.onActive?.Invoke(true);
        }
        
        private void OnMouseEnter()
        {
            isUsed = true;
        }

        private void OnMouseExit()
        {
            isUsed = false;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!isGrab)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                transform.position = originPos;
                isGrab = false;
            }
            
            var dis = Vector2.Distance(transform.position, inlet.position) * accuracy;
            speed = Mathf.Lerp(1, 0, dis);
            
            track.TrackTime = speed;

            Vector3 newPos = mainCam.ScreenToWorldPoint(CursorManager.GetCursorPosition());

            newPos.z = zOffset;

            transform.position = newPos;

            /*
            if (isGrab)
            {
                var mPos = Input.mousePosition;

                mPos = mainCam.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y,
                    -mainCam.transform.position.z + zOffset));

                transform.position = mPos;
            }*/
        }
    }
}