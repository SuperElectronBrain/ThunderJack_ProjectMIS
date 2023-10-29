using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIOnOffAnimationComponent : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private string m_OpenTriggerName = "Open";
	[SerializeField] private string m_CloseTriggerName = "Close";
	private bool isDisable = false;

	// Start is called before the first frame update
	void Start()
    {
		if (m_Animator == null) { m_Animator = GetComponent<Animator>(); }
		if (m_Animator == null) { m_Animator = gameObject.AddComponent<Animator>(); }
	}

    // Update is called once per frame
    void Update()
    {
        if(isDisable == true)
		{
			if (m_Animator != null)
			{
				if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
				{
					gameObject.SetActive(false);
				}
			}
		}
    }

	private void OnEnable()
	{
		if (isDisable == false)
		{
			if (m_Animator != null)
			{
				m_Animator.SetTrigger(m_OpenTriggerName);
			}
		}
		else if(isDisable == true)
		{
			if (m_Animator != null)
			{
				m_Animator.SetTrigger(m_CloseTriggerName);
			}
		}
	}

	private void OnDisable()
	{
		if(isDisable == false)
		{
			isDisable = true;

			GameObject t_GO = new GameObject();
			SelfActivateHelperComponent t_SAH = t_GO.AddComponent<SelfActivateHelperComponent>();
			t_SAH.m_TargetGO = gameObject;
		}
		else if(isDisable == true)
		{
			isDisable = false;
		}
	}
}
