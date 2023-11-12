using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUiComponent : UiComponent
{
	private void OnEnable()
	{
		//ActiveUI();
	}

	private void OnDisable()
	{
		//InactiveUI();
	}

	public override void ActiveUI()
	{
		base.ActiveUI();
	}

	public override void InactiveUI()
	{
		base.InactiveUI();
	}
}
