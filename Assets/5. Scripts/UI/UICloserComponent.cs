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
				gameObject.SetActive(false);
			}
		}
	}
}