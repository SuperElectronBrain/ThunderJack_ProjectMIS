using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class IllustratedGuideUIScript : UIScript
{
	[SerializeField] private Image itemSprite;
	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI itemScript;
	[SerializeField] private float[] elementalRatio = { 0.0f, 0.0f, 0.0f};
	[SerializeField] private RectTransform buttonsParent;
	[SerializeField] private GameObject buttonPrefab;
	[HideInInspector] public IllustratedGuideComponent m_IllustratedGuideComponent;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public override void RefresfAction()
	{
		base.RefresfAction();

		if(m_IllustratedGuideComponent != null)
		{
			if(buttonsParent != null)
			{
				//GenerateButton
				int count = buttonsParent.childCount - m_IllustratedGuideComponent.GetItemCount();
				if (count < 0)
				{
					for (int i = 0; i < (-count); i = i + 1)
					{
						if (buttonPrefab != null) { Instantiate(buttonPrefab, buttonsParent); }
					} 
				}
				else if (count > 0)
				{
					for (int i = 0; i < count; i = i + 1) 
					{ 
						Destroy((buttonsParent.GetChild(buttonsParent.childCount - 1)).gameObject); 
					}
				}

				//initialize Button Action
				for (int i = 0; i < buttonsParent.childCount; i = i + 1)
				{
					Transform buttonTransform = buttonsParent.GetChild(i);

					GameObject t_GO = UniFunc.GetChildOfName(buttonTransform, "ItemImage");
					if (t_GO != null)
					{
						Image t_Image = t_GO.GetComponent<Image>();
						if (t_Image != null)
						{
							t_Image.gameObject.SetActive(true);
							t_Image.sprite = UniFunc.FindSprite(m_IllustratedGuideComponent[i]);
						}
					}
					t_GO = UniFunc.GetChildOfName(buttonTransform, "ItemAmountText (TMP)");
					if (t_GO != null) { t_GO.transform.parent.gameObject.SetActive(false); }

					Button button = buttonTransform.GetComponent<Button>();
					if(button != null)
					{
						int number = i;

						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() => 
						{
							int itemCode = m_IllustratedGuideComponent[number];
							if (itemSprite != null)
							{
								itemSprite.sprite = UniFunc.FindSprite(itemCode);
							}

							BasicItemData basicItemData = UniFunc.FindItemData(itemCode);
							if(basicItemData != null)
							{
								if (itemName != null)
								{
									itemName.text = basicItemData.itemNameKo;
								}
								if (itemScript != null)
								{
									itemScript.text = basicItemData.itemText;
								}
							}
						});
					}
				}
			}
		}
	}
}
