using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUIScript : MonoBehaviour
{
	public TextMeshProUGUI m_MoneyText;
	public TextMeshProUGUI m_HonerText;
	public TextMeshProUGUI m_AddMoneyText;
	public TextMeshProUGUI m_AddHonerText;

	[SerializeField] private Animator m_HonerAnimator;
	[SerializeField] private Animator m_MoneyAnimator;
	[SerializeField] private string m_UpTriggerName = "Up";
	[SerializeField] private string m_DownTriggerName = "Down";

	// Start is called before the first frame update
	void Start()
    {
		if (m_MoneyText == null) { m_MoneyText = UniFunc.GetChildOfName(transform, "MoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_HonerText == null) { m_HonerText = UniFunc.GetChildOfName(transform, "HonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_AddMoneyText == null) { m_AddMoneyText = UniFunc.GetChildOfName(transform, "AddMoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_AddHonerText == null) { m_AddHonerText = UniFunc.GetChildOfName(transform, "AddHonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
	}

	public void AddMoneyText(int param)
	{
		if (m_AddMoneyText != null)
		{
			m_AddMoneyText.gameObject.SetActive(true);
			m_AddMoneyText.text = param + "";
			if(m_MoneyAnimator != null)
			{
				if(param > 0) { m_MoneyAnimator.SetTrigger(m_UpTriggerName); }
				else if(param < 0) { m_MoneyAnimator.SetTrigger(m_DownTriggerName); }
			}
			Invoke("MoneyTextDeactivate", 3);
		}
	}

	private void MoneyTextDeactivate()
	{
		if (m_AddMoneyText != null) { m_AddMoneyText.gameObject.SetActive(false); }
	}

	public void AddHonerText(int param)
	{
		if (m_AddHonerText != null)
		{
			m_AddHonerText.gameObject.SetActive(true);
			m_AddHonerText.text = param + "";
			if (m_HonerAnimator != null)
			{
				if (param > 0) { m_HonerAnimator.SetTrigger(m_UpTriggerName); }
				else if (param < 0) { m_HonerAnimator.SetTrigger(m_DownTriggerName); }
			}
			Invoke("HonerTextDeactivate", 3);
		}
	}

	private void HonerTextDeactivate()
	{
		if (m_AddHonerText != null) { m_AddHonerText.gameObject.SetActive(false); }
	}
}
