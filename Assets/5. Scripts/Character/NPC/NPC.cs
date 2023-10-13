using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Greeting, Sitting, Business, Interaction, Rest, Last
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
    NPCBehaviour prevBehaviour;
    [SerializeField]
    NPCBehaviour curBehaviour;

    public NPCBehaviour PrevBehaviour { get { return prevBehaviour; } }

    public bool IsUsed { get; set; }

    public LookDir lookDir = new();

    float intimacy;

    public bool isMeet;
    public bool isTalk;

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
        states[((int)NPCBehaviour.Business)] = GetComponent<BusinessState>();
    }

    private void Start()
    {
        InitDay();

        EventManager.Subscribe(EventType.NextDialog, TalkEnd);
        dialogBox.InitDialogBox(characterData.characterName);
        DontDestroyOnLoad(gameObject);

        SkAni.Initialize(true);
    }

    public void Init()
    {
        fsm.InitNPC(this, states[((int)NPCBehaviour.Idle)]);
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
        curInteractionObj = player;
        ChangeState(NPCBehaviour.Conversation);
        CameraEvent.Instance.onCamBlendComplate.AddListener(TalkEvent);
    }

    public void TalkEvent()
    {
        GameManager.Instance.Dialogue.InitDialogue(characterData.characterEgName + "_Dialogue", formal);
    }

    public void TalkEnd()
    {
        dialogBox.gameObject.SetActive(false);
    }

    public void Talk(string talkScript)
    {
        dialogBox.gameObject.SetActive(true);
        dialogBox.SetScript(talkScript);
    }

    public void SetSchedule(TimeTableData newSchedule)
    {
        bool isMove = schedule.aiParam2 != newSchedule.aiParam2;

        schedule = newSchedule;

        /*if(isMove)
        {
            RandomDestinationPos();
            ChangeState(NPCBehaviour.Move);
        }*/
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
        StartConversation();
    }

    public void LookAtTarget()
    {
        lookDir.SetDir(transform.position, curInteractionObj.transform.position);
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
    public void SetDir(Vector3 myPos, Vector3 targetPos)
    {
        if (myPos.x <= targetPos.x)
            isRight = true;
        else
            isRight = false;

        if (myPos.z >= targetPos.z)
            isFront = true;
        else
            isFront = false;

        isSideWalk = false;
    }
}