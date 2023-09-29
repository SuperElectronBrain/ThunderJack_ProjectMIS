using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDirectionZone : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vCam;
    [SerializeField]
    CinemachineTrackedDolly dolly;

    Transform playerPos;

    [SerializeField]
    bool isReturnByExit;

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
            //StartCoroutine(StartCameraMove());
            if(dolly != null)
                dolly.m_AutoDolly.m_Enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isReturnByExit)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            //StopCoroutine(StartCameraMove());
            CameraEvent.Instance.ChangeCamera(CamType.Main);
            //vCam.Priority = 0;
            playerPos = null;
            if (dolly != null)
                dolly.m_AutoDolly.m_Enabled = false;
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
