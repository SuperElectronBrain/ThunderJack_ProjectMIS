using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingSystem : MonoBehaviour
{
	[SerializeField] private GameObject TradingPanelPrefab;
	private GameObject TradingPanel;
	[SerializeField] private GameObject SalesPanelPrefab;
	private GameObject SalesPanel;

	private PlayerCharacter playerCharacter;
	private Inventory inventory;
	private List<SelectedItem> selectedItems = new List<SelectedItem>();

	// Start is called before the first frame update
	void Start()
	{
		playerCharacter = gameObject.GetComponent<PlayerCharacter>();
		inventory = gameObject.GetComponent<Inventory>();

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (TradingPanelPrefab != null)
			{
				TradingPanel = Instantiate(TradingPanelPrefab, canvas.transform);
				TradingPanel.SetActive(false);

				FlexibleGridLayout t_PanelLayout = UniFunc.GetChildComponent<FlexibleGridLayout>(TradingPanel);
				if(t_PanelLayout != null)
				{
					List<Button> t_Buttons = UniFunc.GetChildsComponent<Button>(t_PanelLayout.gameObject);
					if(t_Buttons != null)
					{
						for (int i = 0; i < t_Buttons.Count; i = i + 1)
						{
							Image t_Image = UniFunc.GetChildComponent<Image>(t_Buttons[i].gameObject);
							TextMeshProUGUI t_Text = t_Image != null ? UniFunc.GetChildComponent<TextMeshProUGUI>(t_Image.gameObject) : null;

							selectedItems.Add(new SelectedItem(i, 0));
							int value = i;
							t_Buttons[i].onClick.AddListener(() =>
							{
								SelectedItem t_SelectItem = selectedItems[value];
								if (Input.GetMouseButtonDown(0) == true || Input.GetMouseButton(0) == true || Input.GetMouseButtonUp(0) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount + 1;
								}
								if (Input.GetMouseButtonDown(1) == true || Input.GetMouseButton(1) == true || Input.GetMouseButtonUp(1) == true)
								{
									t_SelectItem.itemAmount = t_SelectItem.itemAmount - 1;
									if (t_SelectItem.itemAmount < 0)
									{
										t_SelectItem.itemAmount = 0;
									}
								}

								if (t_Image != null)
								{
									if (t_SelectItem.itemAmount > 0)
									{
										if (t_Image.gameObject.activeSelf == false)
										{
											t_Image.gameObject.SetActive(true);
										}
										if (t_Text != null)
										{
											t_Text.text = t_SelectItem.itemAmount + "";
										}
									}
									else if (t_SelectItem.itemAmount <= 0)
									{
										if (t_Image.gameObject.activeSelf == true)
										{
											t_Image.gameObject.SetActive(false);
										}
									}
								}

								selectedItems[value] = t_SelectItem;
							});
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

		if (Input.GetMouseButtonDown(0) == true)
		{
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
			{
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = Camera.main.farClipPlane;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

				RaycastHit hit;
				if (Physics.Raycast(Camera.main.transform.position, mousePosition, out hit, Mathf.Infinity) == true)
				{
					if (hit.transform.gameObject.GetComponent<NonPlayerCharacter>() != null)
					{
						
					}
				}
			}
		}
	}
}
