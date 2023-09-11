using UnityEngine;

[System.Serializable]
public class LocationData : MonoBehaviour
{
    public string locationName;
    public Vector3 locationTransform;

    private void Awake()
    {
        locationTransform = transform.position;
        locationName = gameObject.name;
    }
}
