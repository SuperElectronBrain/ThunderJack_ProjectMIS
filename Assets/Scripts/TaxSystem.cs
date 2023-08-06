using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxSystem : MonoBehaviour
{
	[SerializeField] private GameObject paymentPanelPrefab;
	private GameObject paymentPanel;
	private TextMeshProUGUI paymentText;
	private Inventory playerInventory;

	[SerializeField] private float paymentCycle = 300.0f;
	private float currentTime = 0.0f;
	[SerializeField] private int paymentMoney = 100;

	// Start is called before the first frame update
	void Start()
	{
		PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();
		if (playerCharacter != null)
		{
			playerInventory = playerCharacter.GetComponent<Inventory>();
		}

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{ 
			if(paymentPanelPrefab != null)
			{
				paymentPanel = Instantiate(paymentPanelPrefab, canvas.transform);

				paymentText = UniFunc.GetChildOfType<TextMeshProUGUI>(paymentPanel);
				if(paymentText == null)
				{
					for(int i = 0; i < paymentPanel.transform.childCount; i = i + 1)
					{
						paymentText = UniFunc.GetChildOfType<TextMeshProUGUI>(paymentPanel.transform.GetChild(i));
						if (paymentText != null)
						{
							break;
						}
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		float DeltaTime = Time.deltaTime;


		currentTime = currentTime + DeltaTime;
		if(paymentText != null)
		{
			paymentText.text = (int)(paymentCycle - currentTime) + "";
		}
		if (currentTime > paymentCycle)
		{
			PaymentExecution();

			currentTime = 0;
		}
	}

	private void PaymentExecution()
	{
		playerInventory.PopItem(ItemCode.Money, paymentMoney);
		playerInventory.RefreshInventory();
	}
}
