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

        // Update is called once per frame
        void Update()
        {
            if (itemCount == 0)
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
                resultValue += Mathf.Lerp(0, 33.3333f, insertedItemProgress[i] / 100);
            }

            Vector3 newPos;

            newPos.x = r * Mathf.Cos(theta);
            newPos.y = r * Mathf.Sin(theta);
            newPos.z = 3;

            handle.transform.position = newPos;
            prevTheta = theta;
        }
    }
}
