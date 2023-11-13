using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnSceneLoadedEventComponent : MonoBehaviour
{
    [SerializeField] private string m_SceneName = "";

	public UnityEvent m_OnSceneLoaded = new UnityEvent();

	// Start is called before the first frame update
	void Start()
    {
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(UnityEngine.SceneManagement.Scene p_Scene, UnityEngine.SceneManagement.LoadSceneMode p_Mode)
	{
        if(p_Scene.name == m_SceneName)
        {
            m_OnSceneLoaded.Invoke();
		}
    }
}
