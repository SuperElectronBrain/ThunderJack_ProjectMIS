using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloserComponent : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if(gameObject.activeSelf == true)
		{
			if (Input.GetKeyDown(KeyCode.Escape) == true)
			{
				if(CloseableUICounter.GetCloseableUICounter().GetRecentlyOpenedUI() == this)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	private void OnEnable()
	{
		CloseableUICounter.GetCloseableUICounter().AddOpenUI(this);
	}

	private void OnDisable()
	{
		CloseableUICounter.GetCloseableUICounter().RemoveCloseUI(this);
	}
}