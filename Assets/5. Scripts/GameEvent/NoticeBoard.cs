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

    [SerializeField]
    Cinemachine.CinemachineVirtualCamera vCam;

    [SerializeField]
    UnityEngine.Events.UnityEvent onInteractionEvent;
	[SerializeField] private AudioSource m_InteractionSound;

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
            //CameraEvent.Instance.onCamBlendComplate.AddListener(ViewNoticeBoard);
            CameraEvent.Instance.ChangeCam(vCam);
            onInteractionEvent?.Invoke();
			if (m_InteractionSound != null) { if (m_InteractionSound.isPlaying == false) m_InteractionSound.Play(); }
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
