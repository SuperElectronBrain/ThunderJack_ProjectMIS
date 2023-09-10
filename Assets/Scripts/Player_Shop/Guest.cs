using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Guest : MonoBehaviour
{
    SkeletonAnimation skAni;
    MeshRenderer mr;

    GuestData guestData;
    RequestData request;

    [SerializeField]
    float answerWatingDuration;
    [SerializeField]
    float craftWaitingDuration;
    IEnumerator curWait;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
        mr = GetComponent<MeshRenderer>();
    }

    public void InitGuest(GuestData guest, RequestData request)
    {
        guestData = guest;
        this.request = request;
        /*skAni.skeletonDataAsset = AddressableManager.LoadObject<SkeletonDataAsset>(guest.guestNameEg);
        mr.material = AddressableManager.LoadObject<Material>(guest.guestNameEg);*/
    }

    void WaitingForAnswer()
    {
        curWait = Waiting(answerWatingDuration);
        StartCoroutine(curWait);
    }

    void WaitingForCraft()
    {
        StartCoroutine(Waiting(craftWaitingDuration));
    }

    IEnumerator Waiting(float watingDuration)
    {
        yield return new WaitForSeconds(watingDuration);
        RefusalSales();
    }

    public void AcceptSales()
    {
        
        skAni.AnimationName = "LAUGH";
        AnimationCheck();
        //WaitingForCraft();
    }

    public void RefusalSales()
    {
        skAni.AnimationName = "ANGRY";
        AnimationCheck();
    }

    void AnimationCheck()
    {
        StopCoroutine(curWait);
        skAni.loop = false;
        skAni.AnimationState.Complete += AnimationEnd;
        skAni.loop = true;
    }

    void AnimationEnd(Spine.TrackEntry te)
    {
        ExitShop();
        skAni.AnimationName = "IDLE";
        skAni.loop = true;
    }

    public void FirstGuest()
    {
        EventManager.Publish(EventType.Dialog);        
    }

    public string GetGuestName()
    {
        return guestData.guestNameKo;
    }

    public string GetRequest()
    {
        return request.requestScript;
    }

    public void EntryShop()
    {
        Debug.Log(gameObject.name + " º’¥‘¿‘¿Â");
        mr.enabled = true;
        WaitingForAnswer();
    }

    public void ExitShop()
    {
        skAni.AnimationState.Complete -= AnimationEnd;
        Debug.Log(gameObject.name + " º’¥‘≈¿Â");
        StopCoroutine(curWait);
        mr.enabled = false;        
        EventManager.Publish(EventType.GuestExit);
    }
}
