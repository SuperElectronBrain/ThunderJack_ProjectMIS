using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{    
	[SerializeField]
	Windmill windmill;

	// Update is called once per frame
	void FixedUpdate()
	{
		if (transform.childCount > 0)
		{
			if (transform.GetChild(1) != null)
			{ 
				transform.GetChild(1).Rotate(0f, 0f, windmill.smallSpinSpeed * Time.fixedDeltaTime, Space.Self);
			}
		}
		if (transform.childCount > 1)
		{
			if (transform.GetChild(2) != null)
			{
				transform.GetChild(2).Rotate(0f, 0f, windmill.bigSpinSpeed * Time.fixedDeltaTime, Space.Self);
			}
		}
	}
}
