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

    bool isAccept;
    bool isFail;
    bool isDone;

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
        isAccept = false;
        isFail = false;
        isDone = false;
        SpineSkinChanger.RandomSkinChange(skAni);
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
        curWait = Waiting(craftWaitingDuration);
        StartCoroutine(curWait);
    }

    IEnumerator Waiting(float watingDuration)
    {
        yield return new WaitForSeconds(watingDuration);
        RefusalSales();
    }

    public void AcceptSales()
    {
        isAccept = true;
        skAni.AnimationName = "Yes";
        AnimationCheck();
        WaitingForCraft();
    }

    public void RefusalSales()
    {
        skAni.AnimationName = "No";
        AnimationCheck();
    }

    void AnimationCheck()
    {
        StopCoroutine(curWait);
        skAni.loop = false;
        skAni.AnimationState.Complete += AnimationEnd;
    }

    void AnimationEnd(Spine.TrackEntry te)
    {
        //ExitShop();        
        skAni.AnimationName = "Idle";
        skAni.loop = true;
        if (!isAccept || isFail)
        {
            EventManager.Publish(EventType.SalesFailure);
            EventManager.Publish(EventType.GuestExit);
        }            
        if (isDone)
        {
            EventManager.Publish(EventType.SalesSuccess);
            EventManager.Publish(EventType.GuestExit);
        }
        skAni.AnimationState.Complete -= AnimationEnd;
    }

    public void CheckItem(int requestItemID)
    {
        var requestItem = GameManager.Instance.ItemManager.GetRequestStuffByItemID(requestItemID);
        Debug.Log(requestItem.requestStuff1 + " " + requestItem.requestStuff2);       

        if (requestItem.requestStuff1 == request.requestStuff1 && requestItem.requestStuff2 == request.requestStuff2)
        {
            isDone = true;
            skAni.AnimationName = "Yes";
            AnimationCheck();                        
        }
        else
        {
            isFail = true;
            skAni.AnimationName = "No";
            AnimationCheck();
        }            
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
        Debug.Log(gameObject.name + " �մ�����");
        mr.enabled = true;
        WaitingForAnswer();
    }

    public void ExitShop()
    {
        Debug.Log(gameObject.name + " �մ�����");
        StopAllCoroutines();
        mr.enabled = false;        
    }
}
