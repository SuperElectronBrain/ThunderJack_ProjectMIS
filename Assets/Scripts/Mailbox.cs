using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour, IInteraction
{
	[SerializeField] [Range(0.0f, 1.0f)] private float rate = 0.3f;

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
		if(Random.Range(0.0f, 1.0f) < rate)
		{

		}
	}

	public void Interaction()
	{
		if(m_PlayerCharacter.m_QuestComponet != null)
		{
			if(m_PlayerCharacter.m_QuestComponet.m_MailBoxUIScript != null)
			{
				m_PlayerCharacter.m_QuestComponet.m_MailBoxUIScript.DisplayMail(true, "");
			}
		}
		//m_PlayerCharacter.
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerCharacter t_PlayerCharacter = other.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			m_PlayerCharacter = t_PlayerCharacter;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerCharacter t_PlayerCharacter = collision.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			m_PlayerCharacter = t_PlayerCharacter;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		PlayerCharacter t_PlayerCharacter = other.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			if (m_PlayerCharacter == t_PlayerCharacter)
			{
				m_PlayerCharacter = null;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerCharacter t_PlayerCharacter = collision.GetComponent<PlayerCharacter>();
		if (t_PlayerCharacter != null)
		{
			if (m_PlayerCharacter == t_PlayerCharacter)
			{
				m_PlayerCharacter = null;
			}
		}
	}
}
