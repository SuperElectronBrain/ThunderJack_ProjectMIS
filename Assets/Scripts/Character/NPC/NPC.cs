using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Greeting, Sleep, Last
}

public class NPC : Character
{
    CharacterFSM<NPC> fsm;
    [SerializeField]
    private State<NPC>[] states;
    [SerializeField]
    NPCBehaviour prevBehaviour;
    [SerializeField]
    NPCBehaviour curBehaviour;

    public NPCBehaviour PrevBehaviour { get { return prevBehaviour; } }

    [SerializeField]
    BehaviourData curBehaviourData;    

    public LookDir lookDir = new();

    float intimacy;

    public bool isMeet;

    public NavMeshAgent agent;
    public Vector3 destinationPos;
    public GameObject curInteractionObj;

    public GameObject player;

    [SerializeField]
    float sightRange;
    
    [SerializeField]
    TextMeshPro dialog;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");        
        fsm = new();        
        agent = GetComponent<NavMeshAgent>();
        states = new State<NPC>[((int)NPCBehaviour.Last) - 1];
        //dialog = GetComponentInChildren<TextMeshPro>();

        states[((int)NPCBehaviour.Idle)] = GetComponent<IdleState>();
        states[((int)NPCBehaviour.Move)] = GetComponent<MoveState>();
        states[((int)NPCBehaviour.Conversation)] = GetComponent<ConversationState>();
        states[((int)NPCBehaviour.Greeting)] = GetComponent<GreetingState>();        
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
        Debug.Log(characterData.characterEgName + "와 대화를 시작합니다");
        GameManager.Instance.Dialogue.InitDialogue(characterData.characterEgName + "_Dialogue");
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

    public void SetCurBehaviourData(BehaviourData newBehaviourData)
    {
        curBehaviourData = newBehaviourData;

        ChangeStateFromBehaviour();
    }

    void ChangeStateFromBehaviour()
    {
        switch(curBehaviourData.actionType)
        {
            case 1:
                //PlayAnimation(((BehaviourType1)curBehaviourData).actionGoal);
                break;
            case 2:
                destinationPos = ((BehaviourType2)curBehaviourData).actionGoal;
                ChangeState(NPCBehaviour.Move);
                break;
            case 3:
                destinationPos = ((BehaviourType2)curBehaviourData).actionGoal;
                destinationPos = LocationManager.GetLocationRandomPosition(destinationPos);
                //Debug.Log(curBehaviourData.)
                //Debug.Log(((BehaviourType2)curBehaviourData).actionGoal);
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
        Vector3 relocationPos = LocationManager.GetLocationRandomPosition(((BehaviourType2)curBehaviourData).actionGoal);
        transform.position = relocationPos;
    }


    [SerializeField]
    bool isDebug;
    private void OnDrawGizmos()
    {
        if (!isDebug)
            return;
        Gizmos.color = Color.green;
            
        Gizmos.DrawWireSphere(transform.position, sightRange);
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