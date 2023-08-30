using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ClockUIScript : MonoBehaviour
{
	[SerializeField] private RectTransform m_ClockHand;
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
	[SerializeField] private float m_TimeSpeed = 1.0f;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;

		ProgressTime(DeltaTime);
		RefreshClock();
	}

	private void ProgressTime(float p_DeltaTime)
	{
		if(bProgressTime == true)
		{
			m_CurrentTime = m_CurrentTime + (p_DeltaTime / m_TimeSpeed);
			if (m_CurrentTime >= m_MaxTime)
			{
				m_CurrentTime = m_CurrentTime - m_MaxTime;
				CurrentDay = CurrentDay + 1;
				m_CurrentWeekday = m_CurrentWeekday + 1;
				if(m_CurrentWeekday >= m_MaxWeekday)
				{
					m_CurrentWeekday = 0;
				}
			}
		}
	}

	private void RefreshClock()
	{
		float t_Progress = (m_CurrentTime % m_MaxTime) / m_MaxTime;
		string t_Date = "Day " + m_CurrentDay + " / ";
		if (m_CurrentWeekday == 0) { t_Date = t_Date + "Mon"; }
		else if (m_CurrentWeekday == 1) { t_Date = t_Date + "Tue"; }
		else if (m_CurrentWeekday == 2) { t_Date = t_Date + "Wed"; }
		else if (m_CurrentWeekday == 3) { t_Date = t_Date + "Thu"; }
		else if (m_CurrentWeekday == 4) { t_Date = t_Date + "Fri"; }
		else if (m_CurrentWeekday == 5) { t_Date = t_Date + "Sat"; }
		else if (m_CurrentWeekday == 6) { t_Date = t_Date + "Sun"; }
		else { t_Date = "NUL"; }
		t_Date = t_Date + ".";

		if(m_ClockHand != null)
		{
			m_ClockHand.rotation = Quaternion.Euler(0.0f, 0.0f, -360.0f * t_Progress * 2);
		}
		if(m_MoonPhaseImage != null)
		{
			m_MoonPhaseImage.rotation = Quaternion.Euler(0.0f, 0.0f, -360.0f * t_Progress);
		}

		if (m_DateText != null)
		{
			m_DateText.text = t_Date;
		}
	}
}
