using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFSM<T> where T : class
{
    T entity;
    State<T> curState;
    State<T> prevState;

    public void InitNPC(T entitiy, State<T> state)
    {
        this.entity = entitiy;
        curState = state;
        ChangeState(state);
    }

    public void ChangeState(State<T> newState)
    {
        prevState = curState;
        if(curState != null)
            curState.Exit(entity);
        curState = newState;
        curState.Enter(entity);
    }

    public void StateUpdate()
    {
        if (curState != null)
        {
            curState.Execute(entity);
            curState.OnTransition(entity);
        }
    }

    public State<T> GetPrevState()
    {
        return prevState;
    }
}
