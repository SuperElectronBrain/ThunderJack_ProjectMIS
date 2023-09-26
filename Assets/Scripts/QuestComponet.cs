using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Quest
{
	public int questID;
	public string questName;
	public string questScript;
	public int questGrade;
	public int requestItemID;
	public int guestID;
	public string guestName;
	public int timeLimit;
	public bool bComplete;

	public Quest(string p_String = null)
	{
		questID = 0;
		questName = null;
		questScript = null;
		questGrade = 0;
		requestItemID = 0;
		guestID = 0;
		guestName = null;
		timeLimit = 0;
		bComplete = false;
	}

	public Quest(int p_QuestID, string p_QuestName, string p_QuestScript, int p_QuestGrade, int p_RequestItemID, int p_GuestID, string p_GuestName, int p_TimeLimit, bool p_Complete)
	{
		questID = p_QuestID;
		questName = p_QuestName;
		questScript = p_QuestScript;
		questGrade = p_QuestGrade;
		requestItemID = p_RequestItemID;
		guestID = p_GuestID;
		guestName = p_GuestName;
		timeLimit = p_TimeLimit;
		bComplete = p_Complete;
	}

	public static implicit operator Quest(string p_String) { return new Quest(); }
	public static implicit operator string(Quest p_Quest)
	{
		string t_String = null;
		if (p_Quest.questID != 0) { t_String = p_Quest.ToString(); }
		return t_String;
	}
}

public class QuestComponet : MonoBehaviour
{
	[SerializeField] private List<Quest> m_Quests = new List<Quest>();

	public QuestListUIScript m_QuestListUIScript;
	public MailBoxUIScript m_MailBoxUIScript;

	// Start is called before the first frame update
	void Start()
	{
		EventManager.Subscribe(EventType.Day, ProgressQuestTimeLimit);
	}

	// Update is called once per frame
	//void Update()
	//{
	//	
	//}

	public void AddQuest(Quest p_Quest)
	{
		int count = 0;
		for(int i = 0; i < m_Quests.Count; i = i + 1)
		{
			if (m_Quests[i].questID == p_Quest.questID)
			{
				m_Quests[i] = p_Quest;
				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			m_Quests.Add(p_Quest);
		}
	}

	public List<Quest> GetQuests()
	{
		return new List<Quest>(m_Quests);
	}

	public bool CompleteQuest(int p_GuestID, bool p_Complete)
	{
		for(int i = 0; i < m_Quests.Count; i = i + 1)
		{
			if (m_Quests[i].timeLimit < 1)
			{
				if (m_Quests[i].guestID == p_GuestID)
				{
					Quest t_Quest = m_Quests[i];
					t_Quest.bComplete = p_Complete;
					m_Quests[i] = t_Quest;

					return true;
				}
			}
		}

		return false;
	}

	void ProgressQuestTimeLimit()
	{
		int count = m_Quests.Count;
		for(int i = 0; i < count; i = i + 1)
		{
			Quest t_Quest = m_Quests[i];
			t_Quest.timeLimit = t_Quest.timeLimit - 1;
			m_Quests[i] = t_Quest;

			if (m_Quests[i].timeLimit < 0)
			{
				m_Quests.RemoveAt(i);
			}
		}

		m_Quests.TrimExcess();
	}
}
