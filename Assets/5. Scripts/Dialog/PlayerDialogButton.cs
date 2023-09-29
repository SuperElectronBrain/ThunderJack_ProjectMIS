using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerDialogButton : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    Sprite defaultSprite;
    [SerializeField]
    Sprite mouseOverSprite;
    [SerializeField]
    Sprite mouseClickSprite;

    [SerializeField]
    TextMeshPro optionText;

    bool isClick;
    bool isMouseOver;

    public UnityAction selectOption;

    private void OnMouseOver()
    {
        isMouseOver = true;
        if (isClick) return;
        sr.sprite = mouseOverSprite;        
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        sr.sprite = defaultSprite;        
    }

    private void OnMouseDown()
    {
        isClick = true;
        sr.sprite = mouseClickSprite;
    }

    private void OnMouseUp()
    {
        isClick = false;       
        if (isMouseOver)
            selectOption?.Invoke();

        sr.sprite = defaultSprite;
    }      

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        optionText = GetComponentInChildren<TextMeshPro>();
    }

    public void SetOptionText(string newOptionText)
    {
        optionText.text = newOptionText;
    }
}
