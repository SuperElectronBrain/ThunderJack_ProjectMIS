using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShop : MonoBehaviour
{
	[SerializeField] List<Need> salesItems = new List<Need>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public List<Need> GetSalesItems()
	{
		return new List<Need>(salesItems);
	}
}
