using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private Vector3 destination;
	[SerializeField] private string destinationSceneName;
	[SerializeField] private bool isOverlapping = false;
	private PlayerCharacter m_PlayerCharacter = null;

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
		PlayerCharacter t_PlayerCharacter = null;
		List<PlayerCharacter> t_PlayerCharacters = FindObjectsOfType<PlayerCharacter>().ToList();
		for(int i = 0; i < t_PlayerCharacters.Count; i = i + 1)
		{
			if(t_PlayerCharacters[i] != m_PlayerCharacter)
			{
				t_PlayerCharacter = t_PlayerCharacters[i];
				break;
			}
		}
		if (t_PlayerCharacter == null)
		{
			t_PlayerCharacter = Instantiate(m_PlayerCharacter.gameObject).GetComponent<PlayerCharacter>();
		}

		if (t_PlayerCharacter != null)
		{
			if (isOverlapping == true)
			{
				t_PlayerCharacter.gameObject.AddComponent<PortalMarker>();
			}
			t_PlayerCharacter.gameObject.transform.position = destination;
			if (m_PlayerCharacter != null)
			{
				if (t_PlayerCharacter.m_Inventory == null)
				{
					t_PlayerCharacter.m_Inventory = t_PlayerCharacter.GetComponent<Inventory>();
				}
				if (t_PlayerCharacter.m_Inventory != null)
				{
					if(t_PlayerCharacter.m_Inventory.GetAItems().Count <= 0)
					{
						t_PlayerCharacter.m_Inventory.TransferInventoryItems(m_PlayerCharacter.m_Inventory);
					}
				}
			}
			t_PlayerCharacter.FindPlayerCharacterUIScript();
		}

		if (m_PlayerCharacter != null) { Destroy(m_PlayerCharacter.gameObject); }
		m_PlayerCharacter = null;
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.isStatic == false)
			{
				if (other.gameObject.GetComponent<PlayerCharacter>() != null)
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
								m_PlayerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
								DontDestroyOnLoad(m_PlayerCharacter.gameObject);
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
