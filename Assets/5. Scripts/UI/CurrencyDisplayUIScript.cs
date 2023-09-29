using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplayUIScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_MoneyText;
	[SerializeField] private TextMeshProUGUI m_HonerText;

	public int m_CurrentMoney = 0;
	public int m_CurrentHoner = 0;
	[SerializeField] private int m_MaxCurrency = 9999999;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float DeltaTime = Time.deltaTime;

		RefreshCurrencyDisplay();
	}

	private void RefreshCurrencyDisplay()
    {
		if(m_MoneyText != null)
		{
			m_MoneyText.text = (m_CurrentMoney > m_MaxCurrency ? m_MaxCurrency : m_CurrentMoney) + "";
		}
		if (m_HonerText != null)
		{
			m_HonerText.text = (m_CurrentHoner > m_MaxCurrency ? m_MaxCurrency : m_CurrentHoner) + "";
		}
	}
}
