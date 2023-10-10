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
    UiComponent noticeUI;

    public UnityEngine.Events.UnityEvent onInteraction;

    public bool IsUsed { get; set; }

    public void Start()
    {
        /*noticeName = transform.Find("Name").GetComponent<TextMeshPro>();
        noticeDescription = transform.Find("Description").GetComponent<TextMeshPro>();*/
    }

    public void SetNoticeBoard(NoticeData noticeData)
    {
        noticeName.text = noticeData.noticeName;
        noticeDescription.text = noticeData.noticeDescription;
    }

    public void Interaction(GameObject user)
    {
        if(user.GetComponent<NPC>())
        {

        }
        else
        {
            CameraEvent.Instance.onCamBlendComplate.AddListener(ViewNoticeBoard);
            onInteraction?.Invoke();
            //CameraEvent.Instance.ChangeCamera(CamType.NoticeBoard);
        }        
    }

    public void ViewNoticeBoard()
    {
        noticeUI.ActiveUI();
    }
}

public class NoticeData
{
    public string noticeName;
    public string noticeDescription;
    public string noticeImage;
}
