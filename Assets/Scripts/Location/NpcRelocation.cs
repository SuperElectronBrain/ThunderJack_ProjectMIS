using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRelocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if(GameManager.Instance.CharacterDB.GetCharacterCount() != 0)
        {
            Invoke("Relocation", 1f);
            //Relocation();
        }
    }

    void Relocation()
    {
        for(int i = 1; i < GameManager.Instance.CharacterDB.GetCharacterCount(); i++)
        {
            var npc = GameManager.Instance.CharacterDB.GetNPC(i);
            npc.gameObject.SetActive(true);
            npc.Relocation();
        }
    }
}
