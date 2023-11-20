using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Greeting, Sitting, Business, PartTimer, Interaction, Rest, Last
}

public enum NPCScheduleType
{
    Interaction = 1, Business, Rest, None, Last
}

public class NPC : Character, IInteraction
{
    CharacterFSM<NPC> fsm;
    [SerializeField]
    private State<NPC>[] states;
    [SerializeField]
    public NPCBehaviour prevBehaviour;
    [SerializeField]
    public NPCBehaviour curBehaviour;

    public NPCBehaviour PrevBehaviour { get { return prevBehaviour; } }

    public bool IsUsed { get; set; }

    public LookDir lookDir = new();

    float intimacy;

    public bool isMeet;
    public bool isTalk;
    public bool isAcquaintance;

    int formal = 0;

    public NavMeshAgent agent;
    public Vector3 destinationPos;
    public GameObject curInteractionObj;
    public GameObject targetInteractionObj;

    public GameObject player;

    [SerializeField]
    float sightRange;
    
    [SerializeField]
    NpcDialog dialogBox;

    [SerializeField]
    TimeTableData schedule;

    public string shopDialog;
    public bool isSales;

    public CharacterData CharacterData {  get { return characterData; } }

    public TimeTableData Schedule { get { return schedule; } }

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        fsm = new();
        agent = GetComponent<NavMeshAgent>();
        states = new State<NPC>[((int)NPCBehaviour.Last)];
        //dialog = GetComponentInChildren<TextMeshPro>();

