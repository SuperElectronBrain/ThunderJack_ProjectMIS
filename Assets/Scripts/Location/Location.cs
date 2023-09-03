using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField]
    List<LocationData> locationList;
    Dictionary<string, LocationData> locationData;

    // Start is called before the first frame update
    void Start()
    {
        locationData = new Dictionary<string, LocationData>();

        foreach(var location in locationList)
        {
            locationData.Add(location.locationName, location);
        }
    }

    public Vector3 GetLocationPosition(string locationName)
    {
        return locationData[locationName].locationTransform;
    }
}