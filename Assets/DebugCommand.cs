using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommand : UiComponent
{
    [SerializeField]
    Vector3 townPos;

    public void MoveTown(GameObject go)
    {
        go.transform.position = townPos;
    }

    public void AddBasicItem()
    {
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(1,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(2,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(3,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(4,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(5,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(22,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(23,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(24,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(25,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(26,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(27,1,5);
    }
}
