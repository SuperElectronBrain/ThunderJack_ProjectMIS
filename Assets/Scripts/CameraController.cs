using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 10.0f;
	[SerializeField] private float mouseMoveSpeed = 100.0f;
	[SerializeField] private float distance = 5.0f;
	[SerializeField] private float angle = 15.0f;
	[SerializeField] private float zoomSpeed = 1000.0f;

	//public GameObject mousePointingTarget;
	[HideInInspector] public GameObject cameraTarget;
	private Vector3 cameraPosition;

	// Start is called before the first frame update
	void Start()
	{
		PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();
		if (playerCharacter != null)
		{
			SetCameraTarget(playerCharacter.gameObject);
		}

		cameraPosition = transform.position;
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
		transform.position = Vector3.Lerp(transform.position, cameraPosition, moveSpeed * DeltaTime);
	}

	GameObject MouseTargeting()
	{
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == true)
		{
			return null;
		}

		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = Camera.main.farClipPlane;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		bool bTemp = false;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, mousePosition, out hit, Mathf.Infinity) == true)
		{
			if(hit.transform.gameObject.GetComponent<Character>() != null)
			{
				bTemp = true;
			}
		}

		if (bTemp == true)
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
		float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
		if (mouseScroll != 0.0f)
		{
			distance = distance - (mouseScroll * DeltaTime * zoomSpeed);

			if (distance < 0.0f)
			{
				mouseScroll = mouseScroll + (distance / (DeltaTime * zoomSpeed));
				distance = 0.0f;
			}

			float radAngle = angle * Mathf.Deg2Rad;
			cameraPosition = cameraPosition + (new Vector3(0.0f, -mouseScroll * Mathf.Sin(radAngle), mouseScroll * Mathf.Cos(radAngle)) * DeltaTime * zoomSpeed);
		}

		if(cameraTarget == null)
		{
			float horizontalMove = Input.GetAxis("Horizontal");
			float verticalMove = Input.GetAxis("Vertical");

			if (horizontalMove != 0.0f || verticalMove != 0.0f)
			{
				cameraPosition = cameraPosition + (new Vector3(horizontalMove, 0.0f, verticalMove) * DeltaTime * moveSpeed);
			}

			float mouseMoveX = Input.GetAxis("Mouse X");
			float mouseMoveY = Input.GetAxis("Mouse Y");

			if (Input.GetMouseButton(0) == true)
			{
				cameraPosition = cameraPosition + (new Vector3(-mouseMoveX, 0.0f, -mouseMoveY) * DeltaTime * mouseMoveSpeed);
			}
		}
		else if(cameraTarget != null)
		{
			cameraPosition = cameraTarget.transform.position + new Vector3(0.0f, distance * Mathf.Sin(angle * Mathf.Deg2Rad), -distance * Mathf.Cos(angle * Mathf.Deg2Rad));
		}
	}

	void SetCameraTarget(GameObject target)
	{
		cameraTarget = target;
		transform.rotation = Quaternion.Euler(angle, 0.0f, 0.0f);
		float radAngle = angle * Mathf.Deg2Rad;
		if(target != null)
		{
			cameraPosition = cameraTarget.transform.position + new Vector3(0.0f, distance * Mathf.Sin(radAngle), -distance * Mathf.Cos(radAngle));
		}
	}
}
