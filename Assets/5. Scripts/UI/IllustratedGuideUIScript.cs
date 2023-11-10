using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;


public class IllustratedGuideUIScript : UIScript
{
	[SerializeField] private Image itemSprite;
	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI itemScript;
	[SerializeField] private RectTransform elementBase;
	[SerializeField] private List<Image> elements = new List<Image>(3);
	[SerializeField] private Color[] elementColors = { new Color(), new Color(), new Color(), new Color(), new Color() };
	[SerializeField] private RectTransform buttonsParent;
	[SerializeField] private GameObject buttonPrefab;
	[HideInInspector] public IllustratedGuideComponent m_IllustratedGuideComponent;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	public override void RefresfAction()
	{
		base.RefresfAction();

		if (m_IllustratedGuideComponent != null)
		{
			//GenerateButton
			SetButtonAmount(m_IllustratedGuideComponent.GetItemCount());
		}
			
		if(buttonsParent != null)
		{
			//initialize Button Action
			for (int i = 0; i < buttonsParent.childCount; i = i + 1)
			{
				Button button = buttonsParent.GetChild(i).GetComponent<Button>();
				SetButtonImageAndEvent(button, i);
			}
		}
	}

	private void SetButtonAmount(int pButtonAmount)
	{
		if (m_IllustratedGuideComponent != null)
		{
			if (buttonsParent != null)
			{
				int count = buttonsParent.childCount - pButtonAmount;
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
						GameObject t_GO = buttonsParent.GetChild(buttonsParent.childCount - i - 1).gameObject;
						Destroy(t_GO);
					}
				}
			}
		}
	}
	private void SetButtonImageAndEvent(Button pButton, int pButtonIndex)
	{
		if (pButton != null)
		{
			int itemCode = 0;
			if (m_IllustratedGuideComponent != null)
			{ itemCode = m_IllustratedGuideComponent[pButtonIndex]; }

			SetButtonSprite(pButton, itemCode);

			pButton.onClick.RemoveAllListeners();
			pButton.onClick.AddListener(() =>
			{
				SetGuideSpriteAndScript(itemCode);
			});
		}
	}
	private void SetButtonSprite(Button button, int pItemCode)
	{
		if(button != null)
		{
			GameObject t_GO = UniFunc.GetChildOfName(button.transform, "ItemImage");
			if (t_GO != null)
			{
				Image t_Image = t_GO.GetComponent<Image>();
				if (t_Image != null)
				{
					t_Image.gameObject.SetActive(true);
					t_Image.sprite = UniFunc.FindSprite(pItemCode);
				}
			}
			t_GO = UniFunc.GetChildOfName(button.transform, "ItemAmountText (TMP)");
			if (t_GO != null) { t_GO.transform.parent.gameObject.SetActive(false); }
		}
	}
	private void SetGuideSpriteAndScript(int pItemCode)
	{
		if (itemSprite != null)
		{
			itemSprite.sprite = UniFunc.FindSprite(pItemCode);
		}

		BasicItemData basicItemData = UniFunc.FindItemData(pItemCode);
		if (basicItemData != null)
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
		SetElementsColorAndRatio(pItemCode);
	}
	private void SetElementsColorAndRatio(int pItemCode)
	{
		if(pItemCode > 0)
		{
			if (elements != null)
			{
				if (elements.Count >= 3)
				{
					MaterialItemData t_MaterialItemData = null;
					if(GameManager.Instance != null)
					{
						if (GameManager.Instance.ItemManager != null) 
						{ t_MaterialItemData = GameManager.Instance.ItemManager.GetBasicItemData(pItemCode) as MaterialItemData; }
					}
					if(t_MaterialItemData != null)
					{
						float elementPercent = t_MaterialItemData.elementPercent1 + t_MaterialItemData.elementPercent2 + t_MaterialItemData.elementPercent3;

						Vector2 originSize = elements[2].rectTransform.sizeDelta;
						originSize.x = elementBase.sizeDelta.x * (t_MaterialItemData.elementPercent1 / elementPercent);
						elements[2].rectTransform.sizeDelta = originSize;
						elements[2].color = elementColors[(t_MaterialItemData.elementType1 - 1) < 0 ? 0 : (t_MaterialItemData.elementType1 - 1)];

						originSize = elements[1].rectTransform.sizeDelta;
						originSize.x = elementBase.sizeDelta.x * ((t_MaterialItemData.elementPercent1 + t_MaterialItemData.elementPercent2) / elementPercent);
						elements[1].rectTransform.sizeDelta = originSize;
						elements[1].color = elementColors[(t_MaterialItemData.elementType2 - 1) < 0 ? 0 : (t_MaterialItemData.elementType2 - 1)];

						//originSize = elements[0].rectTransform.sizeDelta;
						//originSize.x = elementBase.sizeDelta.x;
						//elements[0].rectTransform.sizeDelta = originSize;
						//elements[0].color = elementColors[t_MaterialItemData.elementType3 - 1];
					}
				}
			}
		}
	}
}
