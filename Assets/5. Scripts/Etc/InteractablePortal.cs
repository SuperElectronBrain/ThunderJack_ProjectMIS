using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractablePortal : Portal, IInteraction
{
	public bool IsUsed { get; set; }

	public void Interaction(GameObject user)
	{
		LoadingScene();
		//if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(destinationSceneName) != null)
		//{
		//	//m_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		//	if (user != null)
		//	{
		//		DontDestroyOnLoad(user.gameObject);
		//		DontDestroyOnLoad(gameObject);
		//		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		//	}
		//
		//	UnityEngine.SceneManagement.SceneManager.LoadScene(destinationSceneName);
		//}
		//if (user != null)
		//{
		//	user.gameObject.transform.position = destination;
		//}
	}
}
