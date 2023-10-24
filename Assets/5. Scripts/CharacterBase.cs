using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterBase : MonoBehaviour
{
	[SerializeField][Range(0, 100)] protected float m_Speed = 5.0f;
	[SerializeField] protected float jumpForce = 250.0f;
	[HideInInspector] public Vector3 m_HorizontalMoveDirection;
	[HideInInspector] public Vector3 m_VerticalMoveDirection;
	protected float m_HorizontalMove = 0.0f;
	protected float m_VerticalMove = 0.0f;
	private Vector3 m_PrevPosition;
	protected float m_Velocity;
	protected Vector3 m_GroundNormalVector = Vector3.up;
	protected int m_CollisionCount = 0;
	protected bool m_UseScaleFlip = true;
	protected bool bMovable = true;
	protected Transform m_Destination = null;

	protected Camera m_MainCamera;
	protected Rigidbody m_Rigidbody;
    [SerializeField] protected GameObject m_SD;
    protected Animator m_Animator;
	[HideInInspector] public CameraPresetAreaComponent m_CPAComponent;
	[HideInInspector] public Inventory m_Inventory;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		m_HorizontalMoveDirection = Vector3.right;
		m_VerticalMoveDirection = Vector3.forward;
		m_PrevPosition = transform.position;
		m_UseScaleFlip = true;

		m_MainCamera = Camera.main;
		m_Rigidbody = gameObject.GetComponent<Rigidbody>();
		//if (m_Rigidbody == null) { m_Rigidbody = gameObject.AddComponent<Rigidbody>(); }
		m_Animator = gameObject.GetComponent<Animator>();
		m_Inventory = gameObject.GetComponent<Inventory>();
		if (m_SD != null)
		{
			m_SD.AddComponent<LookAtCameraComponent>();
		}
	}

    // Update is called once per frame
    protected virtual void Update()
    {
		float DeltaTime = Time.deltaTime;
		//if (m_Destination == null) { }
		//KeyInput();

		if (m_SD != null)
        {
			if(m_UseScaleFlip == true)
			{
				Vector3 t_ModelingScale = m_SD.transform.localScale;
				if (m_HorizontalMove > 0) { t_ModelingScale.x = -0.2f; }
				else if (m_HorizontalMove < 0) { t_ModelingScale.x = 0.2f; }
				m_SD.transform.localScale = t_ModelingScale;
			}
		}

		if(m_Animator != null)
		{
			m_Animator.SetFloat("HorizontalSpeed", m_UseScaleFlip ? (m_HorizontalMove < 0 ? -m_HorizontalMove : m_HorizontalMove) : m_HorizontalMove);
			m_Animator.SetFloat("VerticalSpeed", m_VerticalMove);
		}
	}

	protected virtual void FixedUpdate()
	{
		float DeltaTime = Time.fixedDeltaTime;

		/*
		if(m_CPAComponent != null) { SetMoveDirection(m_CPAComponent.transform.right, m_CPAComponent.transform.forward); }
		if(bMovable == true)
		{
			if(m_Destination != null)
			{
				Vector3 t_Vector = (m_Destination.position - transform.position).normalized;
				m_HorizontalMove = t_Vector.x;
				m_VerticalMove = t_Vector.z;
			}
		}
		*/

		HorizontalMove(DeltaTime);
		VerticalMove(DeltaTime);

		m_Velocity = (transform.position - m_PrevPosition).magnitude / DeltaTime;
		m_PrevPosition = transform.position;
	}

	protected virtual void KeyInput()
	{
	}

	protected virtual void Jump()
	{
		if (m_Animator != null)
		{
			m_Animator.SetTrigger("Jump");
		}
	}

	protected virtual void HorizontalMove(float DeltaTime)
	{
		//transform.Translate(new Vector3(m_HorizontalMove, 0.0f, 0.0f) * DeltaTime * speed);
		transform.Translate(m_HorizontalMoveDirection * m_HorizontalMove * DeltaTime * m_Speed);
	}

	protected virtual void VerticalMove(float DeltaTime)
	{
		//transform.Translate(new Vector3(0.0f, 0.0f, m_VerticalMove) * DeltaTime * speed);
		transform.Translate(m_VerticalMoveDirection * m_VerticalMove * DeltaTime * m_Speed);
	}

	public void SetMoveDirection(Vector3 p_HorizontalMoveDirection, Vector3 p_VerticalMoveDirection)
	{
		m_HorizontalMoveDirection = p_HorizontalMoveDirection;
		m_VerticalMoveDirection = p_VerticalMoveDirection;
	}

	public void SetMoveDirection(Vector3 p_HorizontalMoveDirection) { SetMoveDirection(p_HorizontalMoveDirection, Vector3.forward); }
	public void SetMoveDirection() { SetMoveDirection(Vector3.right, Vector3.forward); }

	public void SetDestination(Transform p_Transform)
	{
		m_Destination = p_Transform;
		if (p_Transform == null)
		{
			m_HorizontalMove = 0;
			m_VerticalMove = 0;
		}
	}

	//public virtual void CommunicationStart()
	//{
	//	bMovable = false;
	//}
	//
	//public virtual void CommunicationEnd()
	//{
	//	bMovable = true;
	//}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject != gameObject)
		{
			m_CollisionCount = m_CollisionCount + 1;

			if (collision.contacts != null)
			{
				//m_GroundNormalVector = collision.contacts[0].normal;
				//Vector3 t_HorizontalMoveDirection  = Vector3.Cross(collision.contacts[0].normal, m_VerticalMoveDirection).normalized;
				//Vector3 t_VerticalMoveDirection = Vector3.Cross(collision.contacts[0].normal, t_HorizontalMoveDirection).normalized;
				//Vector3.ProjectOnPlane(collision.contacts[0].normal, m_VerticalMoveDirection);
				//
				//SetMoveDirection(t_HorizontalMoveDirection, t_VerticalMoveDirection);
			}

			if (m_Animator != null)
			{
				m_Animator.SetTrigger("Landing");
			}
		}
	}
	protected virtual void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject != gameObject)
		{
			m_CollisionCount = m_CollisionCount - 1;

			//m_GroundNormalVector = Vector3.up;
			//if (collision.contacts != null)
			//{
			//	Vector3 t_HorizontalMoveDirection = Vector3.Cross(Vector3.up, m_VerticalMoveDirection).normalized;
			//	Vector3 t_VerticalMoveDirection = Vector3.Cross(Vector3.up, t_HorizontalMoveDirection).normalized;
			//
			//	SetMoveDirection(t_HorizontalMoveDirection, t_VerticalMoveDirection);
			//}
		}
	}

	protected virtual void OnTriggerEnter(Collider collision)
	{
		m_CPAComponent = collision.gameObject.GetComponent<CameraPresetAreaComponent>();
	}

	protected virtual void OnTriggerExit(Collider collision)
	{
		CameraPresetAreaComponent t_CPAComponent = collision.gameObject.GetComponent<CameraPresetAreaComponent>();
		if (t_CPAComponent != null)
		{
			if(t_CPAComponent == m_CPAComponent)
			{
				m_CPAComponent.m_PlayerCharacter = null;
				m_CPAComponent = null;
			}
		}
	}
}
