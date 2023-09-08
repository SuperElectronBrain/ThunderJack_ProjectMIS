using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : MonoBehaviour
{
    PlayerShop_Guest guest;
    PlayerShop_Sales sales;
    [SerializeField]
    DialogueBox dialogBox;
    [SerializeField]
    Inventory inventory;

    //큐로 게스트 만들기

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

        inventory = GameObject.FindObjectOfType<Inventory>();
        EventManager.Subscribe(EventType.Minute, GuestCheck);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            EntryGuset();
        }
    }

    public void EntryGuset()
    {
        var newGuest = guest.GetRandomGuest();

        var newRequest = sales.GetRequestData(newGuest.guestId);
        
        dialogBox.SetName(newGuest.guestNameKo);
        dialogBox.SetDialog(newRequest.requestScript);
        dialogBox.ShowDialogBox();
        StartCoroutine(Waiting());
    }    

    void GuestCheck()
    {
        if(entryWeight >= Random.Range(0, 100))
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

    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1f);
        dialogBox.ShowDialogBox(false);
        yield return new WaitForSeconds(5f);
        LeavingGuest();
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
