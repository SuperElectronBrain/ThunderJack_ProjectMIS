using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{ WaitingTutorial, MovePointToPoint, PopUpMonologue, Fade, Teleport, SpawnNPC, EndTutorial, }

[Serializable]
public struct MonologueScript
{
	public string script;
	public float popUpTime;
}

[Serializable]
public struct NPCInfo
{
	public GameObject NPCPrefab;
	public Transform spawnMarker;
}

[Serializable]
public class TutorialState
{
	public StateType stateType;
	public Transform locationMarker;
	public MonologueScript monologueScript;
	public NPCInfo spawnNPC;
}

public class TutorialComponent : MonoBehaviour
{
	[SerializeField] private PlayerCharacter m_PlayerCharacter;
	[SerializeField] private List<TutorialState> m_States = new List<TutorialState>();
	private int m_CurrentState;
	private StateType m_CurrenStatetType = StateType.WaitingTutorial;

	// Start is called before the first frame update
	void Start()
	{
		m_PlayerCharacter = GetComponent<PlayerCharacter>();
		m_CurrentState = 0;
		ProgressTutorial();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public StateType GetCurrentStateType()
	{
		//StateType t_StateType = StateType.EndTutorial;
		//if (m_States != null)
		//{
		//	if (m_CurrentState >= 0 && m_CurrentState < m_States.Count)
		//	{
		//		t_StateType = m_States[m_CurrentState].stateType;
		//	}
		//}
		return m_CurrenStatetType;
	}

	public void ProgressTutorial()
	{
		if (m_States != null)
		{
			if (m_CurrentState >= 0 && m_CurrentState < m_States.Count)
			{
				if (m_PlayerCharacter != null) { m_PlayerCharacter.SetDestination(null); }
				m_CurrenStatetType = m_States[m_CurrentState].stateType;
				switch (m_States[m_CurrentState].stateType)
				{
					case StateType.MovePointToPoint:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.SetDestination(m_States[m_CurrentState].locationMarker);
						}

						break;
					}
					case StateType.PopUpMonologue:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.PopUpMonologue(m_States[m_CurrentState].monologueScript.script, m_States[m_CurrentState].monologueScript.popUpTime);
						}
						break;
					}
					case StateType.Fade:
					{
						break;
					}
					case StateType.Teleport:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.transform.position = m_States[m_CurrentState].locationMarker.position;
						}
						break;
					}
					case StateType.SpawnNPC:
					{
						break;
					}
				}

				m_CurrentState = m_CurrentState + 1;
				if (m_CurrentState >= m_States.Count) { m_CurrentState = 0; }
			}
		}
			
	}
}
