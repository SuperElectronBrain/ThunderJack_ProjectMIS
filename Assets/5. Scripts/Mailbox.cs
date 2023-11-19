using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
	public static Mailbox main
	{ get { return FindObjectOfType<Mailbox>(); } }
	
	[SerializeField] [Range(0.0f, 1.0f)] private float rate = 0.3f;
	private List<AdvencedQuestData> QuestTable = null;
	private AdvencedQuestData m_QuestData;
	private bool bCommunication; public bool IsUsed { get { return bCommunication; } set { bCommunication = value; } }
	[SerializeField] private GameObject m_SD;
	[SerializeField] private Animator m_Animator;
	[SerializeField] private AudioSource m_InteractionSound;

	private Vector2 m_PewpewOriginPosition;

	// Start is called before the first frame update
	void Start()
    {
		if (m_SD != null) 
		{
			m_PewpewOriginPosition = m_SD.transform.localPosition;
			m_SD.SetActive(false); 
		}
		//하루가 지날때마다 퀘스트를 생성함
		EventManager.Subscribe(EventType.Day, GenerateQuest);
	}

	void GenerateQuest()
	{
		if(QuestTable == null)
		{
			if(GameManager.Instance != null)
			{
				if(GameManager.Instance.QuestManager != null)
				{
					List<QuestData> QuestDatas = GameManager.Instance.QuestManager.GetQuestList();
					if (QuestDatas != null)
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

		//퀘스트를 생성하는데 성공했다면
		if (m_QuestData.questID != 0)
		{
			//퓨퓨를 활성화함
			if (m_SD != null) 
			{
				m_SD.transform.localPosition = m_PewpewOriginPosition;
				m_SD.SetActive(true);
			}
		}
	}

	public void Interaction(GameObject p_GameObject)
	{
		//생성되어있는 퀘스트가 존재한다면
		if (m_QuestData.questID != 0)
		{
			if(p_GameObject != null)
			{
				PlayerCharacter t_PalyerCharacter = p_GameObject.GetComponent<PlayerCharacter>();
				if(t_PalyerCharacter != null)
				{
					//플레이어 캐릭터에 QuestComponet가 붙어있다면
					if (t_PalyerCharacter.m_QuestComponet != null)
					{
						//플레이어 캐릭터에 MailBoxUI가 붙어있다면
						if (t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript != null)
						{
							t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript.DisplayMail(true, m_QuestData.questScript);
							t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript.m_OnButtonClick.AddListener(OnButtonClick);
						}

						//애니메이션 재생
						if (m_Animator != null) { m_Animator.SetTrigger("Interaction"); }
						if (m_InteractionSound != null) { if (m_InteractionSound.isPlaying == false) m_InteractionSound.Play(); }
					}
				}
			}
		}
	}

	public void AddQuest(AdvencedQuestData advencedQuestData)
	{
		m_QuestData = advencedQuestData;
		//퀘스트를 생성하는데 성공했다면
		if (m_QuestData.questID != 0)
		{
			//퓨퓨를 활성화함
			if (m_SD != null) { m_SD.SetActive(true); }
		}
	}

	void OnButtonClick(bool param)
	{
		if (param == true)
		{
			PlayerCharacter t_PalyerCharacter = PlayerCharacter.main;
			if (t_PalyerCharacter != null)
			{
				if (t_PalyerCharacter.m_QuestComponet != null)
				{
					t_PalyerCharacter.m_QuestComponet.AddQuest
					(
						new Quest
						(
							m_QuestData.questID,
							m_QuestData.questName,
							m_QuestData.questScript,
							m_QuestData.questGrade,
							m_QuestData.requestItemID,
							m_QuestData.guestID,
							m_QuestData.guestName,
							m_QuestData.timeLimit,
							false
						)
					);
					if (t_PalyerCharacter.m_QuestComponet.m_QuestListUIScript != null)
					{
						t_PalyerCharacter.m_QuestComponet.m_QuestListUIScript.RefreshUI();
					}

					if (t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript != null)
					{
						t_PalyerCharacter.m_QuestComponet.m_MailBoxUIScript.m_OnButtonClick.RemoveListener(OnButtonClick);
					}
				}
			}
		}

		//퀘스트 데이터를 null로 초기화함
		m_QuestData = new AdvencedQuestData();

		if (m_Animator != null) { m_Animator.SetTrigger("Fly"); }
		Invoke("Deactivate", 3.0f);
		InvokeRepeating("GoAwayes", 0, 0.02f);
		
	}

	void Deactivate()
	{
		CancelInvoke("GoAwayes");
		if (m_SD != null) { m_SD.SetActive(false); }
	}

	void GoAwayes()
	{
		if (m_SD != null)
		{
			Vector2 nextPosition = m_SD.transform.localPosition;
			nextPosition.y = nextPosition.y + 0.02f;
			m_SD.transform.localPosition = nextPosition;
		}
	}
}