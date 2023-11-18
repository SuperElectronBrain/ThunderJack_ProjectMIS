using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DialogBoxSizeController : MonoBehaviour
{
	[SerializeField] private SpriteRenderer m_DialogBack;
	[SerializeField] private TextMeshPro m_DialogScript;
	[SerializeField] private Transform m_NPCName;
	[SerializeField] private Vector3 m_NPCNameOriginPosition;

	[SerializeField] private float value0 = 0.6f; //위 아래 간격
	[SerializeField] private float value1 = 0.238f; //한 줄 크기
	[SerializeField] private float value2 = 1.485f; //

	// Start is called before the first frame update
	void Start()
	{
		//m_NPCNameOriginPosition = m_NPCName.transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 size = m_DialogBack.transform.localScale;
		size.y = value0 + (value1 * Mathf.RoundToInt(m_DialogScript.rectTransform.rect.height / value2));
		m_DialogBack.transform.localScale = size;
		
		Vector3 vector = m_DialogBack.transform.localPosition;
		vector.y = (m_DialogBack.transform.localScale.y - 1) * ((m_DialogBack.sprite.rect.height / m_DialogBack.sprite.pixelsPerUnit) / 2);
		m_DialogBack.transform.localPosition = vector;
		m_DialogScript.transform.localPosition = vector;
		m_NPCName.transform.localPosition = new Vector3(m_NPCNameOriginPosition.x, m_NPCNameOriginPosition.y * size.y, m_NPCNameOriginPosition.z) + vector;
	}
}
