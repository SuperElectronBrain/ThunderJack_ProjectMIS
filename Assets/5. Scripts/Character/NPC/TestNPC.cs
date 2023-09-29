using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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
        B cb;

        public Transform player;

        // Start is called before the first frame update
        void Start()
        {
            dir = new LookDir();
            skAni = GetComponentInChildren<SkeletonAnimation>();
            b = B.idle;
            cb = B.idle;
            ChangeAni();
            skAni.AnimationState.Complete += Loop;
        }

        // Update is called once per frame
        void Update()
        {
            dir.SetDir(transform.position, player.transform.position);
            if(b != cb)
            {
                cb = b;
                ChangeAni();
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

        public void SetDir(Vector3 velocity)
        {
            if (velocity.x >= 0)
                isRight = true;
            else
                isRight = false;

            if (velocity.z <= 0)
                isFront = true;
            else
                isFront = false;

            if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.z))
                isSideWalk = true;
            else
                isSideWalk = false;
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