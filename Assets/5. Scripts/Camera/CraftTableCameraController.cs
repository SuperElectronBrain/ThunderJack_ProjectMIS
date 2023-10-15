using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTableCameraController : MonoBehaviour
{
	//private Vector3 m_OriginPosition;
	private Quaternion m_OriginRotation;
	//[SerializeField] private Vector3 m_LeftPosition;
	[SerializeField] private Vector3 m_DownRotation;
	//Vector3 m_NextPosition;
	private Quaternion m_NextRotation;

	// Start is called before the first frame update
	void Start()
    {
		//m_OriginPosition = transform.position;
		//m_NextPosition = transform.position;
		m_OriginRotation = transform.rotation;
		m_NextRotation = transform.rotation;
	}

    // Update is called once per frame
    void Update()
    {
		float DeltaTime = Time.deltaTime;

		//Debug.Log(Screen.width - 1 + ", " + Input.mousePosition.x);
		//if (Screen.width - 1 <= Input.mousePosition.x)
		//{
		//	if (m_NextPosition != m_LeftPosition)
		//	{
		//		m_NextPosition = m_LeftPosition;
		//	}
		//}
		//else if (1 >= Input.mousePosition.x)
		//{
		//	if (m_NextPosition != m_OriginPosition)
		//	{
		//		m_NextPosition = m_OriginPosition;
		//	}
		//}

		bool bMoveable = true;

		MillStone t_MillStone = FindObjectOfType<MillStone>();
		if (t_MillStone != null)
		{
			if(t_MillStone.bProgress == true)
			{
				bMoveable = false;
			}
		}

		MeasurCup t_MeasurCup = FindObjectOfType<MeasurCup>();
		if (t_MeasurCup != null)
		{
			if (t_MeasurCup.m_GrabState == true)
			{
				bMoveable = false;
			}
		}

		if (m_NextRotation != m_OriginRotation)
		{
			Canvas canvas = FindObjectOfType<Canvas>();
			if (canvas != null)
			{
				GameObject GO = UniFunc.GetChildOfName(canvas.gameObject, "ui_sell_talk");
				if (GO != null)
				{
					GO.SetActive(false);
				}
			}
		}

		if (Screen.height - 1 <= Input.mousePosition.y)
		{
			if (m_NextRotation != m_OriginRotation)
			{
				//Canvas canvas = FindObjectOfType<Canvas>();
				//if (canvas != null)
				//{
				//	GameObject GO = UniFunc.GetChildOfName(canvas.gameObject, "ui_sell_talk");
				//	if(GO != null)
				//	{
				//		GO.SetActive(true);
				//	}
				//}

				m_NextRotation = m_OriginRotation;
			}
		}
		else if (1 >= Input.mousePosition.y)
		{
			if (m_NextRotation != Quaternion.Euler(m_DownRotation))
			{
				m_NextRotation = Quaternion.Euler(m_DownRotation);
			}
		}

		if(bMoveable == true)
		{
			CameraMovement(DeltaTime);
		}
	}

	void CameraMovement(float DeltaTime)
	{
		//transform.position = Vector3.Lerp(transform.position, m_NextPosition, 5 * DeltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, m_NextRotation, 5 * DeltaTime);
	}
}
