using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDirectionZone : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vCam;
    CinemachineTrackedDolly dolly;

    [SerializeField]
    CinemachineVirtualCamera ignoreCam;

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
            if(CameraEvent.Instance.IsIgnoreCam(ignoreCam))
                return;

            CameraEvent.Instance.ChangeCamera(vCam);
            playerPos = other.transform;
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
