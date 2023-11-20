using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.Mathematics;
using UnityEngine.AI;

namespace Test
{
    public enum B
    {
        hi, idle, talk, sit1, sit2, sit3, walk1, walk2, walk3
    }

    public class TestNPC : MonoBehaviour
    {
        [SerializeField]
        LookDir dir;

        SkeletonAnimation skAni;

        public B b;
        public bool isTarget;
        B cb;

        public Transform player;
        public NavMeshAgent nma;
        private Camera c;

       // Start is called before the first frame update
        void Start()
        {
            c = Camera.main;
            nma = GetComponent<NavMeshAgent>();
            dir = new LookDir();
            skAni = GetComponentInChildren<SkeletonAnimation>();
            b = B.idle;
            cb = B.idle;
            dir.Init(transform, c);
            ChangeAni();
            //skAni.AnimationState.Complete += Loop;
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y, 0);
            ChangeAni();
            if (isTarget)
            {
                dir.SetDir(transform.position, player.transform.position);
                if(b != cb)
                {
                    cb = b;
                    ChangeAni();
                }                
            }
            else
            {
                dir.SetDir(nma.velocity);
            }
        }

        void ChangeAni()
        {
            Flip();
            if (dir.isFront)
                skAni.AnimationName = "A_" + b.ToString() + "_F";
            else
                skAni.AnimationName = "A_" + b.ToString() + "_B";
        }

        void Flip()
        {
            var scaleX = dir.isRight ? -1 : 1;
            var newScale = transform.localScale;
            newScale.x = Mathf.Abs(transform.localScale.x) * scaleX;
            transform.localScale = newScale;
        }

        void Loop(Spine.TrackEntry track)
        {
            ChangeAni();
        }
    }

    [System.Serializable]
    public class LookDir
    {
        public bool isFront;
        public bool isRight;
        public bool isSideWalk;
        
        private Camera cam;
        private Transform t;
    
        public void Init(Transform myT, Camera mainCam)
        {
            cam = mainCam;
            t = myT;
        }

        public void SetDir(Vector3 velocity)
        {
            var pos = t.position;
            pos.y = 0f;
            velocity += pos;
            velocity.y = 0f;
            var dir = (pos - velocity).normalized;
            var cross = Vector3.Cross(t.forward, dir);
            var dot = Vector3.Dot(t.forward, dir);

            isRight = cross.y <= 0;
            isFront = dot >= 0;
        }
        public void SetDir(Vector3 myPos, Vector3 targetPos)
        {
            if (myPos.x <= targetPos.x)
                isRight = true;
            else
                isRight = false;

            if (myPos.z >= targetPos.z)
                isFront = true;
            else
                isFront = false;

            isSideWalk = false;
        }
    }
}