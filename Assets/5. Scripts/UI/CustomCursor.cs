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
			Cursor.SetCursor(PressCursor, new Vector2(PressCursor.width * 0.33333f, PressCursor.height * 0.83333f), CursorMode.ForceSoftware);
		}
		else if(Input.GetMouseButtonUp(0) == true)
		{
			if (NomalCursor != null)
			{
				Cursor.SetCursor(NomalCursor, new Vector2(PressCursor.width * 0.33333f, PressCursor.height * 0.83333f), CursorMode.ForceSoftware);
			}
		}
	}
}
