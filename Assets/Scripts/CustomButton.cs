using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
	private void Press()
	{
		if (!IsActive() || !IsInteractable())
			return;

		UISystemProfilerApi.AddMarker("Button.onClick", this);
		onClick.Invoke();
	}

	public override void OnPointerClick(PointerEventData eventData) 
	{
		if(eventData.button != PointerEventData.InputButton.Middle) 
		{
			Press();
		}
	}

	private void EvaluateAndTransitionToSelectionState(SelectionState SelectionState)
	{
		if (!IsActive() || !IsInteractable())
			return;

		DoStateTransition(SelectionState, false);
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Middle)
		{
			if (IsInteractable() && navigation.mode != Navigation.Mode.None && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(gameObject, eventData);
			}

			EvaluateAndTransitionToSelectionState(SelectionState.Pressed);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Middle)
		{
			EvaluateAndTransitionToSelectionState(currentSelectionState);
		}
	}
}
