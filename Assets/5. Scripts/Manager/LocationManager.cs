using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        var locationData = GameManager.Instance.DataBase.Parser("Location_Master");

        foreach (var data in locationData)
        {
            int idx = locationList.Count;
            float x = Tools.FloatParse(data["Location_1"]);
            float y = Tools.FloatParse(data["Location_2"]);
            float z = Tools.FloatParse(data["Location_3"]);
            LocationType locationType = (LocationType)Tools.IntParse(data["Location_Type"]);
            locationList.Add(
                new LocationData
                {
                    locationID = Tools.IntParse(data["Location_ID"]),
                    locationName = data["Location_Name"].ToString(),
                    locationType = locationType,
                    locationPosition = new Vector3(x, y, z)
                });
            GameObject locationObj = Instantiate(locationPrefab, locationList[idx].locationPosition, Quaternion.identity);
            if(locationType == LocationType.Entrance)
            {
                locationObj.layer = LayerMask.NameToLayer("Entrance");
                locationObj.AddComponent<BoxCollider>().isTrigger = true;
                locationObj.AddComponent<Entrance>();
            }


            locationObj.name = locationList[idx].locationName;
        }
    }

    public Vector3 GetLocationPosition(int locationID)
    {
        return locationList[locationID - 1].locationPosition;
    }

    public LocationType GetLocationType(int locationID)
    {
        return locationList[locationID - 1].locationType;
    }

    public static GameObject GetObjectFromLocation(Vector3 locationPos, LocationType locationType)
    {
        LayerMask layerMask = locationType == LocationType.Interaction ? LayerMask.GetMask("InteractionObj") : LayerMask.GetMask("Entrance");

        Collider[] colliders = Physics.OverlapSphere(locationPos, 2f, layerMask);

        if(colliders.Length > 1)
        {
            throw new System.Exception("Postion : " + locationPos + " 해당 위치에 검색된 오브젝트가 너무 많습니다.");
        }
        else if(colliders.Length == 0)
        {
            throw new System.Exception("Postion : " + locationPos + " 해당 위치에 검색된 오브젝트가 없습니다.");
        }

        return colliders[0].gameObject;
    }

    public static Vector3 GetLocationRandomPosition(Vector3 locationPos)
    {
        int layer = 1 << NavMesh.GetAreaFromName("Road") | 1 << NavMesh.GetAreaFromName("Walkable");
        Vector3 randomPos = Random.insideUnitSphere * 5;
        randomPos += locationPos;

        NavMesh.SamplePosition(randomPos, out NavMeshHit navHit, 10, layer);
        return navHit.position;
    }
}

public enum LocationType
{
    None = 1, Interaction, Entrance
}

[System.Serializable]
public class LocationData
{
    public int locationID;
    public string locationName;
    public LocationType locationType;
    public Vector3 locationPosition;
}
