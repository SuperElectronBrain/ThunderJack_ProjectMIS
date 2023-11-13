using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{ WaitingTutorial, MovePointToPoint, PopUpMonologue, PopUpGuide, WaitingKeyboardInput, WaitingFewSeconds, WaitingToEnterArea, FadeIn, FadeOut, Teleport, SpawnNPC, EndTutorial, }

[Serializable]
public struct MonologueScript
{
	public string script;
	public float popUpTime;
}

[Serializable]
public struct GuideUI
{
	public string guideUIName;
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
	public GuideUI guideUI;
	public float waitingTime;
	public float fadeSpeed;
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
		if(m_CurrenStatetType == StateType.WaitingTutorial)
		{
			ProgressTutorial();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	public List<TutorialState> GetStates()
	{
		return new List<TutorialState>(m_States);
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
	
	public TutorialState PopStateAt(int p_Index)
	{
		TutorialState t_States = m_States[p_Index];
		m_States.RemoveAt(p_Index);
		m_States.TrimExcess();
		return t_States;
	}
	
	public void CleanStates()
	{
		m_States.Clear();
		m_States.TrimExcess();
	}
	
	public void TakeStates(TutorialComponent p_TutorialComponent)
	{
		if (p_TutorialComponent != null)
		{
			m_PlayerCharacter = GetComponent<PlayerCharacter>();
			m_CurrentState = p_TutorialComponent.m_CurrentState;
			m_CurrenStatetType = p_TutorialComponent.m_CurrenStatetType;
			CleanStates();
			int count = p_TutorialComponent.GetStates().Count;
			for (int i = 0; i < count; i = i + 1)
			{
				m_States.Add(p_TutorialComponent.PopStateAt(0));
			}
			p_TutorialComponent.CleanStates();
		}
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
					case StateType.PopUpGuide:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.PopUpGuide(m_States[m_CurrentState].guideUI.guideUIName, m_States[m_CurrentState].guideUI.popUpTime);
						}
						break;
					}
					case StateType.WaitingKeyboardInput:
					{
						break;
					}
					case StateType.WaitingFewSeconds:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.PopUpGuide("", m_States[m_CurrentState].waitingTime);
						}
						break;
					}
					case StateType.WaitingToEnterArea:
					{
						break;
					}
					case StateType.FadeIn:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.FadeIn(m_States[m_CurrentState].fadeSpeed);
						}
						break;
					}
					case StateType.FadeOut:
					{
						if (m_PlayerCharacter != null)
						{
							m_PlayerCharacter.FadeOut(m_States[m_CurrentState].fadeSpeed);
						}
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
