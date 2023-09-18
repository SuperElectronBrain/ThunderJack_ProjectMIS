using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffectZone : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vCam;
    [SerializeField]
    CinemachineTrackedDolly dolly;

    Transform playerPos;

    void Start()
    {
        dolly = vCam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CameraEvent.Instance.SetCamera(vCam);
            CameraEvent.Instance.ChangeCamera(CamType.Area);
            //vCam.Priority = 100;
            playerPos = other.transform;
            StartCoroutine(StartCameraMove());
            //dolly.m_AutoDolly.m_Enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(StartCameraMove());
            CameraEvent.Instance.ChangeCamera(CamType.Main);
            //vCam.Priority = 0;
            playerPos = null;            
            //dolly.m_AutoDolly.m_Enabled = false;
        }
    }

    IEnumerator StartCameraMove()
    {
        while(true)
        {
            if (playerPos == null)
                yield break;
            dolly.m_PathPosition = dolly.m_Path.FindClosestPoint(playerPos.position, 0, -1, 10);
            yield return null;
        }
    }
}
