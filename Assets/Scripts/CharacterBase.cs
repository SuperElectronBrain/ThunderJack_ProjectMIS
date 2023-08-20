using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	[SerializeField][Range(0, 100)] protected float speed = 5.0f;
	[SerializeField] protected float jumpForce = 250.0f;
	protected float m_HorizontalMove = 0.0f;
	protected float m_VerticalMove = 0.0f;
	private Vector3 m_PrevPosition;
	protected float m_Velocity;

	protected Camera m_MainCamera;
	protected Rigidbody m_Rigidbody;
    [SerializeField] protected GameObject m_SD;
    protected Animator m_Animator;

	// Start is called before the first frame update
	protected virtual void Start()
    {
		m_PrevPosition = transform.position;

		m_MainCamera = Camera.main;
		m_Rigidbody = gameObject.GetComponent<Rigidbody>();
		if (m_Rigidbody == null) { m_Rigidbody = gameObject.AddComponent<Rigidbody>(); }
		m_Animator = gameObject.GetComponent<Animator>();
	}

    // Update is called once per frame
    protected virtual void Update()
    {
		float DeltaTime = Time.deltaTime;

		KeyInput();

		if (m_SD != null)
        {
			Vector3 t_ModelingRotation = m_SD.transform.rotation.eulerAngles;
            if(m_MainCamera != null)
            {
			    t_ModelingRotation.y = m_MainCamera.transform.rotation.eulerAngles.y;
            }
			m_SD.transform.rotation = Quaternion.Euler(t_ModelingRotation);

			Vector3 t_ModelingScale = m_SD.transform.localScale;
			if (m_HorizontalMove > 0) { t_ModelingScale.x = -0.2f; }
			else if (m_HorizontalMove < 0) { t_ModelingScale.x = 0.2f; }
			m_SD.transform.localScale = t_ModelingScale;
		}

		if(m_Animator != null)
		{
			m_Animator.SetFloat("HorizontalSpeed", m_HorizontalMove < 0 ? -m_HorizontalMove : m_HorizontalMove);
			m_Animator.SetFloat("VerticalSpeed", m_VerticalMove);
		}
	}

	protected virtual void FixedUpdate()
	{
		float DeltaTime = Time.fixedDeltaTime;

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
		transform.Translate(new Vector3(m_HorizontalMove, 0.0f, 0.0f) * DeltaTime * speed);
	}

	protected virtual void VerticalMove(float DeltaTime)
	{
		transform.Translate(new Vector3(0.0f, 0.0f, m_VerticalMove) * DeltaTime * speed);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject != gameObject)
		{
			if (m_Animator != null)
			{
				m_Animator.SetTrigger("Landing");
			}
		}
	}
}
