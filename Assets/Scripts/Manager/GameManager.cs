using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public readonly Transform Player;
    [SerializeField]
    Transform spawnPos;

    [Header("DB")]
    [SerializeField]
    DataBase dataBase;
    [SerializeField]
    DataBase_Character characterDB;

    [Header("Item")]
    [SerializeField]
    AddressableManager addressableManager;
    [SerializeField]
    ItemManager itemManager;

    [Header("Time")]
    [SerializeField]
    GameTime gameTime;

    [Header("Behaviour")]
    [SerializeField]
    BehaviourMaster behaviourMaster;

    [Header("Location")]
    [SerializeField]
    LocationManager locationManager;

    [Header("GameEvent")]
    [SerializeField]
    GameEventManager gameEventManager;

    [SerializeField]
    Dialogue dialogue;


    public DataBase DataBase { get { return dataBase; } }
    public DataBase_Character CharacterDB { get { return characterDB; } }
    public GameTime GameTime { get { return gameTime; } }
    public AddressableManager AddressableManager { get { return addressableManager; } }
    public ItemManager ItemManager { get { return itemManager; } }
    public BehaviourMaster BehaviourMaster { get { return behaviourMaster; } }
    public LocationManager LocationManager { get { return locationManager; } }
    public Dialogue Dialogue { get { return dialogue; } }
    public GameEventManager GameEventManager { get { return gameEventManager; } }

    [SerializeField]
    Transform characters;

    private void Start()
    {
  /*      EventManager.Subscribe(EventType.Enter, EnterShop);*/
        EventManager.Subscribe(EventType.Exit, ExitShop);
    }

    public void EnterShop()
    {
        for(int i = 1; i <= characterDB.GetCharacterCount(); i++)
        {
            characterDB.GetCharacter(i).gameObject.SetActive(false);
        }
    }

    public void ExitShop()
    {

    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos.position;
    }
}

public class Tools
{
    public static Transform GetPlayerTransform()
    {
        return GameManager.Instance.Player;
    }

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

    public static float FloatParse(object data)
    {
        return float.Parse(data.ToString());
    }
}
