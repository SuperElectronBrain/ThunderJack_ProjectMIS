using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : MonoBehaviour
{
    PlayerShop_Guest guest;
    PlayerShop_Sales sales;
    [SerializeField]
    DialogueBox dialogBox;

    // Start is called before the first frame update
    void Start()
    {
        guest = GetComponent<PlayerShop_Guest>();
        sales = GetComponent<PlayerShop_Sales>();
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
