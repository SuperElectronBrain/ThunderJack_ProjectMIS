using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInventoryItemGenerator : MonoBehaviour
{
	[SerializeField] private int itemCode = 0;
	[SerializeField] private int itemAmount = 0;

	public void AddItem()
	{
		Inventory.main.AddAItem(itemCode, 1, itemAmount);
	}
}
