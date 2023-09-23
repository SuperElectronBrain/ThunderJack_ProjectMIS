using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshPro noticeName;
    [SerializeField]
    TextMeshPro noticeDescription;
    SpriteRenderer noticeImage;
    [SerializeField]
    UI_Sequence noticeUI;

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

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            noticeUI.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            
        }
    }
}

public class NoticeData
{
    public string noticeName;
    public string noticeDescription;
    public string noticeImage;
}
