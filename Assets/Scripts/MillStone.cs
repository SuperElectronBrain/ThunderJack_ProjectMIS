using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStone : MonoBehaviour
{
	public ItemCode m_Input = ItemCode.None;
	public ItemCode M_Input { get { return m_Input; } set { m_Input = value; InputEvent(); } }
	public bool bProgress = false;
	public float m_Progress = 0.0f;
	public float M_Progress { get { return m_Progress; } set { m_Progress = value > 1.0f ? 1.0f : (value < 0 ? 0 : value);} }
	[SerializeField] private float m_MaxTurnCount = 10.0f;
	private Vector3 m_PreviousHandlePosition;

	public MeasurCup m_MeasurCup;
	[SerializeField] private TMPro.TextMeshPro m_ProgressText;

	// Start is called before the first frame update
	void Start()
	{
		m_PreviousHandlePosition = Vector3.left;
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
			if (m_Input != ItemCode.None)
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
						m_MeasurCup.m_Input = m_Input;
						m_MeasurCup.m_Progress = m_MeasurCup.m_Progress + (t_Progress / m_MaxTurnCount);
					}
				}
				else if(t_Progress < 0)
				{
					bProgress = false; //역회전 방지
				}

				transform.rotation = Quaternion.Euler(0.0f, 0.0f, (m_Progress * m_MaxTurnCount) * 360.0f);

				if(m_Progress <= 0.0f)
				{
					m_Input = ItemCode.None;
				}
				m_PreviousHandlePosition = t_CurrentHandlePosition;
			}
		}
		if(Input.GetMouseButtonUp(0) == true)
		{
			bProgress = false;
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
