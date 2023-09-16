using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonScript : ButtonScript
{
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	public override void OnButtonClick()
    {
		PlayerCharacter t_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
		if(t_PlayerCharacter != null)
		{
			if (t_PlayerCharacter.m_Inventory != null)
			{
				if (t_PlayerCharacter.m_Inventory.m_InventoryUIScript != null)
				{
					t_PlayerCharacter.m_Inventory.m_InventoryUIScript.gameObject.SetActive(!t_PlayerCharacter.m_Inventory.m_InventoryUIScript.gameObject.activeSelf);
				}
			}
		}
    }
}
