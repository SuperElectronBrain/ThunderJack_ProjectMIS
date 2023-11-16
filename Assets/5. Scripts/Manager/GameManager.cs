using System;
using System.Collections;
using System.Collections.Generic;
using RavenCraftCore;
using UnityEngine;

public enum SceneType
{
    OutSide, InSide, Bussiness
}

public class GameManager : Singleton<GameManager>
{
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

    [SerializeField] private NpcRequestManager npcRequestManager;

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
    public NpcRequestManager NpcRequestManager => npcRequestManager;

    [SerializeField]
    Transform characters;
    [SerializeField]
    DebugCommand debugPanel;

    public SceneType prevScene;
    public SceneType curScene;    

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        EventManager.Subscribe(EventType.Enter, EnterShop);
    }


    [SerializeField] private float perfection;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            EventManager.Publish(EventType.Day);
        else if (Input.GetKeyDown(KeyCode.F3))
            debugPanel.ActiveUI();
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
                ExitShop();
                if(gameTime.IsNextDay)
                    gameTime.NewDay();
                gameTime.TimeStop(false);
                break;
            case SceneType.InSide:
                gameTime.TimeStop(true);
                isWork = true;
                break;
            case SceneType.Bussiness:
                gameTime.TimeStop(false);
                break;
        }
    }

    public void ExitShop()
    {
        if (!isWork)
            return;
        isWork = false;

        for (int i = 1; i <= characterDB.GetCharacterCount(); i++)
        {
            characterDB.GetNPC(i).Init();
            characterDB.GetNPC(i).FindPlayer();
        }
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

    public static float FloatParse(object data)
    {
        return float.Parse(data.ToString());
    }
    
    public static Enum EnumParse<T>(object data) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), data.ToString());
    }
}

/*Todo
 
 ���� �̸� �߰��ϴ°�
 �ֹ� ���� ���� 50%
 ��� Fade ���� ����
 ���� 80%
 
 ���� ���� �ֹ� ��� ����
 
 ��ī����
 
 �ֹ� ����Ʈ 50%
 
 ����� Plate���� �κ��丮�� ���� �� �̺�Ʈ
 ��ű�(N) -> �κ��丮 = ����� Plate �ʱ�ȭ �κ��丮 ī��Ʈ
 
 -------------�Ϸ�-------------
 �׷� ������ �߰� 
 �λ��̵� �� ���� �ð� ���߱�
 �մ� ���̾�α� ����
 ���� �ϼ��� �ý��� �߰� 
 �ǽð� ������ 0.5 
 �Ϸ� ����
 */
