using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacterUIScript : MonoBehaviour
{
	public ClockUIScript m_ClockUIScript;
	public TextMeshProUGUI m_MoneyText;
	public TextMeshProUGUI m_HonerText;
	public InventoryUIScript m_InventoryUIScript;
	public UnityEngine.UI.Image m_MouseGrabIcon;

	// Start is called before the first frame update
	void Start()
	{
		if(m_ClockUIScript == null) { m_ClockUIScript = UniFunc.GetChildComponent<ClockUIScript>(transform); }
		if(m_MoneyText == null) { m_MoneyText = UniFunc.GetChildOfName(transform, "MoneyText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_HonerText == null) { m_HonerText = UniFunc.GetChildOfName(transform, "HonerText (TMP)").GetComponent<TextMeshProUGUI>(); }
		if (m_InventoryUIScript == null) { m_InventoryUIScript = UniFunc.GetChildComponent<InventoryUIScript>(transform); }
		if (m_MouseGrabIcon == null) { m_MouseGrabIcon = UniFunc.GetChildOfName(transform, "MouseGrabItem").GetComponent<UnityEngine.UI.Image>(); }
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}
}
