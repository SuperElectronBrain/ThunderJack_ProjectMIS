using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITradeable
{
	public void TryTrade(PlayerCharacter p_PC, AdvencedItem p_Item);
}


public class FishCleaner : MonoBehaviour, IInteraction, ITradeable
{
	[Serializable] private struct TradeableItem { public int requestItemCode; public int rewardItemCode; }
	[SerializeField] private List<TradeableItem> m_TradeableItems = new List<TradeableItem>();
	public bool IsUsed { get { return isUsed; } set {; } }
	private bool isUsed = false;
	private PlayerCharacter m_PlayerCharacter;
	public void Interaction(GameObject user)
	{
		PlayerCharacter t_PlayerCharacter = user.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter == null) { return; };
		m_PlayerCharacter = t_PlayerCharacter;
		StartInteraction();
	}

	public void TryTrade(PlayerCharacter p_PC, AdvencedItem p_Item)
	{
		if(m_TradeableItems == null) { return; }

		for (int i = 0; i < m_TradeableItems.Count; i = i + 1) 
		{
			if(p_Item.itemCode == m_TradeableItems[i].requestItemCode)
			{
				//m_PlayerCharacter.SetPlayerGrabItem(null);
				m_PlayerCharacter.m_Inventory.PopAItem(m_TradeableItems[i].requestItemCode, 1, 1);
				m_PlayerCharacter.m_Inventory.AddAItem(m_TradeableItems[i].rewardItemCode, 1, 1);
				EndInteraction();
				return;
				//return true;
			}
		}

		//return false;
	}

	// Start is called before the first frame update
	private void StartInteraction()
    {
		isUsed = true;
		EventManager.Publish(EventType.StartInteraction);
		if(m_PlayerCharacter != null)
		{
			m_PlayerCharacter.ChangeState(PlayerCharacterState.Communication);
			Inventory.main.PopUpInventory(true);
		}
		Inventory.main.m_OnItemClick.AddListener(TryTrade);
	}

	private void EndInteraction()
	{
		isUsed = false;
		EventManager.Publish(EventType.EndIteraction);
		m_PlayerCharacter.ChangeState(PlayerCharacterState.Moveable);
		Inventory.main.PopUpInventory(false);
		Inventory.main.m_OnItemClick.RemoveListener(TryTrade);
	}

    // Update is called once per frame
    void Update()
    {
		if(isUsed == true)
		{
			if(Input.GetKeyDown(KeyCode.Escape) == true)
			{
				EndInteraction();
			}
		}
    }
}
