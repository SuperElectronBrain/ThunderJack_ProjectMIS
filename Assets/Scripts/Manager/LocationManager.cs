using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    Shop, Town, Park, Home
}

public class LocationManager : Singleton<LocationManager>
{
    [SerializeField]
    Transform location_Shop;
    [SerializeField]
    Transform location_Town;
    [SerializeField]
    Transform location_Park;
    [SerializeField]
    Transform location_Home;

    public Vector3 GetLocationPosition(Location location)
    {
        Vector3 locationPos = Vector3.zero;

        switch (location)
        {
            case Location.Shop:
                locationPos = location_Shop.position;
                break;
            case Location.Town:
                locationPos = location_Town.position;
                break;
            case Location.Park:
                locationPos = location_Park.position;
                break;
            case Location.Home:
                locationPos = location_Home.position;
                break;
        }

        return locationPos;
    }
}
