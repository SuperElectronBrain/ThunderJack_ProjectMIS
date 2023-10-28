using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] protected Vector3 destination;
	public string destinationSceneName;
	[SerializeField] protected bool isOverlapping = false;
	protected PlayerCharacter m_PlayerCharacter = null;

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

	//protected void OnSceneChanged(UnityEngine.SceneManagement.Scene p_Scene0, UnityEngine.SceneManagement.Scene p_Scene1)
	//{
	//	PlayerCharacter t_PlayerCharacter = null;
	//	List<PlayerCharacter> t_PlayerCharacters = FindObjectsOfType<PlayerCharacter>().ToList();
	//	for (int i = 0; i < t_PlayerCharacters.Count; i = i + 1)
	//	{
	//		if (t_PlayerCharacters[i] != m_PlayerCharacter)
	//		{
	//			t_PlayerCharacter = t_PlayerCharacters[i];
	//			break;
	//		}
	//	}
	//	if (t_PlayerCharacter == null)
	//	{
	//		t_PlayerCharacter = Instantiate(m_PlayerCharacter.gameObject).GetComponent<PlayerCharacter>();
	//	}

	//	if (t_PlayerCharacter != null)
	//	{
	//		if (isOverlapping == true) { t_PlayerCharacter.gameObject.AddComponent<PortalMarker>(); }
	//		t_PlayerCharacter.gameObject.transform.position = destination;
	//		if (m_PlayerCharacter != null) { t_PlayerCharacter.TakeComponents(m_PlayerCharacter); }
	//		t_PlayerCharacter.FindPlayerCharacterUIScript();
	//		//CameraController t_CameraController = Camera.main.GetComponent<CameraController>();
	//		//if(t_CameraController != null) { t_CameraController.m_PlayerCharacter = t_PlayerCharacter; }
	//		if (t_PlayerCharacter.GetComponent<Rigidbody>() != null) { t_PlayerCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero; }

	//		Debug.Log(t_PlayerCharacter.m_Inventory.GetAItems().Count);
	//	}

	//	if (m_PlayerCharacter != null) { Destroy(m_PlayerCharacter.gameObject); }
	//	m_PlayerCharacter = null;
	//	UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
	//	Destroy(gameObject);
	//}

	protected void OnSceneLoaded(UnityEngine.SceneManagement.Scene p_Scene, UnityEngine.SceneManagement.LoadSceneMode p_Mode)
	{
		if (p_Scene.name != "loading")
		{
			PlayerCharacter t_PlayerCharacter = null;
			List<PlayerCharacter> t_PlayerCharacters = FindObjectsOfType<PlayerCharacter>().ToList();
			for (int i = 0; i < t_PlayerCharacters.Count; i = i + 1)
			{
				if (t_PlayerCharacters[i] != m_PlayerCharacter)
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
				if (isOverlapping == true) { t_PlayerCharacter.gameObject.AddComponent<PortalMarker>(); }
				t_PlayerCharacter.gameObject.transform.position = destination;
				if (m_PlayerCharacter != null) { t_PlayerCharacter.TakeComponents(m_PlayerCharacter); }
				t_PlayerCharacter.FindPlayerCharacterUIScript();
				if (t_PlayerCharacter.GetComponent<Rigidbody>() != null) { t_PlayerCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero; }
			}

			if (m_PlayerCharacter != null) { Destroy(m_PlayerCharacter.gameObject); }
			m_PlayerCharacter = null;
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.isStatic == false)
			{
				if (other.gameObject.GetComponent<PlayerCharacter>() != null)
				{
					if (other.gameObject.GetComponent<PortalMarker>() == null)
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

	public void SceneLoad()
    {
		Debug.Log("æ¿ ¿Ãµø");
		m_PlayerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
		DontDestroyOnLoad(m_PlayerCharacter.gameObject);
		DontDestroyOnLoad(gameObject);
        //Loading.LoadScene(destinationSceneName);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.LoadScene(destinationSceneName);
    }

	public void LoadingScene()
    {
		m_PlayerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
		DontDestroyOnLoad(m_PlayerCharacter.gameObject);
		DontDestroyOnLoad(gameObject);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		Loading.LoadScene(destinationSceneName);
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
