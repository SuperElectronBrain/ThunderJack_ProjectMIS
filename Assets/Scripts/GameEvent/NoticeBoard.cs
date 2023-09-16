using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoticeBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshPro noticeName;
    [SerializeField]
    TextMeshPro noticeDescription;
    SpriteRenderer noticeImage;

    public void Start()
    {
        noticeName = transform.Find("Name").GetComponent<TextMeshPro>();
        noticeDescription = transform.Find("Description").GetComponent<TextMeshPro>();
    }

    public void SetNoticeBoard(NoticeData noticeData)
    {
        noticeName.text = noticeData.noticeName;
        noticeDescription.text = noticeData.noticeDescription;
    }
}

public class NoticeData
{
    public string noticeName;
    public string noticeDescription;
    public string noticeImage;
}
