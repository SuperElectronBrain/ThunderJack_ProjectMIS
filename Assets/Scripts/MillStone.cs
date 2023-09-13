using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MillStone : MonoBehaviour
{
	public string m_Input = "";
	public string M_Input { get { return m_Input; } set { m_Input = value; InputEvent(); } }
	public bool bProgress = false;
	public float m_Progress = 0.0f;
	public float M_Progress { get { return m_Progress; } set { m_Progress = value > 1.0f ? 1.0f : (value < 0 ? 0 : value);} }
	[SerializeField] private float m_MaxTurnCount = 10.0f;
	private Vector3 m_PreviousHandlePosition;

	public MeasurCup m_MeasurCup;
	[SerializeField] private TMPro.TextMeshPro m_ProgressText;
	[SerializeField] private SkeletonAnimation m_SkeletonAnimation;
	private TrackEntry trackEntry;

	// Start is called before the first frame update
	void Start()
	{
		m_PreviousHandlePosition = Vector3.left;
		trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "animation", false);
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if(Input.GetMouseButtonDown(0) == true)
		{
			m_PreviousHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
		}
		if (bProgress == true)
		{
			if (m_Input != "")
			{
				Vector3 t_CurrentHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

				float t_X = Vector3.Dot(m_PreviousHandlePosition, t_CurrentHandlePosition);//내적으로 m_PreviousHandlePosition를 축으로 하는 t_CurrentHandlePosition의 x축 성분을 계산함
				float t_Y = Vector3.Dot((t_CurrentHandlePosition - (m_PreviousHandlePosition * t_X)).normalized, t_CurrentHandlePosition);//t_CurrentHandlePosition에사 x축 성분을 제거하고 y축 성분을 얻음
				float t_Progress = (Vector3.Cross(m_PreviousHandlePosition, t_CurrentHandlePosition).z < 0 ? 1 : -1) * (Mathf.Atan2(t_Y, t_X) / Mathf.PI) / 2;
				if(t_Progress > 0)
				{
					M_Progress = M_Progress - (t_Progress / m_MaxTurnCount);

					if (m_MeasurCup != null)
					{
						if(m_MeasurCup.m_Input != m_Input)
						{
							m_MeasurCup.m_Progress = 0.0f;
						}
						m_MeasurCup.m_Input = m_Input;
						m_MeasurCup.m_Progress = m_MeasurCup.m_Progress + (t_Progress / m_MaxTurnCount);
					}

					m_SkeletonAnimation.timeScale = 1.0f;
				}
				else if(t_Progress < 0)
				{
					bProgress = false; //역회전 방지
					m_SkeletonAnimation.timeScale = 0.0f;
				}
				else if(t_Progress == 0)
				{
					m_SkeletonAnimation.timeScale = 0.0f;
				}

				float t_AnimationProgress = (1 - m_Progress) * m_MaxTurnCount;
				if(trackEntry != null)
				{
					trackEntry.TrackTime = (t_AnimationProgress - (int)t_AnimationProgress) * trackEntry.AnimationEnd;
				}

				if (m_Progress <= 0.0f)
				{
					m_Input = "";
					m_SkeletonAnimation.timeScale = 0.0f;
				}
				m_PreviousHandlePosition = t_CurrentHandlePosition;
			}
		}
		if(Input.GetMouseButtonUp(0) == true)
		{
			bProgress = false;
			m_SkeletonAnimation.timeScale = 0.0f;
		}

		if (m_ProgressText != null)
		{
			m_ProgressText.text = m_Input + " " +  (int)(m_Progress * 100.0f) + "";
		}
	}

	//void FixedUpdate()
	//{
	//	float DeltaTime = Time.deltaTime;	
	//}


	private void InputEvent()
	{

	}
}
