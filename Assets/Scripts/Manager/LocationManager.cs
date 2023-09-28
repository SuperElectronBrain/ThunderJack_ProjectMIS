using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NavType
{
    Shop, Town, Park
}

public class LocationManager : MonoBehaviour
{
    [SerializeField]
    List<LocationData> locationList;

    [SerializeField]
    GameObject locationPrefab;

    // Start is called before the first frame update
    void Start()
    {
        locationList = new List<LocationData>();

        var locationData = GameManager.Instance.DataBase.Parser("LocationList");

        foreach (var data in locationData)
        {
            int idx = locationList.Count;
            float x = Tools.FloatParse(data["Location_1"]);
            float y = Tools.FloatParse(data["Location_2"]);
            float z = Tools.FloatParse(data["Location_3"]);
            locationList.Add(
                new LocationData
                {
                    locationID = Tools.IntParse(data["Location_ID"]),
                    locationName = data["Location_Name"].ToString(),
                    locationPosition = new Vector3(x, y, z)
                });
            GameObject locationObj = Instantiate(locationPrefab, locationList[idx].locationPosition, Quaternion.identity);
            locationObj.name = locationList[idx].locationName;
        }
    }

    public Vector3 GetLocationPosition(int locationID)
    {
        //Debug.Log(((LocationName)locationName).ToString() + " : " + locationData[((LocationName)locationName).ToString()].locationTransform);
        //return locationData[((LocationName)locationName).ToString()].locationTransform;s
        //gameObject.name = locationList[locationName].locationName;
        return locationList[locationID].locationPosition;
    }

    public Vector3 GetTargetPostion(int locationID)
    {
        return locationList[locationID].locationPosition;
    }

    public static Vector3 GetLocationRandomPosition(Vector3 locationPos)
    {
        int layer = 1 << NavMesh.GetAreaFromName("Road") << NavMesh.GetAreaFromName("Walkable");
        Vector3 randomPos = Random.insideUnitSphere * 5;
        randomPos += locationPos;

        NavMesh.SamplePosition(randomPos, out NavMeshHit navHit, 10, layer);
        return navHit.position;
    }
}

public class LocationData
{
    public int locationID;
    public string locationName;
    public Vector3 locationPosition;
}
