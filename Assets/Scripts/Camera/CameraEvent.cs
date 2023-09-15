using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CamType
{
    Main, Conversation, Bridge, Prev
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
    CinemachineVirtualCamera liveCam;
    [SerializeField]
    CinemachineVirtualCamera prevCam;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCamera(CamType.Main);
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
            case CamType.Bridge:
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

    void PrevCamera()
    {
        prevCam.Priority = 100;
        liveCam = prevCam;        
    }
}
