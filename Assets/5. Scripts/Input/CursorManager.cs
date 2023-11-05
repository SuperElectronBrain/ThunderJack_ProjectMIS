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

    static float mouseSensitivity;
    [SerializeField] private float speed;

    private float mouseX, mouseY;
    private static Vector3 mPos;
    [SerializeField] private Vector3 cursorOffset;
    [SerializeField] private static Vector3 CursorOffset;
    
    public static UnityEvent<bool> onActive = new();
    public static UnityEvent onActiveComplate = new();
    public static bool isUsed;

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
        CursorOffset = cursorOffset;
        mouseSensitivity = speed;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorImage.rectTransform, mousePosition, canvas.worldCamera, out pos);

        /*mPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        cursorImage.rectTransform.anchoredPosition = mPos;*/

        isUsed = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        onActiveComplate?.Invoke();
        onActiveComplate?.RemoveAllListeners();
    }

    private void OnDisable()
    {
        isUsed = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CursorActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public static void SetCursorPosition(Vector3 objectPosition)
    {
        /*Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, objectPosition);
        mPos = (Vector3)screenPoint + CursorOffset;*/
        mPos = objectPosition;
        //mPos = cam.WorldToScreenPoint(objectPosition);
        //mPos.z = 0;
    }

    public static Vector3 GetCursorPosition()
    {
        //return cam.ScreenToWorldPoint(mPos - new Vector3(0, 0, cam.transform.position.z));
        return mPos;
    }

    // Update is called once per frame
    void Update()
    {
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
}