using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Greeting, Sitting, Business, Interaction, Rest, Last
}

public enum NPCBehaviourType
{
    None, Business, Interaction, Rest, Last
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

    public GameObject player;

    [SerializeField]
    float sightRange;
    
    [SerializeField]
    TextMeshPro dialog;

    [SerializeField]
    TimeTableData schedule;

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
    }

    private void Start()
    {
        InitDay();

        EventManager.Subscribe(EventType.NextDialog, TalkEnd);
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
        Debug.Log(characterData.characterEgName + "�� ��ȭ�� �����մϴ�");
        GameManager.Instance.Dialogue.InitDialogue(characterData.characterEgName + "_Dialogue", formal);
        curInteractionObj = player;
        ChangeState(NPCBehaviour.Conversation);        
    }

    public void TalkEnd()
    {
        dialog.gameObject.SetActive(false);
    }

    public void Talk(string talkScript)
    {
        dialog.gameObject.SetActive(true);
        dialog.text = talkScript;
    }

    public void SetSchedule(TimeTableData newSchedule)
    {
        schedule = newSchedule;
    }

    void ChangeStateFromBehaviour()
    {

    }

    public void ChangeState(NPCBehaviour newBehaviour)
    {
        prevBehaviour = curBehaviour;        
        fsm.ChangeState(states[(int)newBehaviour]);
        curBehaviour = newBehaviour;
    }

    public void Relocation()
    {
        Vector3 schedulePos = GameManager.Instance.LocationManager.GetLocationPosition(schedule.aiParam1);
        Vector3 relocationPos = LocationManager.GetLocationRandomPosition(schedulePos);
        transform.position = relocationPos;
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