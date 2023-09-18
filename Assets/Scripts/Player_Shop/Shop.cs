using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Publish(EventType.Work);
    }

    private void OnDestroy()
    {
        EventManager.Publish(EventType.Exit);
        EventManager.Publish(EventType.Work);
    }
}
