using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Guest : MonoBehaviour
{
    SkeletonAnimation skAni;
    MeshRenderer mr;
    float answerWatingDuration;
    float craftWaitingDuration;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
        mr = GetComponent<MeshRenderer>();
    }

    public void InitGuest(string guestName)
    {
        skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>(guestName);
        mr.material = AddressableManager.LoadObject<Material>(guestName);
    }

    public void WaitingForAnswer()
    {
        StartCoroutine(Waiting(answerWatingDuration));        
    }

    public void WaitingForCraft()
    {
        StartCoroutine(Waiting(craftWaitingDuration));
    }

    IEnumerator Waiting(float watingDuration)
    {
        yield return new WaitForSeconds(watingDuration);
    }

    public void AcceptSales()
    {
        skAni.AnimationName = "LAUGH";
        AnimationCheck();
    }

    public void RefusalSales()
    {
        skAni.AnimationName = "ANGRY";
        AnimationCheck();
    }

    void AnimationCheck()
    {
        skAni.AnimationState.Complete += (result) =>
        {
            skAni.AnimationName = "IDLE";
        };
    }

    public void EntryShop()
    {
        mr.enabled = true;
    }

    public void ExitShop()
    {
        mr.enabled = false;
    }
}
