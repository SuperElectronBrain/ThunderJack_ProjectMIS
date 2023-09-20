using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mailbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		EventManager.Subscribe(EventType.Day, () => { });
	}

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}


}
