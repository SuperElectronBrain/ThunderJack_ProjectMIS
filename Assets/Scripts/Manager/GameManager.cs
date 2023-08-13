using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    CameraMove cameraMove;
    [SerializeField]
    Transform Player;

    public void ChangeCameraView(CameraView newCameraView, CameraSetup newSetup)
    {
        cameraMove.ChangeCameraView(newCameraView, newSetup);
    }    

    public Transform GetPlayerTransform()
    {
        return Player;
    }
}

public class Tools
{
    public static float GetDistance(Transform myObj, Transform targetObj)
    {
        Vector3 myVector = myObj.position;
        Vector3 targetVector = targetObj.position;

        return Vector2.Distance(new Vector2(myVector.x, myVector.z), new Vector2(targetVector.x, targetVector.z));
    }
}
