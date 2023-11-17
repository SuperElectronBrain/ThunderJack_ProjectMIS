using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pageName;
    [SerializeField] private TextMeshProUGUI pageDescription;
    [SerializeField] private TextMeshProUGUI perfection;
    [SerializeField] private Image pageImage;
    private string perfectionInit = "최고 완성도 ";

    [SerializeField] private Image element1;
    [SerializeField] private Image element2;
    [SerializeField] private Image element3;

    public void SetPageInfo(string name, string description, float perfectionValue, Sprite sprite)
    {
        pageName.text = name;
        pageDescription.text = description;
        perfection.text = perfectionInit + perfectionValue + "%";
        pageImage.sprite = sprite;
    }

    public void SetElementValues(float per1, float per2, float per3)
    {
        element1.fillAmount = per1 * 0.01f;
        element2.fillAmount = per2 * 0.01f;
        element3.fillAmount = per3 * 0.01f;
    }
}
