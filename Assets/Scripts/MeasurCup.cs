using Spine.Unity;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurCup : MonoBehaviour
{
	public ItemCode m_Input = ItemCode.None;
	public float m_Progress = 0.0f;
	public float M_Progress { get { return m_Progress; } set { m_Progress = value > 1.0f ? 1.0f : (value < 0 ? 0 : value); } }
	public float m_MaxInputPerSecond = 0.1f;
	[HideInInspector] public bool m_IsMouseGrab = false;
	private Vector3 m_OriginPosition;
	private Quaternion m_OriginRotation;

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
			m_IsMouseGrab = false;
		}

		if (m_IsMouseGrab == true)
		{
			/*
			//Vector3 t_Vector = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
			//float t_Value0 = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
			//float t_VerticalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.y, t_Vector.z));
			//float t_HorizontalAngle = Mathf.Abs(Mathf.Atan2(t_Vector.x, t_Vector.z));
			//float t_Value1 = Mathf.Sqrt(Mathf.Pow(Mathf.Tan(t_VerticalAngle) * t_Value0, 2) + Mathf.Pow(Mathf.Tan(t_HorizontalAngle) * t_Value0, 2));
			//transform.position = (t_Vector * Mathf.Sqrt(Mathf.Pow(t_Value0, 2) + Mathf.Pow(t_Value1, 2))) + Camera.main.transform.position;
			*/

			Vector3 t_MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)); 
			transform.position = new Vector3(t_MousePosition.x, t_MousePosition.y, Camera.main.orthographic ? transform.position.z : t_MousePosition.z);

			if(m_MixingBowl != null)
			{
				if(m_MixingBowl.m_MaxDistance > (m_MixingBowl.transform.position - transform.position).magnitude)
				{
					float t_Gradient = 1 - ((m_MixingBowl.transform.position - transform.position).magnitude / m_MixingBowl.m_MaxDistance);

					if(m_Input != ItemCode.None)
					{
						if(m_Progress > 0.0f)
						{
							float t_Temp = 0.0f;
							if (m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) < 0)
							{
								t_Temp = m_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime);
							}

							M_Progress = M_Progress - (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp;
							m_MixingBowl.AddIngredient(m_Input, (m_MaxInputPerSecond * t_Gradient * DeltaTime) - t_Temp);
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

		if (m_ProgressText != null)
		{
			m_ProgressText.text = m_Input + " " + (int)(m_Progress * 100.0f) + "";
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		MixingBowl t_MixingBowl = other.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			m_MixingBowl = t_MixingBowl;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		MixingBowl t_MixingBowl = collision.GetComponent<MixingBowl>();
		if (t_MixingBowl != null)
		{
			m_MixingBowl = t_MixingBowl;
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
	}
}
