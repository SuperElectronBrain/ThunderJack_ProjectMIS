using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst.CompilerServices;

public class MeasurCup : MonoBehaviour, IGrabable
{
	public int m_Input = 0;
	public float m_Progress = 0.0f;
	public float M_Progress { get { return m_Progress; } set { m_Progress = value > 1.0f ? 1.0f : (value < 0 ? 0 : value); } }
	public float m_MaxInputPerSecond = 0.1f;
	[HideInInspector] public bool m_GrabState = false; public void SetGrabState(bool p_State) { m_GrabState = p_State; }
	[HideInInspector] public bool m_IsGrabable = false; public bool IsGrabable() { return m_IsGrabable; }
	private Vector3 m_OriginPosition;
	private Quaternion m_OriginRotation;

	private Press m_Press;
	private PressInputPort m_PressInputPort;
	[SerializeField] private MixingBowl m_MixingBowl;
	[SerializeField] private TMPro.TextMeshPro m_ProgressText;
	[SerializeField] private SkeletonAnimation m_SkeletonAnimation;
	private TrackEntry trackEntry;

	// Start is called before the first frame update
	void Start()
    {
		m_OriginPosition = transform.position;
		if(m_SkeletonAnimation != null)
		{
			trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "animation", false);
		}
	}

    // Update is called once per frame
    void Update()
    {
		float DeltaTime = Time.deltaTime;

		if (Input.GetMouseButtonUp(0) == true)
		{
			transform.position = m_OriginPosition;
			if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
			if (trackEntry != null) { trackEntry.TrackTime = 0.0f; }
			m_GrabState = false;
		}

		//현재 플레이어가 this를 마우스로 집고 있는 상태라면
		if (m_GrabState == true)
		{
			//this를 마우스의 월드 좌표값을 따라 이동시키는 로직
			GrabMoving();

			if(m_Press != null)
			{
				Vector3 t_PressInput = m_PressInputPort.transform.position;
				t_PressInput.z = 0.0f;
				Vector3 t_Cup = m_PressInputPort.transform.position;
				t_Cup.z = 0.0f;

				if (m_Press.m_MaxDistance > (m_PressInputPort.transform.position - transform.position).magnitude)
				{
					//this의 애니메이션 진행도, 기울기값
					float t_Gradient = 1 - ((m_PressInputPort.transform.position - transform.position).magnitude / m_Press.m_MaxDistance);

					//this에 저장된 아이템 코드가 0이 아니라면
					if (m_Input != 0)
					{
						//this에 저장된 아이템의 함유량이 0 보다 크다면
						if (m_Progress > 0.0f)
						{
							float t_Temp = 0.0f;
							if (m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) < 0)
							{
								t_Temp = m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime);
							}

							M_Progress = M_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp;
							m_Press.AddIngredient(m_Input, (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp);
						}

						if (m_Progress <= 0.0f)
						{
							m_Input = 0;
							m_IsGrabable = false;
						}
					}

					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 1.0f; }
					if (trackEntry != null) { trackEntry.TrackTime = t_Gradient * trackEntry.AnimationEnd; }
				}
				else
				{
					if (trackEntry != null) { trackEntry.TrackTime = 0.0f; }
					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}
			}
			else if (m_Press == null)
			{
				if (trackEntry != null) { trackEntry.TrackTime = 0.0f; }
				if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
			}

			//this가 MixingBowl과 접촉하고 있는 상태라면
			if (m_MixingBowl != null)
			{
				//this와 MixingBowl 사이의 거리가 정해진 값보다 가깝다면
				if (m_MixingBowl.m_MaxDistance > (m_MixingBowl.transform.position - transform.position).magnitude)
				{
					//this의 애니메이션 진행도, 기울기값
					float t_Gradient = 1 - ((m_MixingBowl.transform.position - transform.position).magnitude / m_MixingBowl.m_MaxDistance);

					//this에 저장된 아이템 코드가 0이 아니라면
					if (m_Input != 0)
					{
						//this에 저장된 아이템의 함유량이 0 보다 크다면
						if (m_Progress > 0.0f)
						{
							float t_Temp = 0.0f;
							if (m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) < 0)
							{
								t_Temp = m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime);
							}

							M_Progress = M_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp;
							m_MixingBowl.AddIngredient(m_Input, (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp);
						}

						if (m_Progress <= 0.0f)
						{
							m_Input = 0;
							m_IsGrabable = false;
						}
					}

					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 1.0f; }
					if (trackEntry != null) { trackEntry.TrackTime = t_Gradient * trackEntry.AnimationEnd; }
				}
				else
				{
					if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
				}
			}
			else if (m_MixingBowl == null)
			{
				if (m_SkeletonAnimation != null) { m_SkeletonAnimation.timeScale = 0.0f; }
			}
		}

		if (m_Progress > 0.0f)
		{
			m_IsGrabable = true;
		}

		if (m_ProgressText != null)
		{
			m_ProgressText.text = m_Input + " " + (int)(m_Progress * 100.0f) + "";
		}
	}

	public void GrabMoving()
	{
		Vector3 t_Vector = Vector3.zero;
		if (Camera.main.orthographic == true) 
		{ 
			t_Vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			t_Vector.z = 0;
			transform.position = t_Vector;
		}
		else if (Camera.main.orthographic == false)
		{
			t_Vector = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
			float t_Value0 = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
			float t_VerticalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.y, t_Vector.z));
			float t_HorizontalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.x, t_Vector.z));
			float t_Value1 = Mathf.Sqrt(Mathf.Pow(Mathf.Tan(t_VerticalAngle) * t_Value0, 2) + Mathf.Pow(Mathf.Tan(t_HorizontalAngle) * t_Value0, 2));
			transform.position = (t_Vector * Mathf.Sqrt(Mathf.Pow(t_Value0, 2) + Mathf.Pow(t_Value1, 2))) + Camera.main.transform.position;
		}

		/*
		Vector3 t_MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
		transform.position = new Vector3(t_MousePosition.x, t_MousePosition.y, Camera.main.orthographic ? transform.position.z : t_MousePosition.z);
		*/
	}

	private void OnTriggerEnter(Collider other)
	{
		MixingBowl t_MixingBowl = other.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			m_MixingBowl = t_MixingBowl;
		}

		PressInputPort t_PressInputPort = other.GetComponent<PressInputPort>();
		if (t_PressInputPort != null)
		{
			m_PressInputPort = t_PressInputPort;
			Press t_Press = UniFunc.GetParentComponent<Press>(t_PressInputPort.gameObject);
			if (t_Press != null)
			{
				m_Press	= t_Press;
				//t_Press.bProgress = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		MixingBowl t_MixingBowl = collision.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			m_MixingBowl = t_MixingBowl;
		}
		PressInputPort t_PressInputPort = collision.GetComponent<PressInputPort>();
		if (t_PressInputPort != null)
		{
			m_PressInputPort = t_PressInputPort;
			Press t_Press = UniFunc.GetParentComponent<Press>(t_PressInputPort.gameObject);
			if (t_Press != null)
			{
				m_Press = t_Press;
				//t_Press.bProgress = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		MixingBowl t_MixingBowl = other.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			if(m_MixingBowl == t_MixingBowl)
			{
				m_MixingBowl = null;
			}
		}

		PressInputPort t_PressInputPort = other.GetComponent<PressInputPort>();
		if (t_PressInputPort != null)
		{
			if (m_Press == UniFunc.GetParentComponent<Press>(t_PressInputPort.gameObject))
			{
				m_Press = null;
				m_PressInputPort = null;
			}
		}
	}

	private void OnTriggerExit2D (Collider2D collision)
	{
		MixingBowl t_MixingBowl = collision.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			if (m_MixingBowl == t_MixingBowl)
			{
				m_MixingBowl = null;
			}
		}

		PressInputPort t_PressInputPort = collision.GetComponent<PressInputPort>();
		if (t_PressInputPort != null)
		{
			if (m_Press == UniFunc.GetParentComponent<Press>(t_PressInputPort.gameObject))
			{
				m_Press = null;
				m_PressInputPort = null;
			}
		}
	}
}
