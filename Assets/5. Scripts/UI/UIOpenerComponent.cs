using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpenerComponent : MonoBehaviour
{
	[Serializable]
	private struct UIOpenOption
	{
		public GameObject targetUI;
		public KeyCode key;
	}

	[SerializeField] private List<UIOpenOption> m_TargetUIs = new List<UIOpenOption>();

	// Update is called once per frame
	void Update()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "loading")
		{
			for (int i = 0; i < m_TargetUIs.Count; i = i + 1)
			{
				if (Input.GetKeyDown(m_TargetUIs[i].key) == true)
				{
					if (m_TargetUIs[i].key == KeyCode.Escape)
					{
						if (CloseableUICounter.GetCloseableUICounter().GetRecentlyOpenedUI() != null) { break; }
					}

					if (m_TargetUIs[i].targetUI != null)
					{
						if (m_TargetUIs[i].targetUI.activeSelf == false)
						{
							m_TargetUIs[i].targetUI.SetActive(true);
						}
					}
				}
			}
		}
	}
}
