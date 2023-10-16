using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MillStone : MonoBehaviour
{
	private List<BBB> m_MaterialCount = new List<BBB>();
	public int m_Input = 0;
	public int M_Input { get { return m_Input; } set { m_Input = value; InputEvent(); } }
	public bool bProgress = false;
	public float m_Progress = 0.0f;
	public float M_Progress { get { return m_Progress; } set { m_Progress = value > 1.0f ? 1.0f : (value < 0 ? 0 : value); } }
	[SerializeField] private float m_MaxTurnCount = 10.0f;
	private float m_AnimationProgress = 0.0f;
	[SerializeField] private float m_AnimationProgressSpeed = 1.0f;
	private Vector3 m_PreviousHandlePosition;

	public MeasurCup m_MeasurCup;
	[SerializeField] private TMPro.TextMeshPro m_ProgressText;
	[SerializeField] private SkeletonAnimation m_SkeletonAnimation;
	private TrackEntry trackEntry;
	[SerializeField] private Animator m_TopAnimator;
	[SerializeField] private Animator m_BottomAnimator;
	[SerializeField] private GameObject m_Funnel;
	private Vector3 m_FunnelOriginPosition;

	// Start is called before the first frame update
	void Start()
	{
		m_PreviousHandlePosition = Vector3.left;
		trackEntry = m_SkeletonAnimation.state.SetAnimation(0, "animation", false);

		if (m_Funnel != null)
		{
			m_FunnelOriginPosition = m_Funnel.transform.position;
		}
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (Input.GetMouseButtonDown(0) == true)
		{
			m_PreviousHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
		}
		if (bProgress == true)
		{
			if (m_Input != 0)
			{
				Vector3 t_CurrentHandlePosition = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

				float t_X = Vector3.Dot(m_PreviousHandlePosition, t_CurrentHandlePosition);//내적으로 m_PreviousHandlePosition를 축으로 하는 t_CurrentHandlePosition의 x축 성분을 계산함
				float t_Y = Vector3.Dot((t_CurrentHandlePosition - (m_PreviousHandlePosition * t_X)).normalized, t_CurrentHandlePosition);//t_CurrentHandlePosition에사 x축 성분을 제거하고 y축 성분을 얻음
				float t_Progress = (Vector3.Cross(m_PreviousHandlePosition, t_CurrentHandlePosition).z < 0 ? 1 : -1) * (Mathf.Atan2(t_Y, t_X) / Mathf.PI) / 2;
				if (t_Progress > 0)
				{
					M_Progress = M_Progress - (t_Progress / m_MaxTurnCount);

					if (m_MeasurCup != null)
					{
						if (m_MeasurCup.m_Input != m_Input)
						{
							m_MeasurCup.m_Progress = 0.0f;
						}
						m_MeasurCup.m_Input = m_Input;
						m_MeasurCup.m_Progress = m_MeasurCup.m_Progress + (t_Progress / m_MaxTurnCount);
						if(m_MeasurCup.m_Progress >= 1.0f)
						{
							m_MeasurCup.m_Progress = 1.0f;
						}
					}

					if(m_Progress <= 0.0f)
					{
						if(m_MaterialCount != null)
						{
							if (m_MaterialCount.Count > 0)
							{
								Destroy(m_MaterialCount[0].transform.parent.gameObject);
								m_MaterialCount.Clear();
							}
						}
					}

					m_SkeletonAnimation.timeScale = 1.0f;
				}
				/*
				else if (t_Progress < 0)
				{
					bProgress = false; //역회전 방지
					m_SkeletonAnimation.timeScale = 0.0f;
				}
				*/
				else if (t_Progress == 0)
				{
					m_SkeletonAnimation.timeScale = 0.0f;
				}

				if (m_Progress <= 0.0f)
				{
					m_Input = 0;
					m_SkeletonAnimation.timeScale = 0.0f;
				}
				m_PreviousHandlePosition = t_CurrentHandlePosition;
			}
		}
		if (Input.GetMouseButtonUp(0) == true)
		{
			bProgress = false;
			m_SkeletonAnimation.timeScale = 0.0f;
		}

		if(m_Funnel != null)
		{
			m_Funnel.transform.position = m_FunnelOriginPosition + new Vector3(0.0f, (m_Progress - 1) / 2, 0.0f); 
		}

		if (m_Progress >= 1) { m_AnimationProgress = 1; }
		if (m_Progress <= 0) { m_AnimationProgress = 0; }
		float t_ProgressLerp = Mathf.Lerp(m_AnimationProgress, m_Progress, DeltaTime * m_AnimationProgressSpeed);
		m_TopAnimator.SetFloat("Velocity", m_AnimationProgress - t_ProgressLerp);
		m_BottomAnimator.SetFloat("Velocity", m_AnimationProgress - t_ProgressLerp);
		m_AnimationProgress = t_ProgressLerp;

		float t_AnimationProgress = (1 - m_Progress) * m_MaxTurnCount;
		if (trackEntry != null)
		{
			trackEntry.TrackTime = (t_AnimationProgress - (int)t_AnimationProgress) * trackEntry.AnimationEnd;
		}

		if (m_ProgressText != null)
		{
			m_ProgressText.text = m_Input + " " + (int)(m_Progress * 100.0f) + "";
		}
	}

	//void FixedUpdate()
	//{
	//	float DeltaTime = Time.deltaTime;	
	//}


	private void InputEvent()
	{

	}

	public bool SetItem(AdvencedItem p_AItem)
	{
		//if (m_Input == 0)
		//{
			//if (p_AItem.IsAddable(new AdvencedItem()) == false)
			//{
				switch (p_AItem.itemCode)
				{
					case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 11: case 12: case 13: case 14: case 15:
					{
						m_Input = p_AItem.itemCode;
						m_Progress = p_AItem.itemProgress;
						return true;
					}
					default:
					{
						break;
					}
				}
			//}
		//}

		return false;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject != gameObject)
		{
			BBB t_BBB = collision.gameObject.GetComponent<BBB>();
			if (t_BBB != null)
			{
				if (m_MaterialCount.Find((BBB b) => { return b == t_BBB; }) == null)
				{
					m_MaterialCount.Add(t_BBB);
				}

				if(m_MaterialCount.Count > 2)
				{
					if (t_BBB.gameObject.transform.parent != null)
					{
						AAA t_AAA = t_BBB.gameObject.transform.parent.GetComponent<AAA>();
						if(t_AAA != null)
						{
							SetItem(new AdvencedItem(t_AAA.m_ItemCode, 1, 1));
						}
					}
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject != gameObject)
		{
			BBB t_BBB = collision.gameObject.GetComponent<BBB>();
			if (t_BBB != null)
			{
				m_MaterialCount.Remove(t_BBB);
				m_MaterialCount.TrimExcess();
			}
		}
	}
}
