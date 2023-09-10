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

        guestQueue[guestCount--].ExitShop();
    }

    public void EntryGuest()
    {
        //guestQueue[guestCount++].InitGuest("name");
        guestQueue[guestCount++].EntryShop();
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
    Inventory inventory;    

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

        inventory = GameObject.FindObjectOfType<Inventory>();
        EventManager.Subscribe(EventType.Minute, GuestCheck);
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
        
        dialogBox.SetName(newGuest.guestNameKo);
        dialogBox.SetDialog(newRequest.requestScript);
        dialogBox.ShowDialogBox();
        //guestQueue.GetGuest().gameObject.SetActive(true);
        guestQueue.EntryGuest();
        StartCoroutine(Waiting());        
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
        guestQueue.ExitGuest();
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(5f);
        dialogBox.ShowDialogBox(false);
        LeavingGuest();
    }

    public void AcceptSales()
    {
        StopCoroutine(Waiting());
        dialogBox.ShowDialogBox(false);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Minute, GuestCheck);
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
