using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockUIScript : MonoBehaviour
{
	[SerializeField] private RectTransform m_ClockHand;
	[SerializeField] private RectTransform m_ClockMinuteHand;
	[SerializeField] private RectTransform m_MoonPhaseImage;
	[SerializeField] private TextMeshProUGUI m_DateText;

	[SerializeField] private int m_CurrentDay = 0; public int CurrentDay
	{
		get { return m_CurrentDay; }
		set
		{
			m_CurrentDay = value;
			if (m_CurrentDay < 0)
			{
				m_CurrentDay = 0;
			}
		}
	}
	[SerializeField] private int m_CurrentWeekday = 0; public int CurrentWeekday
	{
		get { return m_CurrentWeekday; }
		set
		{
			m_CurrentWeekday = value;
			if (m_CurrentWeekday >= m_MaxWeekday)
			{
				m_CurrentWeekday = m_MaxWeekday - 1;
			}
			else if (m_CurrentWeekday < 0)
			{
				m_CurrentWeekday = 0;
			}
		}
	}
	[SerializeField] [Range(0.0f, 24.0f)] private float m_CurrentTime; public float CurrentTime 
	{
		get { return m_CurrentTime; }
		set 
		{
			m_CurrentTime = value;
			if (m_CurrentTime >= m_MaxTime)
			{
				m_CurrentTime = m_MaxTime;
			}
			else if (m_CurrentTime < 0)
			{
				m_CurrentTime = 0.0f;
			}
		}
	}
	[SerializeField] private int m_MaxWeekday = 7;
	[SerializeField] private float m_MaxTime = 24.0f;
	[SerializeField] private bool bProgressTime = true;
	[SerializeField] private float m_TimeSpeed = 180;


	// Start is called before the first frame update
	void Start()
	{
		bProgressTime = false;
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.GameTime != null)
			{
				float t_CurrentTime = GameManager.Instance.GameTime.GetHour() + (GameManager.Instance.GameTime.GetMinute() / 60.0f);
				if (CurrentTime != t_CurrentTime)
				{
					CurrentTime = t_CurrentTime;

					if (t_CurrentTime == 0.0f)
					{
						CurrentDay = CurrentDay + 1;
						m_CurrentWeekday = m_CurrentWeekday + 1;
						if (m_CurrentWeekday >= m_MaxWeekday)
						{
							m_CurrentWeekday = 0;
						}
					}
				}

				//CurrentTime = CurrentTime + (DeltaTime / (360.0f / (60.0f / GameManager.Instance.GameTime.GetGameSpeed())));
				//if(CurrentTime >= m_MaxTime)
				//{
				//	CurrentTime = 0.0f;
				//}
			}
		}

		if (bProgressTime == true) { ProgressTime(DeltaTime); }
		RefreshClock();
	}

	private void ProgressTime(float p_DeltaTime)
	{
		m_CurrentTime = m_CurrentTime + (p_DeltaTime / m_TimeSpeed);
		if (m_CurrentTime >= m_MaxTime)
		{
			m_CurrentTime = m_CurrentTime - m_MaxTime;
			CurrentDay = CurrentDay + 1;
			m_CurrentWeekday = m_CurrentWeekday + 1;
			if (m_CurrentWeekday >= m_MaxWeekday)
			{
				m_CurrentWeekday = 0;
			}
		}
	}

	private void RefreshClock()
	{
		float t_Progress = (m_CurrentTime % m_MaxTime) / m_MaxTime;
		string t_Date = "NULL";
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.GameTime != null)
			{
				t_Date = GameManager.Instance.GameTime.GetDay() + "일차 " + GameManager.Instance.GameTime.GetHour() + ":" + GameManager.Instance.GameTime.GetMinute();
			}
		}
		///if (m_CurrentWeekday == 0) { t_Date = t_Date + "Mon"; }
		///else if (m_CurrentWeekday == 1) { t_Date = t_Date + "Tue"; }
		///else if (m_CurrentWeekday == 2) { t_Date = t_Date + "Wed"; }
		///else if (m_CurrentWeekday == 3) { t_Date = t_Date + "Thu"; }
		///else if (m_CurrentWeekday == 4) { t_Date = t_Date + "Fri"; }
		///else if (m_CurrentWeekday == 5) { t_Date = t_Date + "Sat"; }
		///else if (m_CurrentWeekday == 6) { t_Date = t_Date + "Sun"; }
		///else { t_Date = "NUL"; }
		//t_Date = t_Date + ".";

		if(m_ClockHand != null)
		{
			//12시간 단위로 2바퀴 돔
			m_ClockHand.rotation = Quaternion.Euler(0.0f, 0.0f, -360.0f * t_Progress * 2);
		}
		if(m_ClockMinuteHand != null)
		{
			//1시간 단위로 24바퀴 돌아야 함
			m_ClockMinuteHand.rotation = Quaternion.Euler(0.0f, 0.0f, -360.0f * t_Progress * m_MaxTime);
		}
		if(m_MoonPhaseImage != null)
		{
			//24시간 단위로 움직임
			m_MoonPhaseImage.rotation = Quaternion.Euler(0.0f, 0.0f, -360.0f * t_Progress);
		}

		if (m_DateText != null)
		{
			m_DateText.text = t_Date;
		}
	}
}
