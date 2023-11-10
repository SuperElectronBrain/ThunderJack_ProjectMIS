using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public Inventory m_PlayerInventory = new Inventory();

    // Start is called before the first frame update
    void Start()
    {
		gameObject.name = "DataSaver";
        DontDestroyOnLoad(gameObject);

		List<DataSaver> t_DataSavers = FindObjectsOfType<DataSaver>().ToList();
        if(t_DataSavers != null)
        {
            for(int i = 0; i < t_DataSavers.Count; i = i + 1)
            {
                if (t_DataSavers[i] != this)
                {
                    Destroy(t_DataSavers[i].gameObject);
                }
            }
        }

		//UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
	}

	//void OnSceneUnloaded(UnityEngine.SceneManagement.Scene p_Scene)
	//{
    //    PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
    //    if(t_PlayerCharacter != null) 
    //    {
    //        if(t_PlayerCharacter.m_Inventory != null)
    //        {
	//			m_PlayerInventory = Instantiate(t_PlayerCharacter.m_Inventory);
	//		}
	//	}
	//}
	void OnSceneLoaded(UnityEngine.SceneManagement.Scene p_Scene, UnityEngine.SceneManagement.LoadSceneMode p_Mode)
    {
		PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			if(m_PlayerInventory != null)
			{
				Inventory t_Inventory = t_PlayerCharacter.GetComponent<Inventory>();
				if (t_Inventory != null)
				{
					t_Inventory.TakeData(m_PlayerInventory);
					Destroy(m_PlayerInventory);
				}
			}
		}
	}

	private void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene p_Scene0, UnityEngine.SceneManagement.Scene p_Scene1)
	{
		//PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		//if (t_PlayerCharacter != null)
		//{
		//	if (m_PlayerInventory != null)
		//	{
		//		Inventory t_Inventory = t_PlayerCharacter.GetComponent<Inventory>();
		//		if (t_Inventory != null)
		//		{
		//			t_Inventory.TakeInventoryItems(m_PlayerInventory);
		//			Destroy(m_PlayerInventory);
		//		}
		//	}
		//}
	}
}
