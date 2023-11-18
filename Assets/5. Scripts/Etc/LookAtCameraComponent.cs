using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraComponent : MonoBehaviour
{
	[SerializeField] bool m_LockX = true;
	[SerializeField] bool m_LockY = false;
	[SerializeField] bool m_LockZ = true;
    // Update is called once per frame
    void Update()
    {
		Vector3 t_Rotation = transform.rotation.eulerAngles;
		if (Camera.main != null)
		{
			Vector3 t_CameraRotation = Camera.main.transform.rotation.eulerAngles;
			t_Rotation.x = (m_LockY ? t_Rotation : t_CameraRotation).x;
			t_Rotation.y = (m_LockY ? t_Rotation : t_CameraRotation).y;
			t_Rotation.z = (m_LockY ? t_Rotation : t_CameraRotation).z;
		}
		transform.rotation = Quaternion.Euler(t_Rotation);
	}
}
