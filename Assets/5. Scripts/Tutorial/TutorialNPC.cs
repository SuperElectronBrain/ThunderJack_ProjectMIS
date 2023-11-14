using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TutorialNPC : MonoBehaviour
{
	[SerializeField] private GameObject m_SpeechBubble;
	[SerializeField] private TextMeshPro m_SpeechBubbleText;
	[SerializeField] private UnityEngine.UI.Text m_SpeechBubbleTextLegacy;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	public void PopUpSpeechBubble(string p_Script, bool bParam)
	{
		if (m_SpeechBubble != null)
		{
			if (m_SpeechBubble.activeSelf != bParam) { m_SpeechBubble.SetActive(bParam); }
			if (m_SpeechBubbleText != null)
			{
				if (m_SpeechBubbleTextLegacy != null)
				{
					m_SpeechBubbleTextLegacy.text = p_Script;

					m_SpeechBubbleTextLegacy.DOKill();
					m_SpeechBubbleTextLegacy.text = null;
					m_SpeechBubbleTextLegacy.DOText(p_Script, 1f).OnUpdate(() =>
					{
						m_SpeechBubbleText.text = m_SpeechBubbleTextLegacy.text;
					});
				}
			}
		}
	}
}
