using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftTableCameraController : MonoBehaviour
{
	private Quaternion m_OriginRotation;
	[SerializeField] private Vector3 m_DownRotation;
	private Quaternion m_NextRotation;
	private bool m_IsCompleteMove = true;

	[HideInInspector] public UnityEvent<string> m_OnCompleteMove = new UnityEvent<string>();
	public static CraftTableCameraController main
	{	
		get { return FindObjectOfType<CraftTableCameraController>(); }
		set {; }
	}

	// Start is called before the first frame update
	void Start()
    {
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
				BasicItemData basicItemData = UniFunc.FindItemData(t_PlayerCharacter.m_GrabItemCode.itemCode);
				if (basicItemData != null)
				{
					if (basicItemData.itemType == ItemType.Jewelry)
					{
						if (m_NextRotation != m_OriginRotation)
						{
							m_IsCompleteMove = false;
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
		transform.rotation = Quaternion.Slerp(transform.rotation, m_NextRotation, 5 * DeltaTime);
		if(m_IsCompleteMove == false)
		{
			if (Mathf.Abs(transform.rotation.eulerAngles.magnitude - m_NextRotation.eulerAngles.magnitude) < 0.01f)
			{
				m_IsCompleteMove = true;
				m_OnCompleteMove.Invoke(m_NextRotation == m_OriginRotation ? "Up" : "Down");
			}
		}
	}

	public void GoToCraft()
	{
		m_IsCompleteMove = false;
		m_NextRotation = Quaternion.Euler(m_DownRotation);
	}
}
