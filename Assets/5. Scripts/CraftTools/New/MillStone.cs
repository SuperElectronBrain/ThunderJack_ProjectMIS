using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

namespace RavenCraftCore
{
    public class MillStone : MonoBehaviour
    {
        private bool isGrab;
        private Camera mainCam;

        [Header("MillStion Setting")]
        [SerializeField]
        private GameObject handle;
        [SerializeField]
        private Vector2 millStoneCenterPos;
        [SerializeField]
        private float r;
        private float prevTheta;
        private float theta;

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

        private void Start()
        {
            mainCam = Camera.main;
        }

        public void GrabHandle(bool isGrab)
        {
            this.isGrab = isGrab;
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
            }

            var mPos = mainCam.ScreenToWorldPoint(CursorManager.GetCursorPosition());

            theta = Mathf.Atan2(mPos.y, mPos.x);
            
            if (theta > 0)
            {
                if (prevTheta <= -3f) ;
                else if (theta >= prevTheta)
                    return;
            }
            else
            {
                if (theta >= prevTheta)
                    return;
            }

            grindingValue = 0;

            for(int i = 0; i < insertedItemProgress.Count; i++)
            {
                if (insertedItemProgress[i] <= 0)
                    continue;
                insertedItemProgress[i] -= Time.deltaTime * grindingSpeed;
                grindingValue += Mathf.Lerp(33.33333f, 0, insertedItemProgress[i] / 100f);
            }

            Vector3 newPos;

            newPos.x = (r * Mathf.Cos(theta)) + millStoneCenterPos.x;
            newPos.y = (r * Mathf.Sin(theta)) + millStoneCenterPos.y;
            newPos.z = 3;

            Debug.DrawLine(handle.transform.position, newPos, Color.blue, 100f);
            handle.transform.position = newPos;
            
            prevTheta = theta;
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
