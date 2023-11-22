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
            Invoke("Relocation", 0.5f);
        }
    }

    void Relocation()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var cDB = GameManager.Instance.CharacterDB;
        for(int i = 1; i <= GameManager.Instance.CharacterDB.GetCharacterCount(); i++)
        {
            var npc = cDB.GetNPC(i);
            npc.Relocation();
            npc.player = player;
        }
    }
}
