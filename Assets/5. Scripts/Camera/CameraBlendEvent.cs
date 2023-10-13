using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraBlendEvent : MonoBehaviour
{
    CinemachineVirtualCameraBase vCam;

    public UnityEvent onBlendComplate;

    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    public void StartBlend()
    {
        if(onBlendComplate != null)
            StartCoroutine(StartBlendCo());
    }

    IEnumerator StartBlendCo()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        yield return new WaitForSeconds(0.05f);

        yield return new WaitUntil(() => brain.IsBlending == false);
        if(CinemachineCore.Instance.IsLive(vCam))
            onBlendComplate?.Invoke();
    }
}
