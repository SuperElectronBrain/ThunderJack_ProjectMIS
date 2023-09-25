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

    public void Interaction()
    {
        Debug.Log(gameObject.name + " 상호작용");
        noticeUI.SetActive(true);
    }
}

public class NoticeData
{
    public string noticeName;
    public string noticeDescription;
    public string noticeImage;
}
