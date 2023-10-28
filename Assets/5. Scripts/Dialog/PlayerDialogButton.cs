using System;
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
    
    bool isSelect;
    [SerializeField]
    private bool isDown;

    public UnityAction selectOption;

    private void OnEnable()
    {
        var camRot = Camera.main.transform.rotation;
        transform.localRotation = camRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        optionText = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        if (isDown)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                sr.sprite = mouseOverSprite;
                isSelect = true;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                sr.sprite = defaultSprite;
                isSelect = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                sr.sprite = mouseOverSprite;
                isSelect = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                sr.sprite = defaultSprite;
                isSelect = false;
            }
        }

        if (isSelect)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                selectOption?.Invoke();
            }    
        }
    }

    public void SetOptionText(string newOptionText)
    {
        optionText.text = newOptionText;
    }
}
