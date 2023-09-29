using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CustomButton : Button
{
	[FormerlySerializedAs("onDown")]
	[SerializeField]
	private ButtonClickedEvent m_OnDown = new ButtonClickedEvent();
	public ButtonClickedEvent onDown
	{
		get { return m_OnDown; }
		set { m_OnDown = value; }
	}

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

			if (IsActive() && IsInteractable())
			{ 
				UISystemProfilerApi.AddMarker("Button.onDown", this);
				onDown.Invoke();
			}
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
