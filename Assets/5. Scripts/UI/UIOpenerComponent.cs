using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpenerComponent : MonoBehaviour
{
    [SerializeField] private GameObject m_TargetUI;
    [SerializeField] private KeyCode m_Key = KeyCode.Escape;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(m_Key) == true)
        {
			if(CloseableUICounter.GetCloseableUICounter().GetRecentlyOpenedUI() == null)
			{
				if (m_TargetUI != null)
				{
					if (m_TargetUI.activeSelf == false)
					{
						m_TargetUI.SetActive(true);
					}
				}
			}
		}
    }
}
