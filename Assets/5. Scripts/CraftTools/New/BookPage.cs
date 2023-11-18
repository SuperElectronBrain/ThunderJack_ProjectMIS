using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookPage : MonoBehaviour
{
    [SerializeField] private TextMeshPro pageName;
    [SerializeField] private TextMeshPro pageDescription;
    [SerializeField] private TextMeshPro perfection;
    [SerializeField] private GameObject memo;
    [SerializeField] private GameObject rateInfo;
    [SerializeField] private SpriteRenderer pageImage;
    private const string perfectionInit = "최고 완성도 ";

    [SerializeField] private Sprite element;

    public void SetPageInfo(string name, string description, float perfectionValue, Sprite sprite)
    {
        memo.SetActive(false);
        rateInfo.SetActive(true);
        pageName.text = name;
        pageDescription.text = description;
        perfection.text = perfectionInit + perfectionValue + "%";
        pageImage.sprite = sprite;
    }

    public void SetElementValues(float per1, float per2, float per3)
    {
        /*element1.fillAmount = per1 * 0.01f;
        element2.fillAmount = per2 * 0.01f;
        element3.fillAmount = per3 * 0.01f;*/
    }
}
