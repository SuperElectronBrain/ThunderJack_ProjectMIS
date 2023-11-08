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

		if (Screen.height - 1 <= Input.mousePosition.y)
		{
			PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
			if (t_PlayerCharacter != null)
			{
				if(t_PlayerCharacter.m_GrabItemCode != null)
				{
					if(t_PlayerCharacter.m_GrabItemCode.itemCode >= 28 && t_PlayerCharacter.m_GrabItemCode.itemCode <= 57)
					{
						if (m_NextRotation != m_OriginRotation)
						{
							m_NextRotation = m_OriginRotation;
						}
					}
				}
			}
		}
		//else if (1 >= Input.mousePosition.y)
		//{
		//	PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		//	if (t_PlayerCharacter != null)
		//	{
		//		if (t_PlayerCharacter.m_GrabItemCode.itemCode >= 28 && t_PlayerCharacter.m_GrabItemCode.itemCode <= 57)
		//		{
		//			if (m_NextRotation != Quaternion.Euler(m_DownRotation))
		//			{
		//				m_NextRotation = Quaternion.Euler(m_DownRotation);
		//			}
		//		}
		//	}
		//}

		CameraMovement(DeltaTime);

	}

	void CameraMovement(float DeltaTime)
	{
		//transform.position = Vector3.Lerp(transform.position, m_NextPosition, 5 * DeltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, m_NextRotation, 5 * DeltaTime);
	}

	public void GoToCraft()
	{
		m_NextRotation = Quaternion.Euler(m_DownRotation);
	}
}
