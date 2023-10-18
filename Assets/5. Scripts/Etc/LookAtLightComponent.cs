using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtLightComponent : MonoBehaviour
{
    [SerializeField] private Light m_LightSource;

    // Start is called before the first frame update
    void Start()
    {
        if (m_LightSource == null) { m_LightSource = FindObjectOfType<Light>(); }
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 t_Rotation = transform.rotation.eulerAngles;
		if (m_LightSource != null)
		{
            t_Rotation = m_LightSource.transform.forward;
            t_Rotation.y = 0.0f;
			t_Rotation = Quaternion.LookRotation(t_Rotation, Vector3.up).eulerAngles;
		}
		transform.rotation = Quaternion.Euler(t_Rotation);
	}
}
