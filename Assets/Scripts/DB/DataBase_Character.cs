using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterCode
{
    Player, Younghoon, DisgustingKim
}
public class DataBase_Character : MonoBehaviour
{
    public Dictionary<int, CharacterData> characterDB = new();

    public GameObject npcPrefab;
    public GameObject playerPrefab;

    [SerializeField]
    string parsingData;
    [SerializeField]
    string characterId;
    [SerializeField]
    string characterName;
    [SerializeField]
    string characterEgName;

    // Start is called before the first frame update
    void Start()
    {
        var cList = GameManager.Instance.DataBase.Parser(parsingData);

        foreach (var character in cList)
        {
            int charId = int.Parse(character[characterId].ToString());

            GameObject characterPrefab = npcPrefab;

            if (charId == 0)
                characterPrefab = playerPrefab;

            var c = Instantiate(characterPrefab, GameManager.Instance.GetSpawnPos(), Quaternion.identity).GetComponent<Character>();
           
            characterDB.Add(charId, new CharacterData
            {
                characterName = character[characterName].ToString(),
                characterEgName = character[characterEgName].ToString(),
                character = c
            }
            );
            c.SetCharacterData(characterDB[charId]);
            c.name = GetCharacterName(charId);
        }
    }

    public string GetCharacterName(int characterID)
    {
        return characterDB[characterID].characterName;
    }

    public string GetCharacterEgName(int characterID)
    {
        return characterDB[characterID].characterEgName;
    }

    public Character GetCharacter(int characterID)
    {
        return characterDB[characterID].character;
    }

    /// <summary>
    /// null Check ÈÄ »ç¿ë
    /// </summary>
    /// <param name="charcterID"></param>
    /// <returns></returns>
    public NPC GetNPC(int charcterID)
    {
        NPC npc = null;

        npc = (characterDB[charcterID].character as NPC);

        return npc;

    }

    public int GetCharacterCount()
    {
        return characterDB.Count;
    }
}

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public string characterEgName;
    public Character character;
}
