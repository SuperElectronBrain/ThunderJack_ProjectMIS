using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop_Sales : MonoBehaviour
{
    [SerializeField]
    List<RequestData> requestList = new List<RequestData>();

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
    }

    public RequestData GetRequestData(int guestId)
    {
        return requestList[guestId];
    }
}

[System.Serializable]
public class RequestData
{
    public int requestId;
    public int guestId;
    public float requestRate;
    public string requestScript;
    public int requestStuff1;
    public int requestStuff2;
}
