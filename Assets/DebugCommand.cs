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
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(55,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(56,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(57,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(58,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(59,1,5);
        FindAnyObjectByType<PlayerCharacter>().m_Inventory.AddAItem(60,1,5);
    }
}
