using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LocationName
{
    Shop, Town, Park
}

public class LocationManager : MonoBehaviour
{
    [SerializeField]
    List<LocationData> locationList;
    [SerializeField]
    List<LocationData> targetLocationList;
    Dictionary<string, LocationData> locationData;

    // Start is called before the first frame update
    void Start()
    {
        locationData = new Dictionary<string, LocationData>();
        locationList = new();
        targetLocationList = new();

        foreach (var location in GameObject.FindGameObjectsWithTag("Land"))
        {
            locationList.Add(location.GetComponent<LocationData>());
        }
        foreach (var location in GameObject.FindGameObjectsWithTag("TargetLocation"))
        {
            targetLocationList.Add(location.GetComponent<LocationData>());
        }

        foreach (var location in locationList)
        {
            locationData.Add(location.name, location);
        }
    }

    public Vector3 GetLocationPosition(int locationName)
    {
        //Debug.Log(((LocationName)locationName).ToString() + " : " + locationData[((LocationName)locationName).ToString()].locationTransform);
        //return locationData[((LocationName)locationName).ToString()].locationTransform;
        //Debug.Log(locationList[locationName].gameObject.name + " " + locationList[locationName].locationTransform);
        //gameObject.name = locationList[locationName].locationName;
        return locationList[locationName].locationTransform;
    }

    public static Vector3 GetLocationRandomPosition(Vector3 locationPos)
    {        
        int layer = 1 << NavMesh.GetAreaFromName("Road");
        Vector3 randomPos = Random.insideUnitSphere * 5;
        randomPos += locationPos;

        NavMesh.SamplePosition(randomPos, out NavMeshHit navHit, 10, layer);
        return navHit.position;
    }
}
