using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class IllustratedGuideUIScript : UIScript
{
	[SerializeField] private Image itemSprite;
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
					for (int i = 0; i < count; i = i + 1) { Destroy(buttonsParent.GetChild(buttonsParent.childCount - 1)); }
				}

				//initialize Button Action
				for (int i = 0; i < buttonsParent.childCount; i = i + 1)
				{
					GameObject t_GO = UniFunc.GetChildOfName(buttonsParent.GetChild(i).transform, "ItemImage");
					if (t_GO != null)
					{
						Image t_Image = t_GO.GetComponent<Image>();
						if (t_Image != null)
						{
							t_Image.gameObject.SetActive(true);
							t_Image.sprite = UniFunc.FindSprite(m_IllustratedGuideComponent[i]);
						}
					}
					t_GO = UniFunc.GetChildOfName(buttonsParent.GetChild(i).transform, "ItemAmountText (TMP)");
					if (t_GO != null) { t_GO.transform.parent.gameObject.SetActive(false); }

					Button button = buttonsParent.GetChild(i).GetComponent<Button>();
					if(button != null)
					{
						int number = i;

						button.onClick.RemoveAllListeners();
						button.onClick.AddListener(() => 
						{
							if(itemSprite != null)
							{
								itemSprite.sprite = UniFunc.FindSprite(m_IllustratedGuideComponent[number]);
							}
							if(itemScript != null)
							{
								itemScript.text = UniFunc.FindItemData(m_IllustratedGuideComponent[number]).itemText;
							}
						});
					}
				}
			}
		}
	}
}
