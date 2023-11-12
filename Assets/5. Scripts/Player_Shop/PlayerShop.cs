using System.Collections;
using System.Collections.Generic;
using RavenCraftCore;
using UnityEngine;
using TMPro;

public class PlayerShop : MonoBehaviour
{
    PlayerShop_Guest guestData;
    PlayerShop_Sales sales;
    [SerializeField]
    DialogueBox dialogBox;

    [SerializeField]
    Guest guest;

    [SerializeField]
    PlayerCharacter player;

    [SerializeField]
    int entryWeight;
    [SerializeField]
    int[] weightValue = new int[10];
    [SerializeField]
    int idx = 0;

    [SerializeField]
    SalesResult salesResult;

    [SerializeField]
    GameObject salesOver;

    [SerializeField]
    TextMeshProUGUI totalIncome;
    [SerializeField]
    TextMeshProUGUI totalFame;
    [SerializeField]
    TextMeshProUGUI totalGuestCount;

    public PlayerShop_Sales Sales { get { return sales; } }
    public SalesResult SalesResult { get { return salesResult; } }

    // Start is called before the first frame update
    void Start()
    {
        guestData = GetComponent<PlayerShop_Guest>();
        sales = GetComponent<PlayerShop_Sales>();

        guest = GetComponentInChildren<Guest>();
        salesResult = new SalesResult();

        if (player == null)
            player = FindObjectOfType<PlayerCharacter>();

        EventManager.Subscribe(EventType.Minute, GuestCheck);
        EventManager.Subscribe(EventType.Dialog, ShowDialog);
        EventManager.Subscribe(EventType.GuestExit, LeavingGuest);
        EventManager.Subscribe(EventType.CloseShop, ShowSalesResult);
        //EventManager.Subscribe(EventType.SalesFailure, LeavingGuest);
        EventManager.Publish(EventType.Work);
    }

    public SpriteRenderer itemImage;
    public int itemCode;

    public void EntryGuset()
    {        
        var newGuest = guestData.GetRandomGuest();
        //var newRequest = sales.GetRequestData(newGuest.guestId);
        var newRequest = sales.GetRequestData(1);

        guest.InitGuest(newGuest, newRequest);
        guest.EntryShop();
    }

    public void ShowDialog()
    {
        dialogBox.SetName(guest.GetGuestName());
        dialogBox.SetDialog(guest.GetRequest());
        dialogBox.SetAcceptButton(guest.GetRequestData().textAnswer1);
        dialogBox.SetRefusalButton(guest.GetRequestData().textAnswer2);
        dialogBox.ShowDialogBox();
    }

    void GuestCheck()
    {
        if (guest.IsEntry())
            return;

        /*        foreach (var guest in player.m_QuestComponet.GetTodayGuests())
                {
                    Debug.Log(guest.questName + " 삭제");
                    player.m_QuestComponet.GetTodayGuests().Remove(guest);
                }

                foreach (var guest in player.m_QuestComponet.GetTodayGuests())
                {
                    Debug.Log(guest.questName);
                }*/

        if (entryWeight >= Random.Range(0, 100))
        {
            idx = 0;
            entryWeight = weightValue[idx];
            EntryGuset();
        }
        else
        {
            entryWeight = weightValue[++idx];
        }
    }

    public void HandOverItem()
    {
        //guest.CheckItem(itemCode, 0, JewelryRank.Low);
        itemCode = 0;
    }

    public void LeavingGuest()
    {
        //guestQueue.GetGuest().gameObject.SetActive(false);
        dialogBox.ShowDialogBox(false);
        guest.ExitShop();        
    }

    void NextDialog()
    {
        var request = guest.GetRequestData();
        
        request = sales.GetRequestData(request.textNext1);
        guest.SetRequestData(request);
        dialogBox.SetAcceptButton(request.textAnswer1);
        dialogBox.SetRefusalButton(request.textAnswer2);
    }

    public void SalesSuccess(SalesData salesData, SalesResult sr)
    { 
        var request = guest.GetRequestData();
        
        request = sales.GetRequestData(request.success);
        guest.SetRequestData(request);
        dialogBox.SetAcceptButton("-1");
        dialogBox.SetRefusalButton("-1");
        dialogBox.SetDialog(request.requestScript);
        dialogBox.ShowDialogBox();
        sales.SalesSuccess(salesData, sr);
    }

    public void SalesFailure(SalesData salesData, SalesResult sr)
    {
        var request = guest.GetRequestData();
        
        request = sales.GetRequestData(request.fail);
        guest.SetRequestData(request);
        
        dialogBox.SetDialog(request.requestScript);
        dialogBox.SetAcceptButton("-1");
        dialogBox.SetRefusalButton("-1");
        dialogBox.ShowDialogBox();
        sales.SalesFailure(salesData, sr);
    }

    public void AcceptSales()
    {
        switch(guest.GetRequestData().requestType)
        {
            case 1:
                NextDialog();
                var request = guest.GetRequestData();
                dialogBox.SetDialog(request.requestScript);
                //dialogBox.SetAcceptButton(request.textAnswer1);
                break;
            case 2:
                Camera.main.GetComponent<CraftTableCameraController>().GoToCraft();
                dialogBox.ShowDialogBox(false);
                guest.AcceptSales();
                break;
        }
    }

    public void RefusalSales()
    { 
        switch(guest.GetRequestData().requestType)
        {
            case 2:
                var request = guest.GetRequestData();
        
                request = sales.GetRequestData(request.textNext2);
                guest.SetRequestData(request);
        
                dialogBox.SetDialog(request.requestScript);
                dialogBox.SetAcceptButton("-1");
                dialogBox.SetRefusalButton("-1");
                guest.RefusalSales();
                break;
        }
    }

    void ShowSalesResult()
    {
        salesOver.SetActive(true);

        totalIncome.text = "수익 : " + salesResult.totalIncome + "원";
        totalFame.text = "얻은 명성 : " + salesResult.totalFame;
        totalGuestCount.text = "찾아온 손님 : " + salesResult.totalGuestCount + "명";
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Minute, GuestCheck);
        EventManager.Unsubscribe(EventType.Dialog, ShowDialog);
        EventManager.Unsubscribe(EventType.GuestExit, LeavingGuest);
    }

    /// <summary>
    /// 매개변수로 넘긴 아이템이 요청한 아이템인지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    public bool IsRequestItem()
    {
        //CheckStuff();
        return true;
    }
}

[System.Serializable]
public class SalesResult
{
    public float totalIncome;
    public int totalFame;
    public int totalGuestCount;

    public SalesResult()
    {
        totalIncome = 0;
        totalFame = 0;
        totalGuestCount = 0;
    }

    public void ResultUpdate(float inome, int fame)
    {
        totalIncome += inome;
        totalFame += fame;
        totalGuestCount++;
    }
}