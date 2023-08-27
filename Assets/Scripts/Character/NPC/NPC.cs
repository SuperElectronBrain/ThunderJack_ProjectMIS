using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCBehaviour
{
    Idle, Move, Conversation, Sleep
}

public class NPC : Character
{
    CharacterFSM<NPC> fsm;
    [SerializeField]
    private State<NPC>[] states;

    NPC_Move npcMove;

    private void Start()
    {
        npcMove = GetComponent<NPC_Move>();
    }

    protected virtual void Update()
    {

    }
}
