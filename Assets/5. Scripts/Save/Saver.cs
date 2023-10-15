using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            EventManager.Publish(EventType.Save);

            PlayerPrefs.Save();
        }

        else if(Input.GetKeyDown(KeyCode.B))
        {
            EventManager.Publish(EventType.Load);
        }
    }
}
