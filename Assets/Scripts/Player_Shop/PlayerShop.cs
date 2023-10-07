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
    int entryWeight;
    [SerializeField]
    int[] weightValue = new int[10];
    [SerializeField]
    int idx = 0;

    [SerializeField]
    Text compItem;

    [SerializeField]
    InputField gemField;
    [SerializeField]
    InputField accessoryField;
    [SerializeField]
    Button button;
    [SerializeField]
    Button button2;

    // Start is called before the first frame update
    void Start()
    {
        guestData = GetComponent<PlayerShop_Guest>();
        sales = GetComponent<PlayerShop_Sales>();

        guest = GetComponentInChildren<Guest>();

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

    public void MakeItem()
    {
        int gemId = GameManager.Instance.ItemManager.GetItemIdByName(gemField.text);
        int accessoryId = GameManager.Instance.ItemManager.GetItemIdByName(accessoryField.text);

        itemCode = GameManager.Instance.ItemManager.GetCombinationItem(gemId, accessoryId);
        compItem.text = GameManager.Instance.ItemManager.GetItemName(itemCode);
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
