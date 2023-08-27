using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    CameraMove cameraMove;
    [SerializeField]
    Transform Player;
    [SerializeField]
    Transform spawnPos;

    [SerializeField]
    DataBase_Character characterDB;
    [SerializeField]
    Dialogue dialogue;

    public void ChangeCameraView(CameraView newCameraView, CameraSetup newSetup)
    {
        cameraMove.ChangeCameraView(newCameraView, newSetup);
    }    

    public Transform GetPlayerTransform()
    {
        return Player;
    }

    public bool isTimePasses;

    public void StartConversation(string loadDialogue, Character npc)
    {
        dialogue.InitDialogue(loadDialogue);
    }

    public string GetCharacterName(int characterId)
    {
        return characterDB.GetCharacterName(characterId);
    }

    public Character GetCharacter(int characterId)
    {
        return characterDB.GetCharacter(characterId);
    }

    public int GetCharacterCount()
    {
        return characterDB.GetCharacterCount();
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos.position;
    }
}

public class Tools
{
    public static float GetDistance(Transform myObj, Transform targetObj)
    {
        Vector3 myVector = myObj.position;
        Vector3 targetVector = targetObj.position;

        return Vector2.Distance(new Vector2(myVector.x, myVector.z), new Vector2(targetVector.x, targetVector.z));
    }

    public static int IntParse(object data)
    {
        return int.Parse(data.ToString());
    }
}
