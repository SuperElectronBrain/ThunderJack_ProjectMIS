using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopComponent : MonoBehaviour
{
	[SerializeField] private GameObject ShopPanelPrefab;
	private GameObject ShopPanel;
	[SerializeField] private string m_SceneName;

	// Start is called before the first frame update
	void Start()
	{
		//Canvas canvas = FindObjectOfType<Canvas>();
		//if (canvas != null)
		//{
		//	if (ShopPanelPrefab != null)
		//	{
		//		ShopPanel = Instantiate(ShopPanelPrefab, canvas.transform);
		//		ShopPanel.SetActive(false);
		//
		//		GameObject t_GameObject = UniFunc.GetChildOfName(ShopPanel, "StartButton");
		//		if(t_GameObject != null)
		//		{
		//			Button t_Button = t_GameObject.GetComponent<Button>();
		//			if (t_Button != null)
		//			{
		//				t_Button.onClick.AddListener(() => 
		//				{
		//					UnityEngine.SceneManagement.SceneManager.LoadScene(m_SceneName);
		//				});
		//			}
		//		}
		//	}
		//}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			if(ShopPanel.activeSelf == false)
			{
				ShopPanel.SetActive(true);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent<PlayerCharacter>() != null)
		{
			if(ShopPanel.activeSelf == true)
			{
				ShopPanel.SetActive(false);
			}
		}
	}
}
