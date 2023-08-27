using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class PlayerCharacter : CharacterBase
{
	private CameraController m_CameraCon;
	private CapsuleCollider m_Collider;
	//[SerializeField] private SplineContainer m_SplineContainer;
	//public GameObject m_Temporary;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		m_CameraCon = Camera.main.gameObject.GetComponent<CameraController>();
		m_Collider = gameObject.GetComponent<CapsuleCollider>();

		//transform.position = SplineUtility.EvaluatePosition(m_SplineContainer.Spline, 1.0f);
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		float DeltaTime = Time.deltaTime;

		//NativeSpline t_SplinePath = new NativeSpline(new SplinePath<Spline>(m_SplineContainer.Splines), m_SplineContainer.transform.localToWorldMatrix);
		//SplineUtility.GetNearestPoint(t_SplinePath, transform.position, out float3 t_Point, out float t_t);
		//m_Temporary.transform.position = t_Point;
		//m_Temporary.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(m_SplineContainer.EvaluateTangent(t_SplinePath, t_t)), m_SplineContainer.EvaluateUpVector(t_SplinePath, t_t));
	}

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

	protected override void OnTriggerEnter(Collider collision)
	{
		m_CPAComponent = collision.gameObject.GetComponent<CameraPresetAreaComponent>();
		if (m_CPAComponent != null)
		{
			m_CPAComponent.m_PlayerCharacter = this;
		}
	}
}
