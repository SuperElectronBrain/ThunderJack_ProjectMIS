using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerCharacter : Character
{
	private CameraController m_CameraCon;
	private CapsuleCollider m_Collider;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		m_CameraCon = Camera.main.gameObject.GetComponent<CameraController>();
		m_Collider = m_CameraCon.GetComponent<CapsuleCollider>();
	}

	// Update is called once per frame
	//protected override void Update()
	//{
	//	base.Update();
	//	float DeltaTime = Time.deltaTime;
	//}

	//protected override void FixedUpdate()
	//{
	//	base.FixedUpdate();
	//	float DeltaTime = Time.fixedDeltaTime;
	//}

	protected override void KeyInput()
	{
		m_HorizontalMove = Input.GetAxis("Horizontal");
		m_VerticalMove = Input.GetAxis("Vertical");
		if (Input.GetAxisRaw("Horizontal") == 0.0f) { m_HorizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { m_VerticalMove = 0.0f; }

		if (Input.GetKeyDown(KeyCode.Space) == true) { Jump(); }
	}

	protected override void Jump()
	{
		base.Jump();

		RaycastHit hit;
		Vector3 t_Point = m_Collider != null ? transform.up * ((m_Collider.height / 2) - m_Collider.radius) : transform.position;
		if (Physics.CapsuleCast(t_Point, -t_Point, m_Collider != null ? m_Collider.radius : transform.localScale.x / 2, transform.forward, out hit, Mathf.Infinity) == true)
		{
			if (hit.transform.gameObject != gameObject)
			{
				m_Rigidbody.AddForce(Vector3.up * jumpForce);
			}
		}
	}

	//protected override void HorizontalMove(float DeltaTime)
	//{
	//	base.HorizontalMove(DeltaTime);
	//}

	//protected override void VerticalMove(float DeltaTime)
	//{
	//	base.VerticalMove(DeltaTime);
	//}
}
