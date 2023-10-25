using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITradeable
{
	public bool TryTrade(AdvencedItem param);
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
		Debug.Log("인터렉션0");
		PlayerCharacter t_PlayerCharacter = user.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter == null) { return; }
		Debug.Log("인터렉션1");
		m_PlayerCharacter = t_PlayerCharacter;
		isUsed = true;
		Debug.Log("인터렉션2");
		EventManager.Publish(EventType.StartInteraction);
		Debug.Log("인터렉션 시작");
		m_PlayerCharacter.ChangeState(PlayerCharacterState.Communication);
		Debug.Log("인벤토리 팝업");
		m_PlayerCharacter.PopUpInventory(true);
	}

	public bool TryTrade(AdvencedItem param)
	{
		if(m_TradeableItems == null) { return false; }

		for (int i = 0; i < m_TradeableItems.Count; i = i + 1) 
		{
			if(param.itemCode == m_TradeableItems[i].requestItemCode)
			{
				m_PlayerCharacter.m_Inventory.PopAItem(m_TradeableItems[i].requestItemCode, 1, 1);
				m_PlayerCharacter.m_Inventory.AddAItem(m_TradeableItems[i].rewardItemCode, 1, 1);
				return true;
			}
		}

		return false;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(isUsed == true)
		{
			if(Input.GetKeyDown(KeyCode.Escape) == true)
			{
				isUsed = false;
				EventManager.Publish(EventType.EndIteraction);
				m_PlayerCharacter.ChangeState(PlayerCharacterState.Moveable);
				m_PlayerCharacter.PopUpInventory(false);
			}
			if(Input.GetMouseButtonDown(0) == true)
			{
				if (m_PlayerCharacter.m_GrabItemCode != null)
				{
					TryTrade(m_PlayerCharacter.m_GrabItemCode);
				}
			}
		}
    }
}
