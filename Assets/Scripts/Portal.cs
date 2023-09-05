using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private Vector3 destination;
	[SerializeField] private string destinationSceneName;
	[SerializeField] private bool isOverlapping = false;

	// Start is called before the first frame update
	void Start()
	{
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		if (gameObject.GetComponent<Renderer>() != null)
		{
			gameObject.GetComponent<Renderer>().enabled = false;
		}
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	private void OnSceneLoaded(UnityEngine.SceneManagement.Scene p_Scene, UnityEngine.SceneManagement.LoadSceneMode p_Mode)
	{
		PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			if (isOverlapping == true)
			{
				t_PlayerCharacter.gameObject.AddComponent<PortalMarker>();
			}
			t_PlayerCharacter.gameObject.transform.position = destination;
		}
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.isStatic == false)
			{
				if (other.gameObject.GetComponent<CharacterBase>() != null)
				{
					if(other.gameObject.GetComponent<PortalMarker>() == null)
					{
						if(isOverlapping == true)
						{
							other.gameObject.AddComponent<PortalMarker>();
						}

						if(destinationSceneName != null && destinationSceneName != "")
						{
							if(UnityEngine.SceneManagement.SceneManager.GetSceneByName(destinationSceneName) != null)
							{
								DontDestroyOnLoad(gameObject);
								UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
								UnityEngine.SceneManagement.SceneManager.LoadScene(destinationSceneName);
							}
						}
						if (destination != null)
						{
							other.gameObject.transform.position = destination;
						}
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.GetComponent<PortalMarker>() != null)
			{
				Destroy(other.gameObject.GetComponent<PortalMarker>());
			}
		}
	}
}
