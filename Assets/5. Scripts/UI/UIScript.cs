using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIScript : MonoBehaviour
{
	protected UiComponent uiComponent;

	protected virtual void Start()
	{
		uiComponent = GetComponent<UiComponent>();
	}

	protected virtual void OnEnable()
	{
		if (uiComponent == null) { uiComponent = GetComponent<UiComponent>(); }
		if (uiComponent != null) { uiComponent.ActiveUI(); }
		RefresfAction();
	}

	protected virtual void OnDisable()
	{

	}

	protected virtual void RefresfAction()
    {

    }
}
