using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeBoard : MonoBehaviour, IInteraction
{
    [SerializeField]
    TextMeshProUGUI noticeName;
    [SerializeField]
    TextMeshProUGUI noticeDescription;
    SpriteRenderer noticeImage;
    [SerializeField]
    GameObject noticeUI;

    public bool IsUsed { get; set; }

    public void Start()
    {
        /*noticeName = transform.Find("Name").GetComponent<TextMeshPro>();
        noticeDescription = transform.Find("Description").GetComponent<TextMeshPro>();*/
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(noticeUI.activeSelf)
            {
                noticeUI.GetComponent<UI_Sequence>().PlayBackwards();                
            }
            CameraEvent.Instance.ChangeCamera(CamType.Prev);
            EventManager.Publish(EventType.EndIteraction);
        }
    }

    public void SetNoticeBoard(NoticeData noticeData)
    {
        noticeName.text = noticeData.noticeName;
        noticeDescription.text = noticeData.noticeDescription;
    }

    public void Interaction(GameObject user)
    {
        IsUsed = !IsUsed;
        Debug.Log(IsUsed);

        CameraEvent.Instance.onCamBlendComplate.AddListener(ViewNoticeBoard);
        CameraEvent.Instance.ChangeCamera(CamType.NoticeBoard);
    }

    public void ViewNoticeBoard()
    {
        noticeUI.SetActive(true);
    }
}

public class NoticeData
{
    public string noticeName;
    public string noticeDescription;
    public string noticeImage;
}
