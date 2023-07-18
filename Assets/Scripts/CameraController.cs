using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float MoveSpeed = 10.0f;
	[SerializeField] private float MouseMoveSpeed = 100.0f;
	[SerializeField] private float Distance = 5.0f;
	[SerializeField] private float Angle = 15.0f;
	[SerializeField] private float ZoomSpeed = 1000.0f;

	//public GameObject mousePointingTarget;
	[HideInInspector] public GameObject CameraTarget;
	private Vector3 CameraPosition;

	// Start is called before the first frame update
	void Start()
	{
		PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();
		if (playerCharacter != null)
		{
			SetCameraTarget(playerCharacter.gameObject);
		}

		CameraPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if(Input.GetMouseButtonDown(0) == true)
		{
			MouseTargeting();
		}
		CameraMovement(DeltaTime);

	}

	private void FixedUpdate()
	{
		float DeltaTime = Time.fixedDeltaTime;
		transform.position = Vector3.Lerp(transform.position, CameraPosition, MoveSpeed * DeltaTime);
	}

	GameObject MouseTargeting()
	{
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == true)
		{
			return null;
		}

		Vector3 MousePosition = Input.mousePosition;
		MousePosition.z = Camera.main.farClipPlane;
		MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);

		RaycastHit hit;
		if (Physics.Raycast(transform.position, MousePosition, out hit, Mathf.Infinity) == true)
		{
			SetCameraTarget(hit.transform.gameObject);
			return hit.transform.gameObject;
		}
		else
		{
			SetCameraTarget(null);
			return null;
		}
	}

	void CameraMovement(float DeltaTime)
	{
		float MouseScroll = Input.GetAxis("Mouse ScrollWheel");
		if (MouseScroll != 0.0f)
		{
			Distance = Distance - (MouseScroll * DeltaTime * ZoomSpeed);

			if (Distance < 0.0f)
			{
				MouseScroll = MouseScroll + (Distance / (DeltaTime * ZoomSpeed));
				Distance = 0.0f;
			}

			float RadAngle = Angle * Mathf.Deg2Rad;
			CameraPosition = CameraPosition + (new Vector3(0.0f, -MouseScroll * Mathf.Sin(RadAngle), MouseScroll * Mathf.Cos(RadAngle)) * DeltaTime * ZoomSpeed);
		}

		if(CameraTarget == null)
		{
			float HorizontalMove = Input.GetAxis("Horizontal");
			float VerticalMove = Input.GetAxis("Vertical");

			if (HorizontalMove != 0.0f || VerticalMove != 0.0f)
			{
				CameraPosition = CameraPosition + (new Vector3(HorizontalMove, 0.0f, VerticalMove) * DeltaTime * MoveSpeed);
			}

			float MouseMoveX = Input.GetAxis("Mouse X");
			float MouseMoveY = Input.GetAxis("Mouse Y");

			if (Input.GetMouseButton(0) == true)
			{
				CameraPosition = CameraPosition + (new Vector3(-MouseMoveX, 0.0f, -MouseMoveY) * DeltaTime * MouseMoveSpeed);
			}
		}
		else if(CameraTarget != null)
		{
			CameraPosition = CameraTarget.transform.position + new Vector3(0.0f, Distance * Mathf.Sin(Angle * Mathf.Deg2Rad), -Distance * Mathf.Cos(Angle * Mathf.Deg2Rad));
		}
	}

	void SetCameraTarget(GameObject Target)
	{
		CameraTarget = Target;
		transform.rotation = Quaternion.Euler(Angle, 0.0f, 0.0f);
		float RadAngle = Angle * Mathf.Deg2Rad;
		if(Target != null)
		{
			CameraPosition = CameraTarget.transform.position + new Vector3(0.0f, Distance * Mathf.Sin(RadAngle), -Distance * Mathf.Cos(RadAngle));
		}
	}
}
