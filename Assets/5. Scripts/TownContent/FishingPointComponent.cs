using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPointComponent : MonoBehaviour, IInteraction
{
	[SerializeField] private float watingTimeMin = 0.0f;
	[SerializeField] private float watingTimeMax = 1.0f;
	[SerializeField] private float fishingTime = 3.0f;
	private float currentFishingTime = 0.0f;

	[SerializeField] private List<AdvencedItem> m_CatchableItemlist = new List<AdvencedItem>();
	private PlayerCharacter m_PlayerCharacter;
	public bool IsUsed { get; set; }

	public void Interaction(GameObject user)
	{
		PlayerCharacter t_PlayerCharacter = user.GetComponent<PlayerCharacter>();
		if(t_PlayerCharacter == null) { return; }

		m_PlayerCharacter = t_PlayerCharacter;
		m_PlayerCharacter.ChangeState(PlayerCharacterState.Fishing);
		Invoke("FishingStart", Random.Range(watingTimeMin, watingTimeMax));
	}

	public void FishingSuccessed()
	{
		if(m_PlayerCharacter != null)
		{
			m_PlayerCharacter.m_Inventory.AddAItem(m_CatchableItemlist[Random.Range(0, m_CatchableItemlist.Count - 1)]);
		}
	}

	private void FishingStart()
	{
		currentFishingTime = fishingTime;
		if (m_PlayerCharacter != null)
		{
			m_PlayerCharacter.ChangeAnimationState(PlayerCharacterAnimation.FishingStart);
		}
	}

	private void FishingEnd()
	{
		if (m_PlayerCharacter != null)
		{
			m_PlayerCharacter.ChangeState(PlayerCharacterState.Moveable);
			m_PlayerCharacter.ChangeAnimationState(PlayerCharacterAnimation.FishingEnd);
			m_PlayerCharacter = null;
		}
	}

	private void Start()
	{
		//m_CatchableItemlist을 초기화하는 부분 필요
		//아이템의 등장확률 초기화 필요
	}

	private void Update()
	{
		float DeltaTime = Time.deltaTime;

		if(currentFishingTime > 0)
		{
			currentFishingTime = currentFishingTime - DeltaTime;
			if(currentFishingTime <= 0)
			{
				FishingEnd();
			}
		}
	}
}
