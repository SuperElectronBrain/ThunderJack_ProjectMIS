using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestListUIScript : MonoBehaviour
{
	public GameObject m_ElementPrefab;
	public GameObject m_ElementsParent;
	public List<GameObject> m_Elements;
	public QuestComponet m_QuestComponet;

	// Start is called before the first frame update
	void Start()
	{
		ReFindElement();
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	public void ReFindElement()
	{
		if (m_ElementsParent != null)
		{
			m_Elements.Clear();
			m_Elements.TrimExcess();
			for (int i = 0; i < m_ElementsParent.transform.childCount; i = i + 1)
			{
				GameObject t_Element = m_ElementsParent.transform.GetChild(i).gameObject;
				if (t_Element != null)
				{
					m_Elements.Add(t_Element);
				}
			}
		}
	}

	public void ReGenerateElement()
	{
		if (m_ElementPrefab != null)
		{
			if (m_QuestComponet != null)
			{
				int ItemsCount = m_QuestComponet.GetQuests().Count;
				if (m_Elements.Count < ItemsCount)
				{
					int count = ItemsCount - m_Elements.Count;
					for (int i = 0; i < count; i = i + 1)
					{
						m_Elements.Add(Instantiate(m_ElementPrefab, m_ElementsParent.transform));
					}
				}
				else if (m_Elements.Count > ItemsCount)
				{
					int count = m_Elements.Count - ItemsCount;
					for (int i = 0; i < count; i = i + 1)
					{
						GameObject t_Element = m_Elements[m_Elements.Count - 1];
						m_Elements.RemoveAt(m_Elements.Count - 1);
						m_Elements.TrimExcess();
						if (t_Element != null)
						{
							Destroy(t_Element.gameObject);
						}
					}
				}
			}
		}
	}

	public void RefreshUI()
	{
		ReFindElement();
		ReGenerateElement();

		if(m_QuestComponet != null)
		{
			List<Quest> t_Quests = m_QuestComponet.GetQuests();

			for (int i = 0; i < m_Elements.Count; i = i + 1)
			{
				TextMeshProUGUI t_Text = UniFunc.GetChildComponent<TextMeshProUGUI>(m_Elements[i]);
				if(t_Text != null)
				{
					string t_String = "";
					t_String = t_String + (t_Quests[i].questGrade == 1 ? "[일반] " : (t_Quests[i].questGrade == 2 ? "[상급] " : (t_Quests[i].questGrade == 3 ? "[최상급] " : "")));
					t_String = t_String + t_Quests[i].questName + "\n";
					t_String = t_String + UniFunc.FindItemData(t_Quests[i].requestItemID).itemNameKo + " " + (t_Quests[i].bComplete == true ? "1" : "0") + "/1" + "\n";
					t_String = t_String + "의뢰자 : " + t_Quests[i].guestName + "\n";
					t_String = t_String + "D" + (t_Quests[i].timeLimit != 0 ? (-t_Quests[i].timeLimit) : "-Day") + "\n";

					t_Text.text = t_String;
				}
			}
		}
	}
}
