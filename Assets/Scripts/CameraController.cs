using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 10.0f;
	//[SerializeField] private float mouseMoveSpeed = 100.0f;
	[SerializeField] private float distance = 5.0f;
	[SerializeField] private float angle = 15.0f;
	//[SerializeField] private float zoomSpeed = 1000.0f;

	//[SerializeField] private SplineContainer m_SplineContainer;
	//public GameObject mousePointingTarget;
	[HideInInspector] public GameObject cameraTarget;
	[HideInInspector] public PlayerCharacter m_PlayerCharacter;
	private Vector3 m_NextPosition;
	private Quaternion m_NextRotation;

	// Start is called before the first frame update
	void Start()
	{
		m_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if (m_PlayerCharacter != null)
		{
			SetCameraPosition(m_PlayerCharacter.gameObject);
		}

		m_NextPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (m_PlayerCharacter != null)
		{
			if (m_PlayerCharacter.m_CPAComponent != null)
			{
				m_NextPosition = m_PlayerCharacter.m_CPAComponent.CalculateCameraPosition(transform.position);
				m_NextRotation = m_PlayerCharacter.m_CPAComponent.CalculateCameraLotation(transform.position);
			}
			else if(m_PlayerCharacter.m_CPAComponent == null)
			{
				m_NextPosition = m_PlayerCharacter.transform.position + (transform.up * distance * Mathf.Sin(angle * Mathf.Deg2Rad)) + (m_PlayerCharacter.m_VerticalMoveDirection * -distance * Mathf.Cos(angle * Mathf.Deg2Rad));
			}
		}

		//NativeSpline t_SplinePath = new NativeSpline(new SplinePath<Spline>(m_SplineContainer.Splines), m_SplineContainer.transform.localToWorldMatrix);
		//SplineUtility.GetNearestPoint(t_SplinePath, m_PlayerCharacter.transform.position, out float3 t_Point, out float t_t);
		//m_NextPosition = t_Point;

		//if(Input.GetMouseButtonDown(0) == true)
		//{
		//	MouseTargeting();
		//}
		//CameraMovement(DeltaTime);


		//if (m_PlayerCharacter != null)
		//{
		//	if(m_PlayerCharacter.m_CPAComponent != null)
		//	{
		//		m_NextPosition = m_PlayerCharacter.m_CPAComponent.CalculateCameraPosition();
		//	}
		//
		//	m_NextPosition = m_PlayerCharacter.transform.position + new Vector3(0.0f, distance * Mathf.Sin(angle * Mathf.Deg2Rad), -distance * Mathf.Cos(angle * Mathf.Deg2Rad));
		//}
	}

	private void FixedUpdate()
	{
		float DeltaTime = Time.fixedDeltaTime;
		transform.position = Vector3.Lerp(transform.position, m_NextPosition, moveSpeed * DeltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, m_NextRotation, moveSpeed * DeltaTime);
	}

	//GameObject MouseTargeting()
	//{
	//	if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == true)
	//	{
	//		return null;
	//	}
	//
	//	Vector3 mousePosition = Input.mousePosition;
	//	mousePosition.z = Camera.main.farClipPlane;
	//	mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
	//
	//	bool bTemp = false;
	//	RaycastHit hit;
	//	if (Physics.Raycast(transform.position, mousePosition, out hit, Mathf.Infinity) == true)
	//	{
	//		if (hit.transform.gameObject.GetComponent<CharacterBase>() != null)
	//		{
	//			bTemp = true;
	//		}
	//	}
	//
	//	if (bTemp == true)
	//	{
	//		SetCameraTarget(hit.transform.gameObject);
	//		return hit.transform.gameObject;
	//	}
	//	else
	//	{
	//		SetCameraTarget(null);
	//		return null;
	//	}
	//}

	void CameraMovement(float DeltaTime)
	{
		//float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
		//if (mouseScroll != 0.0f)
		//{
		//	distance = distance - (mouseScroll * DeltaTime * zoomSpeed);
		//
		//	if (distance < 0.0f)
		//	{
		//		mouseScroll = mouseScroll + (distance / (DeltaTime * zoomSpeed));
		//		distance = 0.0f;
		//	}
		//
		//	float radAngle = angle * Mathf.Deg2Rad;
		//	cameraPosition = cameraPosition + (new Vector3(0.0f, -mouseScroll * Mathf.Sin(radAngle), mouseScroll * Mathf.Cos(radAngle)) * DeltaTime * zoomSpeed);
		//}

		//if(cameraTarget == null)
		//{
		//	float horizontalMove = Input.GetAxis("Horizontal");
		//	float verticalMove = Input.GetAxis("Vertical");
		//
		//	if (horizontalMove != 0.0f || verticalMove != 0.0f)
		//	{
		//		cameraPosition = cameraPosition + (new Vector3(horizontalMove, 0.0f, verticalMove) * DeltaTime * moveSpeed);
		//	}
		//
		//	float mouseMoveX = Input.GetAxis("Mouse X");
		//	float mouseMoveY = Input.GetAxis("Mouse Y");
		//
		//	if (Input.GetMouseButton(0) == true)
		//	{
		//		cameraPosition = cameraPosition + (new Vector3(-mouseMoveX, 0.0f, -mouseMoveY) * DeltaTime * mouseMoveSpeed);
		//	}
		//}
		//else
		if(m_PlayerCharacter != null)
		{
			m_NextPosition = m_PlayerCharacter.transform.position + new Vector3(0.0f, distance * Mathf.Sin(angle * Mathf.Deg2Rad), -distance * Mathf.Cos(angle * Mathf.Deg2Rad));
		}
	}

	void SetCameraPosition(GameObject target)
	{
		cameraTarget = target;
		transform.rotation = Quaternion.Euler(angle, 0.0f, 0.0f);
		float radAngle = angle * Mathf.Deg2Rad;
		if(target != null)
		{
			m_NextPosition = cameraTarget.transform.position + new Vector3(0.0f, distance * Mathf.Sin(radAngle), -distance * Mathf.Cos(radAngle));
		}
	}
}
