using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public enum CamType
{
    Conversation, NoticeBoard, Enter, Prev
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
    [SerializeField]
    CinemachineVirtualCamera enterCam;
    [SerializeField]
    CinemachineTargetGroup targetGroup;

    public UnityEvent onCamBlendComplate;

    // Start is called before the first frame update
    void Start()
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
            case CamType.Enter:
                EnterCamera();
                break;
        }                
    }

    public void ChangeCamera(CinemachineVirtualCamera vCam)
    {
        prevCam = liveCam;
        liveCam = vCam;
        liveCam.Priority = 100;
        if(prevCam!= null)
            prevCam.Priority = 10;        
    }

    public bool IsIgnoreCam(CinemachineVirtualCamera vCam)
    {
        if (prevCam != vCam || vCam == null)
            return false;
        return true;
    }

    public void SetTarget(Transform target)
    {
        if(targetGroup.FindMember(target) == -1)
            targetGroup.AddMember(target, 1, 5);
        else
            targetGroup.RemoveMember(target);
    }

    void ConversationCamera()
    {
        liveCam = conversationCam;
        conversationCam.Priority = 100;
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

    void EnterCamera()
    {
        liveCam = enterCam;
        enterCam.Priority = 100;
    }
}
