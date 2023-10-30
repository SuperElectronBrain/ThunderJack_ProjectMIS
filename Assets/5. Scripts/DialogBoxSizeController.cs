using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogBoxSizeController : MonoBehaviour
{
	[SerializeField] private SpriteRenderer m_DialogBack;
	[SerializeField] private TextMeshPro m_DialogScript;
	[SerializeField] private Transform m_NPCName;
	private Vector3 m_NPCNameOriginPosition;

	// Start is called before the first frame update
	void Start()
	{
		m_NPCNameOriginPosition = m_NPCName.transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 size = m_DialogBack.transform.localScale;
		size.y = 0.6f + (0.238f * Mathf.RoundToInt(m_DialogScript.rectTransform.rect.height / 1.485f));
		m_DialogBack.transform.localScale = size;

		Vector3 vector = m_DialogBack.transform.localPosition;
		vector.y = (m_DialogBack.transform.localScale.y - 1) * ((m_DialogBack.sprite.rect.height / m_DialogBack.sprite.pixelsPerUnit) / 2);
		m_DialogBack.transform.localPosition = vector;
		m_DialogScript.transform.localPosition = vector;
		m_NPCName.transform.localPosition = new Vector3(m_NPCNameOriginPosition.x, m_NPCNameOriginPosition.y * size.y, m_NPCNameOriginPosition.z) + vector;
	}
}
