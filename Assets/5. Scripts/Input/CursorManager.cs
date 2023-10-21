using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private static Camera cam;
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite clickCursor;
    [SerializeField] private Image cursorImage;

    public static float mouseSensitivity;
    [SerializeField] private float speed;

    private float mouseX, mouseY;
    private static Vector3 mPos;

    public static UnityEvent<bool> onActive = new();
    public static UnityEvent onActiveComplate = new();

    private void Start()
    {
        mouseSensitivity = 1;
        cam = Camera.main;
        onActive.AddListener(CursorActive);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        var mousePosition = Input.mousePosition;

        //RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorImage.rectTransform, mousePosition, canvas.worldCamera, out pos);

        /*mPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        cursorImage.rectTransform.anchoredPosition = mPos;*/

        Cursor.visible = false;
        onActiveComplate?.Invoke();
        onActiveComplate?.RemoveAllListeners();
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void CursorActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public static void SetCursorPosition(Vector3 objectPosition)
    {
        mPos = cam.WorldToScreenPoint(objectPosition);
        mPos.z = 0;
    }

    public static Vector3 GetCursorPosition()
    {
        return mPos - new Vector3(0, 0, -cam.transform.position.z);
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
