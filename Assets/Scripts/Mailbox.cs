using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AdvencedQuestData
{
	public int questID;
	public string questName;
	public string questScript;
	public int requestItemID;
	public int guestID;
	public string guestName;
	public int timeLimit;
	public float startRate;
	public float resetRate;
	public float rate;
	public float dayRate;
	public float presentRate;
	public int waitingTime;

	public AdvencedQuestData(string t_String = null)
	{
		questID = 0;
		questName = null;
		questScript = null;
		requestItemID = 0;
		guestID = 0;
		guestName = null;
		timeLimit = 0;
		startRate = 0.0f;
		resetRate = 0.0f;
		rate = 0.0f;
		dayRate = 0.0f;
		presentRate = 0.0f;
		waitingTime = 0;
	}
}

public class Mailbox : MonoBehaviour, IInteraction
{
	[SerializeField] [Range(0.0f, 1.0f)] private float rate = 0.3f;
	private List<AdvencedQuestData> QuestTable = null;
	private AdvencedQuestData m_QuestData;

	[SerializeField] private GameObject m_SD;
	private PlayerCharacter m_PlayerCharacter;

	// Start is called before the first frame update
	void Start()
    {
		EventManager.Subscribe(EventType.Day, GenerateQuest);
	}

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

	void GenerateQuest()
	{
		if(QuestTable == null)
		{
			//QuestTable = GameManager.Instance.QuestManager.GetQuestData();
		}

		if (QuestTable != null)
		{
			if (Random.Range(0.0f, 1.0f) < rate)
			{
				float t_Rate = 0;
				int index = -1;
				for (int i = 0; i < QuestTable.Count; i = i + 1)
				{
					float t_Random = Random.Range(0.0f, QuestTable[i].presentRate);
					if (t_Random >= t_Rate)
					{
						t_Rate = t_Random;
						index = i;
					}
				}

				if(m_QuestData.questID == 0)
				{
					m_QuestData = QuestTable[index];

					AdvencedQuestData t_QuestData = QuestTable[index];
					t_QuestData.presentRate = QuestTable[index].resetRate;
					t_QuestData.waitingTime = QuestTable[index].timeLimit;
					QuestTable[index] = t_QuestData;
				}
			}

			//

			for (int i = 0; i < QuestTable.Count; i = i + 1)
			{
				if(QuestTable[i].waitingTime < 1)
				{
					AdvencedQuestData t_QuestData = QuestTable[i];
					t_QuestData.presentRate = QuestTable[i].presentRate + QuestTable[i].rate;
					t_QuestData.rate = QuestTable[i].rate + QuestTable[i].dayRate;
					QuestTable[i] = t_QuestData;
				}
				else if(QuestTable[i].waitingTime > 0)
				{
					AdvencedQuestData t_QuestData = QuestTable[i];
					t_QuestData.waitingTime = QuestTable[i].waitingTime - 1;
					QuestTable[i] = t_QuestData;
				}
			}
		}

		if (m_QuestData.questID != 0)
		{
			if (m_SD != null)
			{
				m_SD.SetActive(true);
			}
		}
	}

	public void Interaction()
	{
		if(m_QuestData.questID != 0)
		{
			if (m_PlayerCharacter == null)
			{
				m_PlayerCharacter = FindObjectOfType<PlayerCharacter>();
			}

			if (m_PlayerCharacter.m_QuestComponet != null)
			{
				if (m_PlayerCharacter.m_QuestComponet.m_MailBoxUIScript != null)
				{
					m_PlayerCharacter.m_QuestComponet.m_MailBoxUIScript.DisplayMail(true, "");
				}

				m_PlayerCharacter.m_QuestComponet.AddQuest
				(new Quest(m_QuestData.questID, m_QuestData.questName, m_QuestData.questScript, m_QuestData.requestItemID, m_QuestData.guestID, m_QuestData.guestName, m_QuestData.timeLimit));

				m_QuestData = new AdvencedQuestData();
				if (m_SD != null)
				{
					m_SD.SetActive(false);
				}
			}
		}
	}
}
