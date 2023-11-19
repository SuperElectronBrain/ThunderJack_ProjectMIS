using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneProcessorComponent : MonoBehaviour
{
    [SerializeField] private Animator animator;
	[SerializeField] private string triggerName;
	[SerializeField] private KeyCode keyCode = KeyCode.E;
	[SerializeField] private int count = 0;

	private void Start()
	{
		if(animator == null)
		{ animator = GetComponent<Animator>(); }
	}
	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(keyCode) == true)
		{
			if(count > 0)
			{
				count = count - 1;
				if (animator != null)
				{
					animator.SetTrigger(triggerName);
				}
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}
}
