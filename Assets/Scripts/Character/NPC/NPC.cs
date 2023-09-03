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

    float intimacy;

    bool isMeet;
    bool isNear;

    NPC_Move npcMove;

    public NavMeshAgent agent;
    public Vector3 destinationPos;

    private void Start()
    {
        fsm = new();
        agent = GetComponent<NavMeshAgent>();
        states = new State<NPC>[((int)NPCBehaviour.Last)];

        states[((int)NPCBehaviour.Idle)] = GetComponent<IdleState>();
        states[((int)NPCBehaviour.Move)] = GetComponent<MoveState>();
        states[((int)NPCBehaviour.Conversation)] = GetComponent<ConversationState>();

        InitDay();
    }

    public void InitDay()
    {
        isMeet = false;
    }

    protected virtual void Update()
    {
        fsm.StateUpdate();
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
                PlayAnimation(((BehaviourType1)curBehaviourData).actionGoal);
                break;
            case 2:
                destinationPos = ((BehaviourType2)curBehaviourData).actionGoal.position;
                ChangeState(NPCBehaviour.Move);
                break;
            case 3:
                destinationPos = ((BehaviourType2)curBehaviourData).actionGoal.position;
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
