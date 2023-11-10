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
    PlayerShop playerShop;

    [SerializeField]
    float answerWatingDuration;
    [SerializeField]
    float craftWaitingDuration;
    IEnumerator curWait;

    bool isAccept;
    bool isFail;
    bool isDone;
    bool isEntry;

    // Start is called before the first frame update
    void Start()
    {
        skAni = GetComponent<SkeletonAnimation>();
        mr = GetComponent<MeshRenderer>();
        playerShop = GetComponentInParent<PlayerShop>();
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
        isFail = true;
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

    public void CheckItem(int requestItemID, float perfection)
    {
        var requestItem = GameManager.Instance.ItemManager.GetRequestStuffByItemID(requestItemID);
        Debug.Log(requestItem.requestStuff1 + " " + requestItem.requestStuff2);

        SalesData salesData = GameManager.Instance.ItemManager.GetSalesData(requestItemID);
        salesData.perfection = perfection;

        if (requestItem.requestStuff1 == request.requestStuff1 && requestItem.requestStuff2 == request.requestStuff2)
        {
            isDone = true;
            skAni.AnimationName = "Yes";

            playerShop.Sales.SalesSuccess(salesData, playerShop.SalesResult);
        }
        else
        {
            isFail = true;
            skAni.AnimationName = "No";

            playerShop.Sales.SalesFailure(salesData, playerShop.SalesResult);
        }
        AnimationCheck();
    }

    public string GetGuestName()
    {
        return guestData.guestNameKo;
    }

    public string GetRequest()
    {
        return request.requestScript;
    }

    public RequestData GetRequestData()
    {
        return request;
    }

    public void EntryShop()
    {
        Debug.Log(gameObject.name + " º’¥‘¿‘¿Â");
        EventManager.Publish(EventType.Dialog);
        isEntry = true;
        mr.enabled = true;
        WaitingForAnswer();
    }

    public void ExitShop()
    {
        Debug.Log(gameObject.name + " º’¥‘≈¿Â");
        StopAllCoroutines();
        isEntry = false;
        mr.enabled = false;        
    }

    public bool IsEntry()
    {
        return isEntry;
    }
}
