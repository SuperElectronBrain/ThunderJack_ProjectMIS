using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPointComponent : MonoBehaviour, IInteraction
{
	[SerializeField] private KeyCode fishingKey;
	[SerializeField] float firstImpactTimeMin = 0.0f;
	[SerializeField] float firstImpactTimeMax = 3.0f;
	[SerializeField] float secondImpactTimeMin = 0.0f;
	[SerializeField] float secondImpactTimeMax = 3.0f;
	private float currentFishingTime = 0.0f;

	[SerializeField] private List<AdvencedItem> m_CatchableItemlist = new List<AdvencedItem>();
	private PlayerCharacter m_PlayerCharacter;
	public bool IsUsed { get; set; }

	public void Interaction(GameObject user)
	{
		PlayerCharacter t_PlayerCharacter = user.GetComponent<PlayerCharacter>();
		if(t_PlayerCharacter == null) { return; }

		m_PlayerCharacter = t_PlayerCharacter;
		EventManager.Publish(EventType.StartInteraction);
		m_PlayerCharacter.ChangeState(PlayerCharacterState.Fishing);
		
		//Invoke("FirstImpact", Random.Range(firstImpactTimeMin, firstImpactTimeMax));
		FirstImpact();
	}

	private void FirstImpact()
	{
		if (m_PlayerCharacter != null)
		{
			m_PlayerCharacter.ChangeAnimationState(PlayerCharacterAnimation.FishingHit);
		}
		Invoke("SecondImpact", Random.Range(firstImpactTimeMin, firstImpactTimeMax));
	}

	private void SecondImpact()
	{
		currentFishingTime = Random.Range(secondImpactTimeMin, secondImpactTimeMax);
		if (m_PlayerCharacter != null)
		{
			m_PlayerCharacter.ChangeAnimationState(PlayerCharacterAnimation.FishingHit);
		}
	}

	public void FishingSuccessed()
	{
		if(m_PlayerCharacter != null)
		{
			m_PlayerCharacter.m_Inventory.AddAItem(m_CatchableItemlist[Random.Range(0, m_CatchableItemlist.Count - 1)]);
		}
	}

	private void FishingEnd()
	{
		currentFishingTime = 0;
		CancelInvoke("SecondImpact");
		if (m_PlayerCharacter != null)
		{
			EventManager.Publish(EventType.EndIteraction);
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
			if (Input.GetKeyDown(fishingKey) == true)
			{
				FishingSuccessed();
				FishingEnd();
			}

			currentFishingTime = currentFishingTime - DeltaTime;
			if(currentFishingTime <= 0)
			{
				FishingEnd();
			}
		}
		else if(currentFishingTime <= 0)
		{
			if (Input.GetKeyDown(fishingKey) == true)
			{
				FishingEnd();
			}
		}
	}
}
