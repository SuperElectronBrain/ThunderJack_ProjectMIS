using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if (GameManager.Instance != null)
		{
			if(GameManager.Instance.GameTime != null)
			{
				if (GameManager.Instance.GameTime.enabled == true)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}
}
