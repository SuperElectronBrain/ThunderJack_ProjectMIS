using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
	[SerializeField] private Texture2D NomalCursor;
	[SerializeField] private Texture2D PressCursor;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) == true)
		{
			Cursor.SetCursor(PressCursor, Vector2.up * PressCursor.height, CursorMode.ForceSoftware);
		}
		else if(Input.GetMouseButtonUp(0) == true)
		{
			if (NomalCursor != null)
			{
				Cursor.SetCursor(NomalCursor, Vector2.up * NomalCursor.height, CursorMode.ForceSoftware);
			}
		}
	}
}
