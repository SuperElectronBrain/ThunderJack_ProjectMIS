using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
	// Start is called before the first frame update
	protected virtual void Start()
    {
		GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(OnButtonClick);
		GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnButtonClick);
    }

	// Update is called once per frame
	//protected virtual void Update()
	//{
	//    
	//}

	public virtual void OnButtonClick()
	{

	}
}
