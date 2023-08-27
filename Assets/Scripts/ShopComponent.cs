using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class ShopComponent : MonoBehaviour
{
	[SerializeField] private GameObject ShopPanelPrefab;
	private GameObject ShopPanel;
	[SerializeField] private string m_SceneName;

	// Start is called before the first frame update
	void Start()
	{
		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (ShopPanelPrefab != null)
			{
				ShopPanel = Instantiate(ShopPanelPrefab, canvas.transform);
				ShopPanel.SetActive(false);
			}
		}
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
