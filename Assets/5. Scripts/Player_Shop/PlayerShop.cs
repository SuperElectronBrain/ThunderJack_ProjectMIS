using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    SalesResult salesResult;

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
        //EventManager.Subscribe(EventType.SalesFailure, LeavingGuest);
        EventManager.Publish(EventType.Work);
    }

    public SpriteRenderer itemImage;
    public int itemCode;

    public void EntryGuset()
    {        
        var newGuest = guestData.GetRandomGuest();
        var newRequest = sales.GetRequestData(newGuest.guestId);

        guest.InitGuest(newGuest, newRequest);
        guest.EntryShop();
    }

    public void ShowDialog()
    {
        dialogBox.SetName(guest.GetGuestName());
        dialogBox.SetDialog(guest.GetRequest());
        dialogBox.ShowDialogBox();
    }

    void GuestCheck()
    {
        if (guest.IsEntry())
            return;

/*        foreach (var guest in player.m_QuestComponet.GetTodayGuests())
        {
            Debug.Log(guest.questName + " ����");
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
        guest.CheckItem(itemCode, 0);
        itemCode = 0;
    }

    public void LeavingGuest()
    {
        //guestQueue.GetGuest().gameObject.SetActive(false);
        dialogBox.ShowDialogBox(false);
        guest.ExitShop();        
    }

    public void AcceptSales()
    {
        guest.AcceptSales();
        dialogBox.ShowDialogBox(false);
    }

    public void RefusalSales()
    {
        guest.RefusalSales();
        dialogBox.ShowDialogBox(false);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Minute, GuestCheck);
        EventManager.Unsubscribe(EventType.Dialog, ShowDialog);
        EventManager.Unsubscribe(EventType.GuestExit, LeavingGuest);
    }

    /// <summary>
    /// �Ű������� �ѱ� �������� ��û�� ���������� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool IsRequestItem()
    {
        //CheckStuff();
        return true;
    }
}

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
        totalIncome = inome;
        totalFame = fame;
        totalGuestCount++;
    }
}