using System.Collections;
using System.Collections.Generic;
using RavenCraftCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookPage : MonoBehaviour
{
    [SerializeField] private TextMeshPro pageName;
    [SerializeField] private TextMeshPro pageDescription;
    [SerializeField] private TextMeshPro perfection;
    [SerializeField] private TextMeshPro memo;
    [SerializeField] private GameObject rateInfo;
    [SerializeField] private SpriteRenderer pageImage;
    private const string perfectionInit = "최고 완성도 ";

    [SerializeField] private Material elementMaterial;

    [SerializeField] private Color elementColor1;
    [SerializeField] private Color elementColor2;
    [SerializeField] private Color elementColor3;
    [SerializeField] private Color elementColor4;
    [SerializeField] private Color elementColor5;

    private readonly string materialProperty1 = "_Element1_Amount"; 
    private readonly string materialProperty2 = "_Element2_Amount"; 
    private readonly string materialProperty3 = "_Element3_Amount";
    private readonly string materialProperty4 = "_Element1_Color";
    private readonly string materialProperty5 = "_Element2_Color";
    private readonly string materialProperty6 = "_Element3_Color";

    public void Init()
    {
        rateInfo.SetActive(false);
    }
    
    public void SetPageInfo(string name, string description, float perfectionValue, Sprite sprite, string memoText = "")
    {
        if (perfectionValue == 0)
        {
            memo.text = memoText.Replace("\\n", "\n");
            memo.gameObject.SetActive(true);   
            rateInfo.SetActive(false);
        }
        else
        {
            memo.gameObject.SetActive(false);   
            rateInfo.SetActive(true);
        }
        
        pageName.text = name;
        pageDescription.text = description.Replace("\\n", "\n");
        perfection.text = perfectionInit + perfectionValue + "%";
        pageImage.sprite = sprite;
    }

    public void SetElementValues(float per1, float per2, float per3)
    {
        Debug.Log(per1);
        Debug.Log(per2);
        Debug.Log(per3);
        elementMaterial.SetFloat(materialProperty1, per1 * 0.01f);
        elementMaterial.SetFloat(materialProperty2, per2 * 0.01f);
        elementMaterial.SetFloat(materialProperty3, per3 * 0.01f);
    }

    public void SetElement(ElementType et1, ElementType et2, ElementType et3)
    {
        elementMaterial.SetColor(materialProperty4, GetColorByType(et1));
        elementMaterial.SetColor(materialProperty5, GetColorByType(et2));
        elementMaterial.SetColor(materialProperty6, GetColorByType(et3));
    }

    Color GetColorByType(ElementType et)
    {
        Color color = new Color();

        switch (et)
        {
            case ElementType.Justice:
                color = elementColor1;
                break;
            case ElementType.Wisdom:
                color = elementColor2;
                break;
            case ElementType.Nature:
                color = elementColor3;
                break;
            case ElementType.Mystic:
                color = elementColor4;
                break;
            case ElementType.Insight:
                color = elementColor5;
                break;
        }

        return color;
    }
}
