using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTableCameraController : MonoBehaviour
{
	private Vector3 m_OriginPosition;
	[SerializeField] private Vector3 m_LeftPosition;
	private Vector3 m_NextPosition;

	// Start is called before the first frame update
	void Start()
    {
		m_OriginPosition = transform.position;
		m_NextPosition = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
		float DeltaTime = Time.deltaTime;

		//Debug.Log(Screen.width - 1 + ", " + Input.mousePosition.x);
		if (Screen.width - 1 <= Input.mousePosition.x)
		{
			if (m_NextPosition != m_LeftPosition)
			{
				m_NextPosition = m_LeftPosition;
			}
		}
		else if (1 >= Input.mousePosition.x)
		{
			if (m_NextPosition != m_OriginPosition)
			{
				m_NextPosition = m_OriginPosition;
			}
		}
		CameraMovement(DeltaTime);
    }

	void CameraMovement(float DeltaTime)
	{
		transform.position = Vector3.Lerp(transform.position, m_NextPosition, 5 * DeltaTime);
	}
}
