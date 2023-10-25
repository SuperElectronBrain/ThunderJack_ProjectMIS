using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace RavenCraftCore
{
    public class MillStone : MonoBehaviour
    {
        private bool isOver;
        private bool isGrab;
        private Camera mainCam;

        [Header("MillStion Setting")]
        [SerializeField]
        private GameObject handle;
        [SerializeField]
        private Vector2 millStoneCenterPos;
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
        [SerializeField]
        private TrackEntry track;

        private void Start()
        {
            mainCam = Camera.main;
            millStoneTrack = GetComponentInChildren<Cinemachine.DollyCartMove>();
            skAni = GetComponentInChildren<SkeletonAnimation>();
            cup.SetInputItemID(insertedItemID);
            track = skAni.state.SetAnimation(0, "animation", false);
        }

        IEnumerator CStartAnimation(bool isBack = false)
        {
            var animationProgress = 0f;

            while (animationProgress <= 1f)
            {
                animationProgress += Time.deltaTime;
                track.TrackTime = animationProgress;
                yield return null;
            }
            track.TrackTime = 1;
        }

        public void EnterHandle()
        {
            if (isGrab)
                return;
            StopAllCoroutines();
            StartCoroutine(CStartAnimation());
        }

        public void ExitHandle()
        {
            if (isGrab)
                return;
            StopAllCoroutines();
            track.TrackTime = 0;
        }

        public void GrabHandle(bool isGrab)
        {
            this.isGrab = isGrab;
            track.TrackTime = 1;
            GetMousePosToDeg();
            millStoneTrack.m_Position = deg / 360f;
            prevTheta = deg;
        }

        void GetMousePosToDeg()
        {
            var mPos = CursorManager.GetCursorPosition();

            theta = Mathf.Atan2(mPos.y, mPos.x);
            deg = (theta * Mathf.Rad2Deg) + 180;

            if (deg < 0)
                deg *= -2;

            theta = deg * Mathf.Deg2Rad;
        }

        // Update is called once per frame
        void Update()
        {
            if(resultValue > 0 && grindingValue > 0)
            {
                FlowMaterialSolution();
            }

            if (itemCount == 0 || !isGrab)
                return;

            if(Input.GetMouseButtonUp(0))
            {
                isGrab = false;
                track.TrackTime = 0;
            }

            GetMousePosToDeg();

            //CursorManager.SetCursorPosition(handle.transform.position);            

            if(deg > prevTheta)
            {
                if(deg - prevTheta <= 350f)
                    return;
            }

            if (Mathf.Abs(deg - prevTheta) > 10f)
                return;

            if (deg == prevTheta)
                return;

            /*grindingValue = 0f;
            resultValue = 100f;

            if(deg - prevTheta < 0)
            {
                for (int i = 0; i < insertedItemProgress.Count; i++)
                {
                    if (insertedItemProgress[i] <= 0)
                    {
                        grindingValue += 33.33333333f;
                        resultValue -= 33.33333333f;
                        continue;
                    }                        
                    insertedItemProgress[i] += (deg - prevTheta) * grindingSpeed;
                    insertedItemProgress[i] = Mathf.Max(0, insertedItemProgress[i]);
                    resultValue -= insertedItemProgress[i];
                    grindingValue += Mathf.Lerp(33.33333f, 0, insertedItemProgress[i] * 0.01f);
                }
            }*/

            for (int i = 0; i < insertedItemProgress.Count; i++)
            {
                if (insertedItemProgress[i] <= 0)
                {   
                    continue;
                }
                var gv = (deg - prevTheta) * grindingSpeed;
                insertedItemProgress[i] += gv;
                insertedItemProgress[i] = Mathf.Max(0, insertedItemProgress[i]);
                resultValue -= insertedItemProgress[i];
                grindingValue += Mathf.Lerp(33.33333f, 0, insertedItemProgress[i] * 0.01f);
            }

            /*Vector3 newPos;

            newPos.x = (r * Mathf.Cos(theta)) + millStoneCenterPos.x;
            newPos.y = (r * Mathf.Sin(theta)) + millStoneCenterPos.y;
            newPos.z = 3;

            Debug.DrawLine(handle.transform.position, newPos, Color.blue, 100f);*/
            //handle.transform.position = newPos;

            millStoneTrack.m_Position = deg / 360f;

            prevTheta = deg;            
        }

        public void FlowMaterialSolution()
        {
            if (100 - grindingValue > resultValue)
                return;

            var flowValue = Time.deltaTime * flowSpeed;
            resultValue -= flowValue;

            cup.FillMaterialSoultion(flowValue);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(millStoneCenterPos, 0.3f);
        }
    }
}