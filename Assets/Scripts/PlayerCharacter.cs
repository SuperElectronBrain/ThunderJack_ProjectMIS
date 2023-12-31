using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

//public interface IHitable
//{
//	void OnMouseHit(GameObject go);
//}

public class PlayerCharacter : CharacterBase
{
	private ItemCode m_GrabItem = 0;

	private CameraController m_CameraCon;
	private CapsuleCollider m_Collider;
	[HideInInspector] public GameObject m_HitObject;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();

		m_CameraCon = Camera.main.gameObject.GetComponent<CameraController>();
		m_Collider = gameObject.GetComponent<CapsuleCollider>();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		float DeltaTime = Time.deltaTime;

	}

	//protected override void FixedUpdate()
	//{
	//	base.FixedUpdate();
	//	float DeltaTime = Time.fixedDeltaTime;
	//}

	protected override void KeyInput()
	{
		if (Camera.main.orthographic == false)
		{
			m_HorizontalMove = Input.GetAxis("Horizontal");
			m_VerticalMove = Input.GetAxis("Vertical");
		}
		if (Input.GetAxisRaw("Horizontal") == 0.0f) { m_HorizontalMove = 0.0f; }
		if (Input.GetAxisRaw("Vertical") == 0.0f) { m_VerticalMove = 0.0f; }

		if (Input.GetKeyDown(KeyCode.Space) == true) { Jump(); }
		
		if (Input.GetMouseButtonDown(0) == true)
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
			{
				Vector3 t_MousePosition = Vector3.zero;
				if(Camera.main.orthographic == false)
				{
					t_MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
					if (Physics.Raycast(Camera.main.transform.position, t_MousePosition, out RaycastHit hit, Mathf.Infinity) == true)
					{ OnClickHit(hit); }
					else
					{ OnClickMiss(); }
				}
				else if(Camera.main.orthographic == true)
				{
					t_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					RaycastHit2D hit2D = Physics2D.Raycast(t_MousePosition, new Vector3(t_MousePosition.x, t_MousePosition.y, t_MousePosition.z + 100));
					if (hit2D == true)
					{ OnClickHit2D(hit2D); }
					else
					{ OnClickMiss2D(); }
				}
			}
		}
		if (Input.GetMouseButtonUp(0) == true)
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
			{
				Vector3 t_MousePosition = Vector3.zero;
				if (Camera.main.orthographic == false)
				{
					t_MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
					if (Physics.Raycast(Camera.main.transform.position, t_MousePosition, out RaycastHit hit, Mathf.Infinity) == true)
					{ OnReleaseHit(hit); }
					else
					{ OnReleaseMiss(); }
				}
				else if (Camera.main.orthographic == true)
				{
					t_MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					RaycastHit2D hit2D = Physics2D.Raycast(t_MousePosition, new Vector3(t_MousePosition.x, t_MousePosition.y, t_MousePosition.z + 100));
					if (hit2D == true)
					{ OnReleaseHit2D(hit2D); }
					else
					{ OnReleaseMiss2D(); }
				}
			}
		}
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
				if(m_Rigidbody != null)
				{
					m_Rigidbody.AddForce(Vector3.up * jumpForce);
				}
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

	//3DHit

	protected virtual void OnClickHit(RaycastHit hit)
	{
		m_HitObject = hit.transform.gameObject;

		MillStoneHandle t_MillStoneHandle = hit.transform.GetComponent<MillStoneHandle>();
		if (t_MillStoneHandle != null)
		{
			UniFunc.GetParentComponent<MillStone>(t_MillStoneHandle.gameObject).bProgress = true;
		}

		MeasurCup t_MeasurCup = hit.transform.GetComponent<MeasurCup>();
		if (t_MeasurCup != null)
		{
			t_MeasurCup.m_IsMouseGrab = true;
		}
	}
	protected virtual void OnClickMiss()
	{
		m_HitObject = null;
	}

	protected virtual void OnReleaseHit(RaycastHit hit)
	{
		MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
		if (t_MillStone != null)
		{
			if(t_MillStone.M_Input == 0)
			{
				t_MillStone.M_Input = m_GrabItem;
				t_MillStone.M_Progress = 1.0f;
			}
		}
	}

	protected virtual void OnReleaseMiss()
	{
		m_HitObject = null;
	}

	//2DHit

	protected virtual void OnClickHit2D(RaycastHit2D hit)
	{
		m_HitObject = hit.transform.gameObject;

		MillStoneHandle t_MillStoneHandle = hit.transform.GetComponent<MillStoneHandle>();
		if (t_MillStoneHandle != null)
		{
			MillStone t_MillStone = UniFunc.GetParentComponent<MillStone>(t_MillStoneHandle.gameObject);
			if (t_MillStone != null)
			{
				t_MillStone.bProgress = true;
			}
		}

		MeasurCup t_MeasurCup = hit.transform.GetComponent<MeasurCup>();
		if (t_MeasurCup != null)
		{
			//if (t_MeasurCup.m_Progress >= 1.0f)
			//{
				t_MeasurCup.m_IsMouseGrab = true;
			//}
		}
	}

	protected virtual void OnClickMiss2D()
	{
		m_HitObject = null;
	}

	protected virtual void OnReleaseHit2D(RaycastHit2D hit)
	{
		MillStone t_MillStone = hit.transform.GetComponent<MillStone>();
		if (t_MillStone != null)
		{
			if (t_MillStone.M_Input == 0)
			{
				t_MillStone.M_Input = m_GrabItem;
			}
		}
	}

	protected virtual void OnReleaseMiss2D()
	{
		m_HitObject = null;
	}
}
