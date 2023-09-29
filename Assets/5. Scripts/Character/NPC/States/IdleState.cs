using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<NPC>
{
    [SerializeField]
    int maxIdleDuration;
    bool isWait = true;
    IEnumerator waitCo;

    public override void Enter(NPC entity)
    {
        waitCo = Wait(Random.Range(0, maxIdleDuration));
        StartCoroutine(waitCo);

        if (entity.lookDir.isFront)
            entity.SkAni.AnimationName = "A_idle_F";
        else
            entity.SkAni.AnimationName = "A_idle_B";
    }

    public override void Execute(NPC entity)
    {
        
    }

    public override void Exit(NPC entity)
    {
        if(waitCo != null)
            StopCoroutine(waitCo);
    }

    public override void OnTransition(NPC entity)
    {
        if (entity.IsInSight())
        {
            entity.ChangeState(NPCBehaviour.Greeting);
        }
        if(!isWait)
        {
            entity.GetRandomDestinationByMyPosition();
            entity.ChangeState(NPCBehaviour.Move);
        }
    }

    IEnumerator Wait(float waitDuration)
    {
        isWait = true;
        yield return new WaitForSeconds(waitDuration);
        isWait = false;
    }
}