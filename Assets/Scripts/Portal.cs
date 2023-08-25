using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField] private Transform destination;

	// Start is called before the first frame update
	void Start()
	{
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		if (gameObject.GetComponent<Renderer>() == true)
		{
			gameObject.GetComponent<Renderer>().enabled = false;
		}
	}

	// Update is called once per frame
	//void Update()
	//{
	//    
	//}

	private void OnTriggerEnter(Collider other)
	{
		if(destination != null)
		{
			if (other.gameObject != this.gameObject)
			{
				if (other.gameObject.isStatic == false)
				{
					if (other.gameObject.GetComponent<Rigidbody>() != null)
					{
						if(other.gameObject.GetComponent<PortalMarker>() == null)
						{
							other.gameObject.AddComponent<PortalMarker>();
							other.gameObject.transform.position = destination.position;
						}
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != this.gameObject)
		{
			if (other.gameObject.GetComponent<PortalMarker>() != null)
			{
				Destroy(other.gameObject.GetComponent<PortalMarker>());
			}
		}
	}
}
