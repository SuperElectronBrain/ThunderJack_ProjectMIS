using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public enum CamType
{
    Main, Conversation, Area, NoticeBoard, Prev
}

public class CameraEvent : Singleton<CameraEvent>
{
    [SerializeField]
    CinemachineBrain brain;
    [SerializeField]
    CinemachineVirtualCamera conversationCam;
    [SerializeField]
    CinemachineVirtualCamera mainCam;
    [SerializeField]
    CinemachineVirtualCamera areaCam;
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
        ChangeCamera(CamType.Main);
    }

    IEnumerator OnBlendComplate()
    {
        yield return new WaitForSeconds(0.05f);

        yield return new WaitUntil(() => brain.IsBlending == false);
        onCamBlendComplate?.Invoke();
        onCamBlendComplate = null;
    }

    public void SetCamera(CinemachineVirtualCamera newAreaCam)
    {
        areaCam = newAreaCam;
    }

    public void ChangeCamera(CamType camType)
    {
        liveCam.Priority = 10;
        if(camType != CamType.Prev)
            prevCam = liveCam;
        switch (camType)
        {
            case CamType.Main:
                MainCamera();
                break;
            case CamType.Conversation:
                ConversationCamera();                
                break;
            case CamType.Area:
                AreaCamera();
                break;
            case CamType.NoticeBoard:
                NoticeBoardCamera();
                break;
            case CamType.Prev:
                PrevCamera();
                break;
        }                
    }

    void ConversationCamera()
    {
        liveCam = conversationCam;
        conversationCam.Priority = 100;
    }

    void MainCamera()
    {
        liveCam = mainCam;
        mainCam.Priority = 100;
    }

    void AreaCamera()
    {
        liveCam = areaCam;
        areaCam.Priority = 100;
    }

    void NoticeBoardCamera()
    {
        liveCam = noticeBoardCam;
        noticeBoardCam.Priority = 100;
        StartCoroutine(OnBlendComplate());
    }

    void PrevCamera()
    {
        prevCam.Priority = 100;
        liveCam = prevCam;        
    }
}
