using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Quest
{
	public int questID;
	public string questName;
	public string questScript;
	public int requestItemID;
	public int guestID;
	public string guestName;
	public int timeLimit;

	public Quest(string p_String = null)
	{
		questID = 0;
		questName = null;
		questScript = null;
		requestItemID = 0;
		guestID = 0;
		guestName = null;
		timeLimit = 0;
	}

	public Quest(int p_QuestID, string p_QuestName, string p_QuestScript, int p_RequestItemID, int p_GuestID, string p_GuestName, int p_TimeLimit)
	{
		questID = p_QuestID;
		questName = p_QuestName;
		questScript = p_QuestScript;
		requestItemID = p_RequestItemID;
		guestID = p_GuestID;
		guestName = p_GuestName;
		timeLimit = p_TimeLimit;
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
	private List<Quest> Quests = new List<Quest>();

	public MailBoxUIScript m_MailBoxUIScript;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void AddQuest(Quest p_Quest)
	{
		int count = 0;
		for(int i = 0; i < Quests.Count; i = i + 1)
		{
			if (Quests[i].questID == p_Quest.questID)
			{
				Quests[i] = p_Quest;
				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			Quests.Add(p_Quest);
		}
	}
}
