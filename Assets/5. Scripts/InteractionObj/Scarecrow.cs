using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Scarecrow : MonoBehaviour, IInteraction
{
    SkeletonAnimation skAni;

    bool isActive;

    public bool IsUsed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
    }

    public void Interaction(GameObject user)
    {

    }

    public void ChangeAnimation(Spine.TrackEntry te)
    {
        skAni.loop = true;
        if (isActive)
        {
            skAni.AnimationName = "Move";
        }
        else
        {
            skAni.AnimationName = "Idle";
        }

        skAni.AnimationState.Complete -= ChangeAnimation;
    }

    private void OnTriggerEnter(Collider other)
    {
        isActive = true;
        skAni.loop = false;
        skAni.AnimationName = "IdleToMove";
        skAni.AnimationState.Complete += ChangeAnimation;
    }

    private void OnTriggerExit(Collider other)
    {
        isActive = false;
        skAni.loop = false;
        skAni.AnimationName = "MoveToIdle";
        skAni.AnimationState.Complete += ChangeAnimation;
    }
}
