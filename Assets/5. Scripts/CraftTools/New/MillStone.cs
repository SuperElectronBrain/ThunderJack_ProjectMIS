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
        private bool isUsed;
        private bool isGrab;
        private Camera mainCam;


        [SerializeField]
        private Vector2 millStoneCenterPos;
        [SerializeField]
        private GameObject handle;

        [SerializeField]
        private float r;

        [SerializeField]
        private float prevTheta;
        [SerializeField]
        private float theta;


        [Header("Debug")]
        [SerializeField]
        int insertedItemID;
        [SerializeField]
        int itemCount;

        [SerializeField]
        List<float> insertedItemProgress;

        [SerializeField]
        float resultValue;
        [SerializeField]
        float grindingSpeed;

        private void Start()
        {
            mainCam = Camera.main;
            resultValue = 100;
        }

        public void GrabHandle(bool isGrab)
        {
            this.isGrab = isGrab;
        }

        // Update is called once per frame
        void Update()
        {
            if (itemCount == 0 && !isGrab)
                return;

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

            resultValue = 0;

            for(int i = 0; i < insertedItemProgress.Count; i++)
            {
                insertedItemProgress[i] += Time.deltaTime * grindingSpeed;
                resultValue += Mathf.Lerp(0, 33.33333f, insertedItemProgress[i] / 100);
            }

            Vector3 newPos;

            newPos.x = (r * Mathf.Cos(theta)) + millStoneCenterPos.x;
            newPos.y = (r * Mathf.Sin(theta)) + millStoneCenterPos.y;
            newPos.z = 3;

            Debug.DrawLine(handle.transform.position, newPos, Color.blue, 100f);
            handle.transform.position = newPos;
            
            prevTheta = theta;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(millStoneCenterPos, 0.3f);
        }
    }
}
