using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public void Start()
    {
        EventManager.Subscribe(EventType.Save, SaveGardenData);
        EventManager.Subscribe(EventType.Load, LoadGardenData);
    }

    void SaveGardenData()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetComponentInChildren<FlowerPot>().SaveFlowerData();
        }
    }

    void LoadGardenData()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetComponentInChildren<FlowerPot>().LoadFlowerData();
        }
    }
}
