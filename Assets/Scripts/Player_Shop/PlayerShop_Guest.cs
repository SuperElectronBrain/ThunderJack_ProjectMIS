using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop_Guest : MonoBehaviour
{
    [SerializeField]
    List<GuestData> guestList = new List<GuestData>();
    
    // Start is called before the first frame update
    void Start()
    {
        var guestData = GameManager.Instance.DataBase.Parser("Guest_Master");

        foreach(var guest in guestData)
        {
            guestList.Add(
                new GuestData
                {
                    guestId = Tools.IntParse(guest["Guest_ID"]),
                    guestNameEg = guest["Guest_Name_Eg"].ToString(),
                    guestNameKo = guest["Guest_Name_Ko"].ToString(),
                    guestGrade = Tools.IntParse(guest["Guest_Grade"]),
                    guestRate = Tools.FloatParse(guest["Guest_Rate"])
                }
                );
        }
    }

    public GuestData GetRandomGuest()
    {
        return guestList[Random.Range(0, guestList.Count)];
    }
}

[System.Serializable]
public class GuestData
{
    public int guestId;
    public string guestNameEg;
    public string guestNameKo;
    public int guestGrade;
    public float guestRate;
}