using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterCode
{
    Player, Younghoon, DisgustingKim
}
public class DataBase_Character : MonoBehaviour
{
    public Dictionary<int, string> characterNameDB = new();
    public Dictionary<int, Character> characterDB = new();

    public GameObject npcPrefab;
    public GameObject playerPrefab;

    [SerializeField]
    string parsingData;
    [SerializeField]
    string characterId;
    [SerializeField]
    string characterName;

    // Start is called before the first frame update
    void Start()
    {
        var cList = DataParser.dataParser(parsingData);

        foreach (var character in cList)
        {
            int charId = int.Parse(character[characterId].ToString());
            characterNameDB.Add(charId, character[characterName].ToString());

            GameObject characterPrefab = npcPrefab;

            if (charId == 0)
                characterPrefab = playerPrefab;

            var c = Instantiate(characterPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Character>();

            characterDB.Add(charId, c);
        }
    }

    public string GetCharacterName(int characterID)
    {
        return characterNameDB[characterID];
    }

    public Character GetCharacter(int characterID)
    {
        return characterDB[characterID];
    }

    public int GetCharacterCount()
    {
        return characterDB.Count;
    }
}
