using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AdvencedQuestData
{
	public int questID;
	public string questName;
	public string questScript;
	public int questGrade;
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
		questGrade = 0;
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
	private bool bCommunication; public bool IsUsed { get { return bCommunication; } set { bCommunication = value; } }

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
			List<QuestData> QuestDatas = GameManager.Instance.QuestManager.GetQuestList();
			if(QuestDatas != null)
			{
				QuestTable = new List<AdvencedQuestData>();
				for (int i = 0; i < QuestDatas.Count; i = i + 1)
				{
					AdvencedQuestData t_AQuestData = new AdvencedQuestData();
					t_AQuestData.questID = QuestDatas[i].questID;
					t_AQuestData.questName = QuestDatas[i].questName;
					t_AQuestData.questScript = QuestDatas[i].questScript;
					t_AQuestData.questGrade = QuestDatas[i].questGrade;
					t_AQuestData.requestItemID = QuestDatas[i].requestItemID;
					t_AQuestData.guestName = QuestDatas[i].questCharacter;
					t_AQuestData.timeLimit = QuestDatas[i].questTimeLimit;
					t_AQuestData.startRate = QuestDatas[i].startRate;
					t_AQuestData.resetRate = QuestDatas[i].resetRate;
					t_AQuestData.rate = QuestDatas[i].rate;
					t_AQuestData.dayRate = QuestDatas[i].dayRate;
					QuestTable.Add(t_AQuestData);
				}
			}
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
					if (t_QuestData.rate < 0) { t_QuestData.rate = 0; }
					if (t_QuestData.rate > 1) { t_QuestData.rate = 1; }
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

	public void Interaction(GameObject p_GameObject)
	{
		if(m_QuestData.questID != 0)
		{
			if(p_GameObject != null)
			{
				PlayerCharacter t_PalyerCharacter = p_GameObject.GetComponent<PlayerCharacter>();
				if(t_PalyerCharacter != null)
				{
					if (t_PalyerCharacter.m_QuestComponet != null)
					{
						if (t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript != null)
						{
							t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript.DisplayMail(true, "");
						}

						t_PalyerCharacter.m_QuestComponet.AddQuest
						(new Quest(m_QuestData.questID, m_QuestData.questName, m_QuestData.questScript, m_QuestData.questGrade, m_QuestData.requestItemID, m_QuestData.guestID, m_QuestData.guestName, m_QuestData.timeLimit, false));
						if (t_PalyerCharacter.m_QuestComponet.m_QuestListUIScript != null)
						{
							t_PalyerCharacter.m_QuestComponet.m_QuestListUIScript.RefreshUI();
						}

						m_QuestData = new AdvencedQuestData();
						if (m_SD != null)
						{
							m_SD.SetActive(false);
						}
					}
				}
			}
		}
	}
}
