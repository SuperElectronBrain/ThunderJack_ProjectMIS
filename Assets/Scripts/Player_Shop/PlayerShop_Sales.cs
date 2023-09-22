using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum JewelryGrade
{

}

public class PlayerShop_Sales : MonoBehaviour
{
    [SerializeField]
    List<RequestData> requestList = new List<RequestData>();

    public UnityEvent<float, int, int> salesSuccessEvent;
    public UnityEvent<float, int, int> salesFailureEvent;    

    // Start is called before the first frame update
    void Start()
    {
        var requestData = GameManager.Instance.DataBase.Parser("Request_Master");

        foreach(var rData in requestData)
        {
            requestList.Add(
                new RequestData
                {
                    requestId = Tools.IntParse(rData["Request_ID"]),
                    guestId = Tools.IntParse(rData["Guest_ID"]),
                    requestRate = Tools.FloatParse(rData["Request_Rate"]),
                    requestScript = rData["Request_Script"].ToString(),
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

    public static void SalesSuccess(SalesData salesData)
    {
        FindObjectOfType<Inventory>().AddAItem(((int)ItemCode.Money), salesData.perfection, salesData.money);
        FindObjectOfType<Inventory>().AddAItem(((int)ItemCode.Honor), salesData.perfection, salesData.fame);
    }

    public static void SalesFailure(SalesData salesData)
    {
        FindObjectOfType<Inventory>().AddAItem(((int)ItemCode.Money), salesData.perfection, salesData.money);
        FindObjectOfType<Inventory>().AddAItem(((int)ItemCode.Honor), salesData.perfection, -(salesData.fame / 2));
    }

    private void OnDestroy()
    {
        salesSuccessEvent.RemoveAllListeners();
        salesFailureEvent.RemoveAllListeners();
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
    public string requestScript;
}

public class SalesData
{
    public float perfection;
    public int money;
    public int fame;
}