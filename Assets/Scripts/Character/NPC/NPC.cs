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

    protected virtual void Update()
    {

    }
}
