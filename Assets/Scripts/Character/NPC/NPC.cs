using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool isMeet;
    bool isNear;

    NPC_Move npcMove;

    private void Start()
    {
        fsm = new();
        npcMove = GetComponent<NPC_Move>();
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

    public void SetCurBehaviourData(BehaviourData newBehaviourData)
    {
        curBehaviourData = newBehaviourData;
    }

    public void ChangeState(NPCBehaviour newBehaviour)
    {
        prevBehaviour = curBehaviour;        
        fsm.ChangeState(states[(int)newBehaviour]);
        curBehaviour = newBehaviour;
    }
}
