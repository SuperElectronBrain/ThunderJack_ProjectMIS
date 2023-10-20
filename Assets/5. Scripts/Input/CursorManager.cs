using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultCursor;
    [SerializeField]
    private Sprite clickCursor;
    [SerializeField]
    private Image cursorImage;

    public static float mouseSensitivity;
    [SerializeField] private float speed;

    private float mouseX, mouseY;
    private static Vector3 mPos;

    public static UnityEvent<bool> onActive = new();
    
    private void Start()
    {
        mouseSensitivity = 1;
        onActive.AddListener(CursorActive);
    }

    private void OnEnable()
    {               
        var mousePosition = Input.mousePosition;

        mPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));
        mPos.z = 0;
        cursorImage.rectTransform.position = mPos;

        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    public void CursorActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public static Vector3 GetCursorPosition()
    {
        return mPos;
    }

    // Update is called once per frame
    void Update()
    {
        mouseSensitivity = speed;
        if (Input.GetMouseButtonDown(0))
        {
            cursorImage.sprite = clickCursor;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gameObject.SetActive(false);
        }
        
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        mPos += new Vector3(mouseX, mouseY, 0);

        cursorImage.rectTransform.position = mPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new Exception("충돌");
    }
}
