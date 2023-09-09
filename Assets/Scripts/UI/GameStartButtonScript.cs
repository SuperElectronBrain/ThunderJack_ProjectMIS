using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButtonScript : ButtonScript
{
	[SerializeField] private string destinationSceneName;

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
		if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(destinationSceneName) != null)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(destinationSceneName);
		}
	}
}
