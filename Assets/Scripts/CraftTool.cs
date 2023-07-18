using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CraftTool : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != this.gameObject)
		{
			if(other.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				Inventory inventory = other.gameObject.GetComponent<Inventory>();
				if (inventory != null)
				{
					inventory.DisplayItems(true);
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.GetComponent<PlayerCharacter>() != null)
			{
				Inventory inventory = other.gameObject.GetComponent<Inventory>();
				if (inventory != null)
				{
					inventory.DisplayItems(false);
				}
			}
		}
	}
}
