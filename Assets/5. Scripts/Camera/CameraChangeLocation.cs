using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeLocation : MonoBehaviour
{
    [SerializeField]
    CameraSetup cameraSetup;
    [SerializeField]
    CameraView cameraView;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Character")) return;
        Debug.Log("¿‘¿Â");
    }

    private void OnTriggerExit(Collider collision)
    {
        
    }
}
