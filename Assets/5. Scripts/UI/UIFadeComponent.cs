using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIFadeComponent : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image m_TargetImage;
    [SerializeField] private TextMeshProUGUI m_TargetTMP;
    [SerializeField] private float m_FadeInSpeed = 0.0f;
	private float m_FadeInTime = 0.0f;
    [SerializeField] private float m_FadeOutSpeed = 0.0f;
	private float m_FadeOutTime = 0.0f;

	// Start is called before the first frame update
	void Start()
    {
        if(m_TargetImage == null)
        {
			m_TargetImage = GetComponent<UnityEngine.UI.Image>();
		}
        if(m_TargetTMP == null)
        {
			m_TargetTMP = GetComponent<TextMeshProUGUI>();
		}
    }

	// Update is called once per frame
	private void Update()
    {
		float DeltaTime = Time.deltaTime;

		if(m_FadeInTime > 0.0f)
		{
			m_FadeInTime = m_FadeInTime - DeltaTime;
			m_FadeInTime = m_FadeInTime < 0 ? 0 : m_FadeInTime;
			if (m_TargetImage != null)
			{
				Color t_Color = m_TargetImage.color;
				t_Color.a = 1 - (m_FadeInTime / m_FadeInSpeed);
				m_TargetImage.color = t_Color;
			}
			if (m_TargetTMP != null)
			{
				Color t_Color = m_TargetTMP.color;
				t_Color.a = 1 - (m_FadeInTime / m_FadeInSpeed);
				m_TargetTMP.color = t_Color;
			}
		}
		if(m_FadeOutTime > 0)
		{
			if(m_FadeOutTime > 0 && (m_FadeOutTime - DeltaTime) <= 0)
			{
				gameObject.SetActive(false);
			}
			m_FadeOutTime = m_FadeOutTime - DeltaTime;
			m_FadeOutTime = m_FadeOutTime < 0 ? 0 : m_FadeOutTime;
			if (m_TargetImage != null)
			{
				Color t_Color = m_TargetImage.color;
				t_Color.a = m_FadeOutTime / m_FadeOutSpeed;
				m_TargetImage.color = t_Color;
			}
			if (m_TargetTMP != null)
			{
				Color t_Color = m_TargetTMP.color;
				t_Color.a = m_FadeOutTime / m_FadeOutSpeed;
				m_TargetTMP.color = t_Color;
			}
		}
    }

	private void OnEnable()
	{
		if(m_FadeInSpeed > 0)
		{
			if(m_FadeOutTime <= 0)
			{
				m_FadeInTime = m_FadeInSpeed;
				if (m_TargetImage != null)
				{
					Color t_Color = m_TargetImage.color;
					t_Color.a = 0;
					m_TargetImage.color = t_Color;
				}
				if (m_TargetTMP != null)
				{
					Color t_Color = m_TargetTMP.color;
					t_Color.a = 0;
					m_TargetTMP.color = t_Color;
				}
			}
		}
	}

	private void OnDisable()
	{
		if(m_FadeOutSpeed > 0)
		{
			if(m_FadeOutTime <= 0)
			{
				m_FadeOutTime = m_FadeOutSpeed;
				if (m_TargetImage != null)
				{
					Color t_Color = m_TargetImage.color;
					t_Color.a = 1;
					m_TargetImage.color = t_Color;
				}
				if (m_TargetTMP != null)
				{
					Color t_Color = m_TargetTMP.color;
					t_Color.a = 1;
					m_TargetTMP.color = t_Color;
				}

				GameObject t_GO = new GameObject();
				SelfActivateHelperComponent t_SAH = t_GO.AddComponent<SelfActivateHelperComponent>();
				t_SAH.m_TargetGO = gameObject;
			}
		}
	}
}
