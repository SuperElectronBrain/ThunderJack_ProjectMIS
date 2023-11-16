using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Player, Normal, Merchant, PartTimer
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

    [SerializeField]
    int maxCharacter;

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

            var characterData = new CharacterData
            {
                characterName = character[characterName].ToString(),
                characterEgName = character[characterEgName].ToString(),
                characterType = (CharacterType)(Tools.IntParse(character["Character_Type"])),
                character = newCharacter,
                spawnPoint = new Vector3(Tools.FloatParse(character["Spawn_Location_1"]), Tools.FloatParse(character["Spawn_Location_2"]), Tools.FloatParse(character["Spawn_Location_3"]))
            };

            newCharacter = Instantiate(npcPrefab, characterData.spawnPoint, Quaternion.identity).GetComponent<Character>();
            characterData.character = newCharacter;

            characterDB.Add(characterData);            

            newCharacter.SetCharacterData(characterDB[characterDB.Count - 1]);
            newCharacter.gameObject.name = characterData.characterName;

            if(characterData.characterType == CharacterType.Merchant)
            {
                newCharacter.gameObject.AddComponent<NPCShop>().enabled = false;
                newCharacter.gameObject.AddComponent<Inventory>();
            }

            newCharacter.InitCharacter(characterData.characterEgName);
            ((NPC)newCharacter).Init();
            if (charId == maxCharacter)
                break;
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
    public CharacterType characterType;
    public Character character;
    public Vector3 spawnPoint;
}
