using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
    private RectTransform rectTransform;
	[SerializeField] private RectTransform Idle;
	[SerializeField] private RectTransform Click;

	// Start is called before the first frame update
	void Start()
    {
       // base.Start();
		rectTransform = GetComponent<RectTransform>();

		Canvas canvas = UniFunc.GetParentComponent<Canvas>(rectTransform);
		rectTransform.transform.SetParent(canvas.transform);
		rectTransform.SetAsLastSibling();
	}

    // Update is called once per frame
    void Update()
    {
		if (rectTransform != null) 
		{
			rectTransform.position = Input.mousePosition;
			if (Cursor.visible == true) { Cursor.visible = false; }

			if (Input.GetMouseButtonUp(0) == true)
			{
				if (Idle != null) { if (Idle.gameObject.activeSelf == false) { Idle.gameObject.SetActive(true); } }
				if (Click != null) { if (Click.gameObject.activeSelf == true) { Click.gameObject.SetActive(false); } }
			}
			else if (Input.GetMouseButtonDown(0) == true)
			{
				if (Idle != null) { if (Idle.gameObject.activeSelf == true) { Idle.gameObject.SetActive(false); } }
				if (Click != null) { if (Click.gameObject.activeSelf == false) { Click.gameObject.SetActive(true); } }
			}
		}
		else if (rectTransform == null) { Cursor.visible = true; gameObject.SetActive(false); }
	}
}
