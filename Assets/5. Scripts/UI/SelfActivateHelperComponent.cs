using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfActivateHelperComponent : MonoBehaviour
{
    [HideInInspector] public GameObject m_TargetGO;
    // Start is called before the first frame update
    void Start()
    {
        if(m_TargetGO != null)
        {
			m_TargetGO.gameObject.SetActive(true);
		}
		Destroy(gameObject);
	}
}
