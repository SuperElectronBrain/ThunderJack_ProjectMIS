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
	[HideInInspector] public Vector3 m_CharacterInputVector = Vector3.zero;
	protected Vector3 m_GroundNormalVector = Vector3.up;
	protected int m_CollisionCount = 0;
	protected bool m_UseScaleFlip = true;
	protected bool bMovable = true;
	protected Transform m_Destination = null;
	protected bool isGround = false;
	protected bool isWall = false;

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

		if (m_Destination != null)
		{
			Vector3 t_Vector = (m_Destination.position - transform.position).normalized;
			m_HorizontalMove = t_Vector.x;
			m_VerticalMove = t_Vector.z;
		}

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

	public void SetDestination(Transform p_Transform)
	{
		m_Destination = p_Transform;
		if (p_Transform == null)
		{
			m_HorizontalMove = 0;
			m_VerticalMove = 0;
		}
	}
	
	public void Teleport(Vector3 location)
	{
		transform.position = location;
	}

	#region TranslateMove
	protected virtual void HorizontalMove(float DeltaTime)
	{
		transform.Translate(m_HorizontalMoveDirection * new Vector2(m_HorizontalMove, m_VerticalMove).normalized.x * DeltaTime * m_Speed);
	}
	protected virtual void VerticalMove(float DeltaTime)
	{
		transform.Translate(m_VerticalMoveDirection * new Vector2(m_HorizontalMove, m_VerticalMove).normalized.y * DeltaTime * m_Speed);
	}
	#endregion

	#region SetMoveDirection
	public void SetMoveDirection(Vector3 p_HorizontalMoveDirection, Vector3 p_VerticalMoveDirection)
	{
		m_HorizontalMoveDirection = p_HorizontalMoveDirection;
		m_VerticalMoveDirection = p_VerticalMoveDirection;
	}
	public void SetMoveDirection(Vector3 p_HorizontalMoveDirection) { SetMoveDirection(p_HorizontalMoveDirection, Vector3.forward); }
	public void SetMoveDirection() { SetMoveDirection(Vector3.right, Vector3.forward); }
	#endregion

	#region Ground and Forward Check
	protected virtual void GroundCheck()
	{
		m_GroundNormalVector = Vector3.up;
		isGround = false;

		Vector3 startPosition = transform.position;
		Vector3 direction = new Vector3(0, -1, 0);
		bool result = Physics.Raycast(startPosition, direction, out RaycastHit raycastHit, 1.25f);
		if (result == true)
		{
			if (raycastHit.collider.isTrigger == false)
			{
				m_GroundNormalVector = raycastHit.normal;
				isGround = true;
			}
		}
	}
	protected virtual void ForwardCheck()
	{
		isWall = false;
		
		Vector3 startPosition = transform.position;
		startPosition.y = startPosition.y - 0.5f;
		Vector3 direction = m_CharacterInputVector;
		bool result = Physics.Raycast(startPosition, direction, out RaycastHit raycastHit, 0.55f);
		
		if (result == true)
		{
			if (raycastHit.collider.isTrigger == false)
			{
				if(Vector3.Dot(direction.normalized, -raycastHit.normal) > 0.9)
				{
					isWall = true;
				}
			}
		}
	}
	#endregion

	#region Collision
	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject != gameObject)
		{
			m_CollisionCount = m_CollisionCount + 1;

			if (collision.contacts != null)
			{
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
	#endregion
}
