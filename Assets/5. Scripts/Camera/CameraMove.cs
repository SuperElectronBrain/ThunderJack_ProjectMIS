using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraView
{
    Default, Bridge, Town, End
}

[System.Serializable]
public class CameraSetup
{
    [SerializeField]
    Vector3 position;
    [SerializeField]
    Vector3 rotation;
    [SerializeField]
    GameObject targetObj;
    [SerializeField]
    Vector2 distance;

    public Vector3 Position { get => position; }
    public Vector3 Rotation { get => rotation; }
    public GameObject TargetObj { get => targetObj; }
    public Vector2 Distance { get => distance; }

}

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Transform cameraObj;

    CameraSetup curCameraSetup;

    [SerializeField]
    float changeTime;

    [Header("CameraPosition")]
    [SerializeField]
    Vector3 curCameraOffset;
    [SerializeField]
    Vector3 cameraOffset;

    [Header("Camera Rotation")]
    [SerializeField]
    Vector3 curCameraRotation;
    [SerializeField]
    Vector3 cameraRotation;    

    [Header("Camera Location")]
    [SerializeField]
    Transform cameraLB;
    [SerializeField]
    Transform cameraRT;

    [Header("Debug")]
    [SerializeField]
    bool isDebug = false;
    [SerializeField]
    Vector3 cameraLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraObj.position = target.position;
        Camera.main.transform.localPosition = curCameraOffset;
        Camera.main.transform.rotation = Quaternion.Euler(curCameraRotation);
    }

    public void ChangeCameraView(CameraView newCameraView, CameraSetup newSetup)
    {
        StartCoroutine(StartViewChange(newSetup));
    }

    IEnumerator StartViewChange(CameraSetup newSetup)
    {
        Vector3 thisOffset = curCameraOffset;
        Vector3 thisRotation = curCameraRotation;

        while(true)
        {
/*            curCameraOffset = Vector3.Lerp(newSetup.Position, thisOffset, Tools.GetDistance(GameManager.Instance.GetPlayerTransform(), newSetup.TargetObj.transform) / newSetup.Distance.x);
            curCameraRotation = Vector3.Lerp(newSetup.Rotation, thisRotation, Tools.GetDistance(GameManager.Instance.GetPlayerTransform(), newSetup.TargetObj.transform) / newSetup.Distance.x);*/

            yield return null;
        }

        curCameraOffset = newSetup.Position;
        curCameraRotation = newSetup.Rotation;
    }

    private void OnDrawGizmos()
    {
        if (!isDebug) return;

        Gizmos.color = Color.green;
        Vector3 centerPos = cameraLB.position - cameraRT.position;
        Gizmos.DrawWireCube(centerPos, cameraLocation);
    }
}
