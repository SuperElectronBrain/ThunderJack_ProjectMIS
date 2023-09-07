using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationName
{
    Shop, Town, Park
}

public class LocationManager : MonoBehaviour
{
    [SerializeField]
    List<LocationData> locationList;
    Dictionary<string, LocationData> locationData;

    // Start is called before the first frame update
    void Start()
    {
        locationData = new Dictionary<string, LocationData>();

        foreach (var location in locationList)
        {
            locationData.Add(location.name, location);
        }
    }

    public Vector3 GetLocationPosition(int locationName)
    {
        //Debug.Log(((LocationName)locationName).ToString() + " : " + locationData[((LocationName)locationName).ToString()].locationTransform);
        //return locationData[((LocationName)locationName).ToString()].locationTransform;
        return locationList[locationName].locationTransform;
    }
}
