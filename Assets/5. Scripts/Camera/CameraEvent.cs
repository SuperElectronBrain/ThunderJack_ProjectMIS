using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public enum CamType
{
    Conversation, NoticeBoard, Prev
}

public class CameraEvent : Singleton<CameraEvent>
{
    [SerializeField]
    CinemachineBrain brain;
    [SerializeField]
    CinemachineVirtualCamera conversationCam;
    [SerializeField]
    CinemachineVirtualCamera liveCam;
    [SerializeField]
    CinemachineVirtualCamera prevCam;
    [SerializeField]
    CinemachineVirtualCamera noticeBoardCam;

    public UnityEvent onCamBlendComplate;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    IEnumerator OnBlendComplate()
    {
        yield return new WaitForSeconds(0.05f);

        yield return new WaitUntil(() => brain.IsBlending == false);
        onCamBlendComplate?.Invoke();
        onCamBlendComplate?.RemoveAllListeners();
    }

    public void ChangeCamera(CamType camType)
    {
        liveCam.Priority = 10;
        if(camType != CamType.Prev)
            prevCam = liveCam;
        switch (camType)
        {
            case CamType.Conversation:
                ConversationCamera();                
                break;
            case CamType.NoticeBoard:
                NoticeBoardCamera();
                break;
            case CamType.Prev:
                PrevCamera();
                break;
        }                
    }

    public void ChangeCamera(CinemachineVirtualCamera vCam)
    {
        prevCam = liveCam;
        liveCam = vCam;
        liveCam.Priority = 100;
        if (prevCam != null && prevCam != vCam)
            prevCam.Priority = 10;        
    }

    public bool IsIgnoreCam(List<CinemachineVirtualCamera> vCams)
    {
        if (vCams.Count == 0)
            return false;

        for(int i = 0; i < vCams.Count; i++)
        {
            if (liveCam == vCams[i])
                return true;
        }

        return false;
        
    }

    void ConversationCamera()
    {
        liveCam = conversationCam;
        conversationCam.Priority = 100;
        StartCoroutine(OnBlendComplate());
    }

    public void ChangeCam(CinemachineVirtualCamera vCam)
    {
        liveCam.Priority = 10;
        prevCam = liveCam;
        liveCam = vCam;
        liveCam.Priority = 100;
        StartCoroutine(OnBlendComplate());
    }

    void NoticeBoardCamera()
    {
        liveCam = noticeBoardCam;
        noticeBoardCam.Priority = 100;
        StartCoroutine(OnBlendComplate());
    }

    void PrevCamera()
    {
        StopAllCoroutines();
        onCamBlendComplate?.RemoveAllListeners();
        prevCam.Priority = 100;
        liveCam = prevCam;
    }
}
