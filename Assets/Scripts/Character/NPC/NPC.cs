using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Sleep, Last
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

    [SerializeField]
    BehaviourData curBehaviourData;    

    public LookDir lookDir = new();

    float intimacy;

    bool isMeet;
    bool isNear;

    NPC_Move npcMove;

    public NavMeshAgent agent;
    public Vector3 destinationPos;
    public GameObject curInteractionObj;

    protected override void Start()
    {
        base.Start();
        fsm = new();        
        agent = GetComponent<NavMeshAgent>();
        states = new State<NPC>[((int)NPCBehaviour.Last) - 1];

        states[((int)NPCBehaviour.Idle)] = GetComponent<IdleState>();
        states[((int)NPCBehaviour.Move)] = GetComponent<MoveState>();
        states[((int)NPCBehaviour.Conversation)] = GetComponent<ConversationState>();

        fsm.InitNPC(this, states[((int)NPCBehaviour.Idle)]);

        InitDay();        
    }

    public void InitDay()
    {
        isMeet = false;
    }

    protected virtual void Update()
    {
        fsm.StateUpdate();
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            curInteractionObj = gameObject;
            StartConversation();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeState(NPCBehaviour.Move);
        }

    }

    public void StartConversation()
    {
        Debug.Log(characterData.characterEgName + "와 대화를 시작합니다");
        GameManager.Instance.Dialogue.InitDialogue(characterData.characterEgName + "_Dialogue");
        
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