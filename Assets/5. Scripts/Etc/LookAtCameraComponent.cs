using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraComponent : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		Vector3 t_Rotation = transform.rotation.eulerAngles;
		if (Camera.main != null)
		{
			t_Rotation.y = Camera.main.transform.rotation.eulerAngles.y;
		}
		transform.rotation = Quaternion.Euler(t_Rotation);
	}
}
