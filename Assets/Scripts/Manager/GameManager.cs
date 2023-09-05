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

    [SerializeField]
    Dialogue dialogue;

    [SerializeField]
    NPC npc;

    public DataBase DataBase { get { return dataBase; } }
    public DataBase_Character CharacterDB { get { return characterDB; } }
    public GameTime GameTime { get { return gameTime; } }
    public AddressableManager AddressableManager { get { return addressableManager; } }
    public ItemManager ItemManager { get { return itemManager; } }
    public BehaviourMaster BehaviourMaster { get { return behaviourMaster; } }
    public LocationManager LocationManager { get { return locationManager; } }
    public Dialogue Dialogue { get { return dialogue; } }

    private void Start()
    {
        
    }    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            npc.StartConversation();
        }
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
