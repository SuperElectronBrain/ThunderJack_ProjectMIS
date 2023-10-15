using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    OutSide, InSide, Bussiness
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public readonly Transform Player;

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

    [Header("Quest")]
    [SerializeField]
    QuestManager questManager;

    [Header("UiManager")]
    [SerializeField]
    UI_Manager uiManager;

    [SerializeField]
    Dialogue dialogue;

    public bool isWork;

    public DataBase DataBase { get { return dataBase; } }
    public DataBase_Character CharacterDB { get { return characterDB; } }
    public GameTime GameTime { get { return gameTime; } }
    public AddressableManager AddressableManager { get { return addressableManager; } }
    public ItemManager ItemManager { get { return itemManager; } }
    public BehaviourMaster BehaviourMaster { get { return behaviourMaster; } }
    public LocationManager LocationManager { get { return locationManager; } }
    public Dialogue Dialogue { get { return dialogue; } }
    public GameEventManager GameEventManager { get { return gameEventManager; } }
    public QuestManager QuestManager { get { return questManager; } }
    public UI_Manager UIManager { get { return uiManager; } }

    [SerializeField]
    Transform characters;

    public SceneType prevScene;
    public SceneType curScene;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        EventManager.Subscribe(EventType.Enter, EnterShop);
        EventManager.Subscribe(EventType.Exit, ExitShop);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            EventManager.Publish(EventType.Day);
        else if (Input.GetKeyDown(KeyCode.X))
            EventManager.Publish(EventType.EndIteraction);
    }

    public void EnterShop()
    {
        /*for(int i = 1; i <= characterDB.GetCharacterCount(); i++)
        {
            characterDB.GetCharacter(i).transform.SetParent(null);
            characterDB.GetCharacter(i).gameObject.SetActive(false);
        }*/
        isWork = true;
        EventManager.Publish(EventType.Save);
    }

    public void InitScene(SceneType sceneType)
    {
        prevScene = curScene;
        curScene = sceneType;

        switch(sceneType)
        {
            case SceneType.OutSide:
                CameraEvent.Instance.Init();
                GameEventManager.Init();
                EventManager.Publish(EventType.Load);
                isWork = false;
                break;
            case SceneType.InSide:
                isWork = true;
                break;
            case SceneType.Bussiness:

                break;
        }
    }

    public void ExitShop()
    {
        isWork = false;
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
