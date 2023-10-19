using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace RavenCraftCore
{
    public class MillStone : MonoBehaviour
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
        }
        
        private void OnMouseEnter()
        {
            isUsed = true;
        }

        private void OnMouseExit()
        {
            isUsed = false;
        }

        [SerializeField]
        private GameObject go;

        [SerializeField]
        private float r;

        [SerializeField]
        private float prevTheta;

        [SerializeField]
        private float theta;

        [SerializeField]
        private float deg;
        
        // Update is called once per frame
        void Update()
        {
            var mPos = Input.mousePosition;
            mPos = mainCam.ScreenToWorldPoint(CursorManager.GetCursorPosition());

            theta = Mathf.Atan2(mPos.y, mPos.x);
            
            deg = theta * Mathf.Rad2Deg;
            
            if (theta > 0)
            {
                if (prevTheta <= -3.1f) ;
                else if (theta >= prevTheta)
                    return;
            }
            else
            {
                if (theta >= prevTheta)
                    return;
            }

            Vector3 newPos;

            newPos.x = r * Mathf.Cos(theta);
            newPos.y = r * Mathf.Sin(theta);
            newPos.z = 3;

            go.transform.position = newPos;
            prevTheta = theta;
        }
    }
}