        states[((int)NPCBehaviour.Idle)] = GetComponent<IdleState>();
        states[((int)NPCBehaviour.Move)] = GetComponent<MoveState>();
        states[((int)NPCBehaviour.Conversation)] = GetComponent<ConversationState>();
        states[((int)NPCBehaviour.Greeting)] = GetComponent<GreetingState>();
        states[((int)NPCBehaviour.Sitting)] = GetComponent<SittingState>();
        states[((int)NPCBehaviour.Rest)] = GetComponent<RestState>();
        states[((int)NPCBehaviour.Interaction)] = GetComponent<InteractionState>();
        states[((int)NPCBehaviour.PartTimer)] = GetComponent<PartTimeState>();
        states[((int)NPCBehaviour.Business)] = GetComponent<BusinessState>();
    }

    private void Start()
    {
        InitDay();

        EventManager.Subscribe(EventType.NextDialog, TalkEnd);
        EventManager.Subscribe(EventType.EndConversation, EndConversation);
        dialogBox.InitDialogBox(characterData.characterName);
        DontDestroyOnLoad(gameObject);

        SkeletonInitialize();
    }

    public void Init()
    {
        fsm.InitNPC(this, states[((int)NPCBehaviour.Idle)]);
    }

    public void InitObj()
    {
        player = null;
        targetInteractionObj = null;
    }

    public void ChangeAni(string aniName)
    {
        SkAni.AnimationName = aniName;
    }

    public void InitDay()
    {
        isMeet = false;
        formal = 0;
    }

    protected virtual void Update()
    {
        fsm.StateUpdate();
        myTransform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
    }

    public bool IsInSight()
    {
        if (player == null)
            return false;

        return (player.transform.position - transform.position).sqrMagnitude <= sightRange && !isMeet;
    }

    public void Greeting()
    {
        if (isMeet)
            return;

        ChangeState(NPCBehaviour.Greeting);
    }

    public void StartConversation()
    {
        formal++;
        Debug.Log(characterData.characterEgName + "와 대화를 시작합니다");
        FindPlayer();
        curInteractionObj = player;
        ChangeState(NPCBehaviour.Conversation);
        CameraEvent.Instance.onCamBlendComplate.AddListener(TalkEvent);
    }

    public void TalkEvent()
    {
        GameManager.Instance.GameTime.TimeStop(true);
        if (prevBehaviour == NPCBehaviour.Business)
            GameManager.Instance.Dialogue.InitDialogue(shopDialog, formal);
        else
            GameManager.Instance.Dialogue.InitDialogue(characterData.characterEgName + "_Text_Master", formal);
    }

    public void TalkEnd()
    {
        //GameManager.Instance.GameTime.TimeStop(false);
        dialogBox.transform.parent.gameObject.SetActive(false);
    }

    public void Talk(string talkScript)
    {
        dialogBox.SetScript(talkScript);
        dialogBox.transform.parent.gameObject.SetActive(true);
    }

    void EndConversation()
    {
        dialogBox.transform.parent.gameObject.SetActive(false);
    }

    public void SetSchedule(TimeTableData newSchedule)
    {
        bool isMove = schedule.aiParam1 != newSchedule.aiParam1;
        
        schedule = newSchedule;
        
        if(isMove)
            ChangeStateFromSchedule();
    }

    void RandomDestinationPos()
    {
        targetInteractionObj = null;
        Vector3 schedulePos = GameManager.Instance.LocationManager.GetLocationPosition(schedule.aiParam2);
        destinationPos = LocationManager.GetLocationRandomPosition(schedulePos);
    }

    void TargetDestinationPos()
    {
        destinationPos = GameManager.Instance.LocationManager.GetLocationPosition(schedule.aiParam2);
        LocationType locationType = GameManager.Instance.LocationManager.GetLocationType(schedule.aiParam2);
        targetInteractionObj = LocationManager.GetObjectFromLocation(destinationPos, locationType);
    }

    void ChangeStateFromSchedule()
    {
        switch ((NPCScheduleType)schedule.aiParam1)
        {
            case NPCScheduleType.Interaction:
                TargetDestinationPos();
                ChangeState(NPCBehaviour.Move);
                break;
            case NPCScheduleType.Business:
                if (curBehaviour == NPCBehaviour.Business)
                    break;
                TargetDestinationPos();
                ChangeState(NPCBehaviour.Move);
                break;
            case NPCScheduleType.Rest:
                TargetDestinationPos();
                ChangeState(NPCBehaviour.Move);
                break;
            case NPCScheduleType.None:
                RandomDestinationPos();
                ChangeState(NPCBehaviour.Move);
                break;
        }
    }

    public void ChangeState(NPCBehaviour newBehaviour)
    {
        if(curBehaviour != NPCBehaviour.Greeting && curBehaviour != NPCBehaviour.Conversation)
            prevBehaviour = curBehaviour;
        fsm.ChangeState(states[(int)newBehaviour]);
        curBehaviour = newBehaviour;
    }

    public void Relocation()
    {
        agent.enabled = false;
        Vector3 schedulePos = GameManager.Instance.LocationManager.GetLocationPosition(schedule.aiParam2);
        Vector3 relocationPos = LocationManager.GetLocationRandomPosition(schedulePos);
        transform.position = relocationPos;
        agent.enabled = true;

        ChangeStateFromSchedule();
    }

    public void GetRandomDestinationByMyPosition()
    {
        destinationPos = LocationManager.GetLocationRandomPosition(transform.position);
    }

    public void Interaction(GameObject user)
    {
        EventManager.Publish(EventType.StartInteraction);
        StartConversation();
    }

    public void FindPlayer()
    {
        if (player == null || player.activeSelf == false)
        {
            player = GameObject.FindObjectOfType<PlayerCharacter>().gameObject;
        }
    }

    public void LookAtTarget()
    {
        lookDir.SetDir(myTransform, curInteractionObj.transform.position);
    }

    public void Flip()
    {
        var scaleX = lookDir.isRight ? -1 : 1;
        var newScale = transform.localScale;
        newScale.x = Mathf.Abs(transform.localScale.x) * scaleX;
        transform.localScale = newScale;
    }    
}

[System.Serializable]
public class LookDir
{
    public bool isFront;
    public bool isRight;
    public bool isSideWalk;

    public void SetDir(Vector3 velocity)
    {        
        if (velocity.x >= 0)
            isRight = true;
        else
            isRight = false;

        if(velocity.z <= 0)
            isFront = true;
        else
            isFront = false;

        if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.z))
            isSideWalk = true;
        else
            isSideWalk = false;
    }
    public void SetDir(Transform myPos, Vector3 targetPos)
    {
        var pos = myPos.position;
        pos.y = 0f;
        targetPos.y = 0f;
        var dir = (targetPos - pos).normalized;
        var cross = Vector3.Cross(myPos.transform.forward, dir);
        var dot = Vector3.Dot(myPos.transform.forward, dir);
        
        isRight = cross.y >= 0;
        isFront = dot <= 0;

        isSideWalk = false;
    }
}