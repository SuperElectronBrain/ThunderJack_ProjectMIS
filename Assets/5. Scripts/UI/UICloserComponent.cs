using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloserComponent : MonoBehaviour
{
	public KeyCode m_Key;

	// Update is called once per frame
	void Update()
	{
		if (gameObject.activeSelf == true)
		{
			if (Input.GetKeyDown(KeyCode.Escape) == true)
			{
				if (CloseableUICounter.GetCloseableUICounter().GetRecentlyOpenedUI() == this)
				{
					gameObject.SetActive(false);
				}
			}
			if (Input.GetKeyDown(m_Key) == true)
			{
				if (m_Key == KeyCode.Escape)
				{
					if (CloseableUICounter.GetCloseableUICounter().GetRecentlyOpenedUI() != this) { return; }
				}

				gameObject.SetActive(false);
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