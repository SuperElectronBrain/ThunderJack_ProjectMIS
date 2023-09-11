using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircularGuestQueue
{
    [SerializeField]
    Guest[] guestQueue;
    [SerializeField]
    int maxGuestCount;
    [SerializeField]
    int guestCount;

    public CircularGuestQueue(Guest[] guests, int maxGuestCount = 3)
    {
        this.maxGuestCount = maxGuestCount;
        //guestQueue = new Guest[maxGuestCount];
        guestQueue = guests;
    }   

    public void ExitGuest()
    {
        var temp = guestQueue[maxGuestCount - 1];
        for (int i = 0; i < guestQueue.Length - 1; i++)
        {
            if(i == 0)
                guestQueue[maxGuestCount - 1] = guestQueue[i];
            else
                guestQueue[i - 1] = guestQueue[i];
        }
        guestQueue[maxGuestCount - 2] = temp;

        guestCount--;
        //guestQueue[guestCount--].ExitShop();
        if (guestCount != 0)
            GetGuest().FirstGuest();
    }

    public void EntryGuest(GuestData newGuest, RequestData newRequest)
    {        
        guestQueue[guestCount].InitGuest(newGuest, newRequest);
        guestQueue[guestCount].EntryShop();
        if (guestCount == 0)
            guestQueue[guestCount].FirstGuest();
        guestCount++;
    }

    public bool IsFull()
    {
        return guestCount == maxGuestCount;
    }

    public Guest GetGuest()
    {
        return guestQueue[0];
    }
}

public class PlayerShop : MonoBehaviour
{
    PlayerShop_Guest guest;
    PlayerShop_Sales sales;
    [SerializeField]
    DialogueBox dialogBox;

    [SerializeField]
    CircularGuestQueue guestQueue;

    [SerializeField]
    int entryWeight;
    [SerializeField]
    int[] weightValue = new int[10];
    [SerializeField]
    int idx = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        guest = GetComponent<PlayerShop_Guest>();
        sales = GetComponent<PlayerShop_Sales>();

        var guests = transform.GetComponentsInChildren<Guest>();
        guestQueue = new CircularGuestQueue(guests, guests.Length);

        EventManager.Subscribe(EventType.Minute, GuestCheck);
        EventManager.Subscribe(EventType.Dialog, ShowDialog);
        EventManager.Subscribe(EventType.GuestExit, LeavingGuest);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EntryGuset();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            guestQueue.ExitGuest();
        }
    }

    public void EntryGuset()
    {        
        var newGuest = guest.GetRandomGuest();
        var newRequest = sales.GetRequestData(newGuest.guestId);

        guestQueue.EntryGuest(newGuest, newRequest);

        //guestQueue.GetGuest().gameObject.SetActive(true);       
    }

    public void ShowDialog()
    {
        dialogBox.SetName(guestQueue.GetGuest().GetGuestName());
        dialogBox.SetDialog(guestQueue.GetGuest().GetRequest());
        dialogBox.ShowDialogBox();
    }

    void GuestCheck()
    {
        if (guestQueue.IsFull())
            return;

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

    public void LeavingGuest()
    {
        //guestQueue.GetGuest().gameObject.SetActive(false);
        dialogBox.ShowDialogBox(false);
        guestQueue.ExitGuest();        
    }

    public void AcceptSales()
    {
        guestQueue.GetGuest().AcceptSales();
        dialogBox.ShowDialogBox(false);
    }

    public void RefusalSales()
    {
        guestQueue.GetGuest().RefusalSales();
        dialogBox.ShowDialogBox(false);
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

/*    bool CheckStuff()
    {

    }*/
}
