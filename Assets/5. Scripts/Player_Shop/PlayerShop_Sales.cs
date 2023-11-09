using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShop_Sales : MonoBehaviour
{
    [SerializeField]
    List<RequestData> requestList = new List<RequestData>();
    [SerializeField]
    Inventory inventory;

    //Request_ID,Request_Guest,Text_Type,Text_Script,Text_Next,Success,Fail,Request_Item

    // Start is called before the first frame update
    void Start()
    {        
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        var requestData = GameManager.Instance.DataBase.Parser("Request_Master2");

        foreach(var rData in requestData)
        {
            requestList.Add(
                new RequestData
                {
                    requestId = Tools.IntParse(rData["Request_ID"]),
                    guestId = Tools.IntParse(rData["Request_Guest"]),
                    requestType = Tools.IntParse(rData["Text_Type"]),
                    requestScript = rData["Text_Script1"].ToString(),
                    textNext1 = Tools.IntParse(rData["Text_Next1"]),
                    textNext2 = Tools.IntParse(rData["Text_Next2"]),
                    success = Tools.IntParse(rData["Success"]),
                    fail = Tools.IntParse(rData["Fail"]),
                    requestStuff1 = Tools.IntParse(rData["Request_Stuff_1"]),
                    requestStuff2 = Tools.IntParse(rData["Request_Stuff_2"])
                }
                );
        }

/*        salesSuccessEvent.AddListener(SalesSuccess);
        salesFailureEvent.AddListener(SalesFailure);*/
    }

    public RequestData GetRequestData(int guestId)
    {
        return requestList[guestId];
    }

    public void SalesSuccess(SalesData salesData, SalesResult salesResult)
    {
        float eventValue = 1;

        if (GameManager.Instance.GameEventManager.GetGameEventType() == GameEventType.Fame)
        {
            eventValue += GameManager.Instance.GameEventManager.GetGameEventValue();
        }

        int addFameValue = salesData.fame + (int)(salesData.fame * eventValue);

        inventory.AddAItem(((int)ItemCode.Money), salesData.perfection, salesData.money);
        inventory.AddAItem(((int)ItemCode.Honor), salesData.perfection, addFameValue);

        salesResult.ResultUpdate(salesData.money, addFameValue);
    }

    public void SalesFailure(SalesData salesData, SalesResult salesResult)
    {
        float eventValue = 1;

        if (GameManager.Instance.GameEventManager.GetGameEventType() == GameEventType.Fame)
        {
            eventValue += GameManager.Instance.GameEventManager.GetGameEventValue();
        }

        int addFameValue = -((salesData.fame + (int)(salesData.fame * eventValue)) / 2);

        inventory.AddAItem(((int)ItemCode.Money), salesData.perfection, salesData.money / 2);
        inventory.AddAItem(((int)ItemCode.Honor), salesData.perfection, addFameValue);

        salesResult.ResultUpdate(salesData.money / 2, addFameValue);
    }

    private void OnDestroy()
    {

    }
}

public class RequestStuff
{
    public int requestStuff1;
    public int requestStuff2;
}

[System.Serializable]
public class RequestData : RequestStuff
{
    public int requestId;
    public int guestId;
    public float requestRate;
    public int requestType;
    public string requestScript;
    public int textNext1;
    public int textNext2;
    public int success;
    public int fail;
}

public class SalesData
{
    public float perfection;
    public int money;
    public int fame;
}