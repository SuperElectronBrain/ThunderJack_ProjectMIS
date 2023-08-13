using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CEOSystem : MonoBehaviour
{
	[SerializeField] private GameObject EmployPanelPrefab;
	private GameObject EmployPanel;
	[SerializeField] private GameObject partTimerPanelPrefab;
	private GameObject partTimerPanel;
	private List<PartTimer> partTimers = new List<PartTimer>();
	private PlayerCharacter playerCharacter;
	private Character focusingTarget = null;

	// Start is called before the first frame update
	void Start()
	{
		playerCharacter = gameObject.GetComponent<PlayerCharacter>();

		Canvas canvas = FindObjectOfType<Canvas>();
		if (canvas != null)
		{
			if (partTimerPanelPrefab != null)
			{
				partTimerPanel = Instantiate(partTimerPanelPrefab, canvas.transform);
				partTimerPanel.SetActive(false);

				GameObject t_PartTimerDisplay = UniFunc.GetChildOfName(partTimerPanel, "PartTimerDisplay");
				if(t_PartTimerDisplay != null)
				{
					GameObject t_GameObject = UniFunc.GetChildOfName(t_PartTimerDisplay, "KickButton");
					if(t_GameObject != null)
					{
						Button t_Button = t_GameObject.GetComponent<Button>();
						if(t_Button != null)
						{
							t_Button.onClick.AddListener(() => 
							{
								RemovePartTimer(partTimers[0]);

								if (partTimerPanel.activeSelf == true)
								{
									partTimerPanel.SetActive(false);
								}
							});
						}
					}

					t_GameObject = UniFunc.GetChildOfName(t_PartTimerDisplay, "StartButton");
					if (t_GameObject != null)
					{
						Button t_Button = t_GameObject.GetComponent<Button>();
						if (t_Button != null)
						{
							t_GameObject = UniFunc.GetChildOfName(t_PartTimerDisplay, "Dropdown");
							if(t_GameObject != null)
							{
								TMP_Dropdown t_Dropdown = t_GameObject.GetComponent<TMP_Dropdown>();
								if(t_Dropdown != null)
								{
									t_Button.onClick.AddListener(() =>
									{
										partTimers[0].productionItem = (ItemCode)System.Enum.Parse(typeof(ItemCode), t_Dropdown.options[t_Dropdown.value].text);

										partTimers[0].StartProduction();
									});
								}
							}
						}
					}

					t_GameObject = UniFunc.GetChildOfName(t_PartTimerDisplay, "EndButton");
					if (t_GameObject != null)
					{
						Button t_Button = t_GameObject.GetComponent<Button>();
						if (t_Button != null)
						{
							t_Button.onClick.AddListener(() =>
							{
								partTimers[0].EndProduction();
							});
						}
					}
				}
			}

			if (EmployPanelPrefab != null)
			{
				EmployPanel = Instantiate(EmployPanelPrefab, canvas.transform);
				EmployPanel.SetActive(false);

				List<Button> t_Buttons = UniFunc.GetChildsComponent<Button>(EmployPanel);
				if (t_Buttons != null)
				{
					for (int i = 0; i < t_Buttons.Count; i = i + 1)
					{
						if (t_Buttons[i].name == "EmployButton")
						{
							t_Buttons[i].onClick.AddListener(() => 
							{
								AddPartTimer(focusingTarget);

								if (EmployPanel.activeSelf == true)
								{
									EmployPanel.SetActive(false);
								}

								if (partTimerPanel.activeSelf == false)
								{
									partTimerPanel.SetActive(true);
								}
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
				if (EmployPanel.activeSelf == true)
				{
					EmployPanel.SetActive(false);
				}

				if (partTimerPanel.activeSelf == true)
				{
					partTimerPanel.SetActive(false);
				}

				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = Camera.main.farClipPlane;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

				RaycastHit hit;
				if (Physics.Raycast(Camera.main.transform.position, mousePosition, out hit, Mathf.Infinity) == true)
				{
					if (hit.transform.gameObject.GetComponent<NonPlayerCharacter>() != null)
					{
						if (EmployPanel.activeSelf == false)
						{
							EmployPanel.SetActive(true);
						}
						focusingTarget = hit.transform.gameObject.GetComponent<NonPlayerCharacter>();

						PartTimer t_PartTimer = hit.transform.gameObject.GetComponent<PartTimer>();
						if (t_PartTimer != null)
						{
							for (int i = 0; i < partTimers.Count; i = i + 1)
							{
								if (partTimers[i] == t_PartTimer)
								{
									if (EmployPanel.activeSelf == true)
									{
										EmployPanel.SetActive(false);
									}

									if (partTimerPanel.activeSelf == false)
									{
										partTimerPanel.SetActive(true);
									}

									break;
								}
							}
						}
					}
				}
			}
		}
	}

	public void Employ()
	{

	}

	public void AddPartTimer(PartTimer p_PartTimer)
	{
		int count = 0;
		for (int i = 0; i < partTimers.Count; i = i + 1)
		{
			if (partTimers[i] == p_PartTimer)
			{
				count = count + 1;
				break;
			}
		}

		if(count < 1)
		{
			partTimers.Add(p_PartTimer);
			if(playerCharacter != null)
			{
				p_PartTimer.SetMaster(playerCharacter);

				GameObject t_PartTimerDisplay = UniFunc.GetChildOfName(partTimerPanel, "PartTimerDisplay");
				if (t_PartTimerDisplay != null)
				{
					GameObject t_GameObject = UniFunc.GetChildOfName(t_PartTimerDisplay, "TimeDisplayText");
					if (t_GameObject != null)
					{
						TextMeshProUGUI t_TMP = t_GameObject.GetComponent<TextMeshProUGUI>();
						if (t_TMP != null)
						{
							p_PartTimer.m_TMP = t_TMP;
						}
					}
				}
			}
		}
	}
	public void AddPartTimer(Character p_Character)
	{
		PartTimer t_PartTimer = p_Character.GetComponent<PartTimer>();
		if (t_PartTimer == null) 
		{
			t_PartTimer = p_Character.AddComponent<PartTimer>();
		}
		AddPartTimer(t_PartTimer);
	}

	public void RemovePartTimer(PartTimer p_PartTimer)
	{
		if (p_PartTimer != null)
		{
			for (int i = 0; i < partTimers.Count; i = i + 1)
			{
				if (partTimers[i] == p_PartTimer)
				{
					partTimers.RemoveAt(i);
					p_PartTimer.m_TMP = null;
					p_PartTimer.SetMaster(null);
					Destroy(p_PartTimer);
					break;
				}
			}
		}
	}

	public void RemovePartTimer(Character p_Character) 
	{
		RemovePartTimer(p_Character.GetComponent<PartTimer>());
	}
}
