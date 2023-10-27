using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{    
	[SerializeField]
	Windmill windmill;

	[SerializeField] 
	private bool isSingle;

	// Update is called once per frame
	void FixedUpdate()
	{
		if (isSingle)
		{
			if (transform.childCount > 1)
			{ 
				transform.GetChild(1).Rotate(0f, 0f, windmill.smallSpinSpeed * Time.fixedDeltaTime, Space.Self);
			}
		}
		else
		{
			if (transform.childCount > 2)
			{
				transform.GetChild(1).Rotate(0f, 0f, windmill.smallSpinSpeed * Time.fixedDeltaTime, Space.Self);
				transform.GetChild(2).Rotate(0f, 0f, windmill.bigSpinSpeed * Time.fixedDeltaTime, Space.Self);
			}
		}
	}
}
