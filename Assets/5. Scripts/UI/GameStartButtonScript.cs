using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStartButtonScript : ButtonScript
{
	[SerializeField] private Vector3 destination;
	[SerializeField] private string destinationSceneName;
	private PlayerCharacter m_PlayerCharacter;

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
				t_PlayerCharacter.gameObject.transform.position = destination;
				if (m_PlayerCharacter != null)
				{
					t_PlayerCharacter.TakeComponents(m_PlayerCharacter);
				}
				t_PlayerCharacter.FindPlayerCharacterUIScript();
				CameraController t_CameraController = Camera.main.GetComponent<CameraController>();
				if (t_CameraController != null) { t_CameraController.m_PlayerCharacter = t_PlayerCharacter; }
				if (t_PlayerCharacter.GetComponent<Rigidbody>() != null) { t_PlayerCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero; }
			}

			if (m_PlayerCharacter != null) { Destroy(m_PlayerCharacter.gameObject); }
			m_PlayerCharacter = null;
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
			if (gameObject != null)
			{
				Destroy(gameObject);
			}
		}
	}

	public override void OnButtonClick()
	{
		base.OnButtonClick();
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != destinationSceneName)
		{
			m_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
			if (m_PlayerCharacter != null)
			{
				DontDestroyOnLoad(m_PlayerCharacter.gameObject);
				DontDestroyOnLoad(gameObject);
				UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
			}

			UnityEngine.SceneManagement.SceneManager.LoadScene(destinationSceneName);
		}
		if (m_PlayerCharacter != null)
		{
			m_PlayerCharacter.gameObject.transform.position = destination;
		}
	}
}
