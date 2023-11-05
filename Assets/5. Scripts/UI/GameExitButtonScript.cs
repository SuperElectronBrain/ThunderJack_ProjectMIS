using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitButtonScript : ButtonScript
{
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	//protected override void Update()
	//{
	//	base.Update();
	//
	//}

	public override void OnButtonClick()
	{
		base.OnButtonClick();

#if UNITY_EDITOR
		Debug.Log("Exit");
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
