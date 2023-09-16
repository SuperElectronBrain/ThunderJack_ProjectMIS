using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterCode
{
    Player, Younghoon, DisgustingKim
}
public class DataBase_Character : MonoBehaviour
{
    public List<CharacterData> characterDB = new();

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

            Character newCharacter = null;

            if (charId == 0)
                continue;
            newCharacter = Instantiate(npcPrefab, GameManager.Instance.GetSpawnPos(), Quaternion.identity).GetComponent<Character>();
           
            characterDB.Add(new CharacterData
            {
                characterName = character[characterName].ToString(),
                characterEgName = character[characterEgName].ToString(),
                character = newCharacter
            }
            );
            newCharacter.SetCharacterData(characterDB[charId - 1]);
            newCharacter.gameObject.name = GetCharacterName(charId);

            newCharacter.InitCharacter("Cat");
        }
    }

    public string GetCharacterName(int characterID)
    {
        return characterDB[characterID - 1].characterName;
    }

    public string GetCharacterEgName(int characterID)
    {
        return characterDB[characterID - 1].characterEgName;
    }

    public Character GetCharacter(int characterID)
    {
        return characterDB[characterID - 1].character;
    }

    /// <summary>
    /// null Check ÈÄ »ç¿ë
    /// </summary>
    /// <param name="charcterID"></param>
    /// <returns></returns>
    public NPC GetNPC(int charcterID)
    {
        NPC npc = null;

        npc = (characterDB[charcterID - 1].character as NPC);

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
