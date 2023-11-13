using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Lamp : MonoBehaviour, IInteraction
{
    public bool IsUsed { get; set; }

    public UnityEvent morningEvent;
    public UnityEvent nightEvent;
    

    public void Interaction(GameObject user)
    {
        EventManager.Publish(EventType.StartInteraction);

        if (GameManager.Instance.GameTime.GetHour() < 9)
        {
            morningEvent?.Invoke();
            EndWork();
        }
        else
        {
            nightEvent?.Invoke();
            EndTheDay();            
        }
    }

    public void EndInteraction()
    {
        EventManager.Publish(EventType.EndIteraction);
    }

    void EndWork()
    {
        GameManager.Instance.GameTime.EndWork();
    }

    public void EndTheDay()
    {
        GameManager.Instance.GameTime.IsNextDay = true;
    }
}
