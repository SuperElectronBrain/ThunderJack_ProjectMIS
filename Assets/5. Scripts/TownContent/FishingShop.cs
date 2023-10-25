using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingShop : MonoBehaviour
{
	[Serializable] private struct TradeableItem { public int requestItemCode; public int rewardItemCode; }
	[SerializeField] private List<TradeableItem> m_TradeableItems = new List<TradeableItem>();

	private NPC m_NPC;

	private void Start()
	{
		m_NPC = GetComponent<NPC>();

		EventManager.Subscribe(EventType.StartInteraction, StartInteraction);
		EventManager.Subscribe(EventType.EndIteraction, EndInteraction);
	}
	private void StartInteraction()
	{
		if (m_NPC != null)
		{
			if(m_NPC.curInteractionObj != null)
			{
				PlayerCharacter t_PC = m_NPC.curInteractionObj.GetComponent<PlayerCharacter>();
				if(t_PC != null)
				{
					Inventory.main.PopUpInventory(true);
					Inventory.main.m_OnItemClick.AddListener(TryTrade);
				}
			}
		}
	}

	private void EndInteraction()
	{
		if (m_NPC != null)
		{
			if (m_NPC.curInteractionObj != null)
			{
				PlayerCharacter t_PC = m_NPC.curInteractionObj.GetComponent<PlayerCharacter>();
				if (t_PC != null)
				{
					Inventory.main.PopUpInventory(false);
					Inventory.main.m_OnItemClick.RemoveListener(TryTrade);
				}

			}
		}
	}

	public void TryTrade(PlayerCharacter p_PC, AdvencedItem p_Item)
	{
		if (m_TradeableItems == null) { return; }

		for (int i = 0; i < m_TradeableItems.Count; i = i + 1)
		{
			if (p_Item.itemCode == m_TradeableItems[i].requestItemCode)
			{
				p_PC.m_Inventory.PopAItem(m_TradeableItems[i].requestItemCode, 1, 1);
				p_PC.m_Inventory.AddAItem(m_TradeableItems[i].rewardItemCode, 1, 1);
				return;
			}
		}
	}
}
