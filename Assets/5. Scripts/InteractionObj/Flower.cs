using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour, IInteraction
{
    public bool IsUsed { get; set; }
    bool isGrow;
    int growDay = 1;

    [SerializeField]
    GameObject sprout;
    [SerializeField]
    GameObject flower;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Subscribe(EventType.Day, Grow);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            EventManager.Publish(EventType.Day);
        else if (Input.GetKeyDown(KeyCode.X))
            EventManager.Publish(EventType.EndIteraction);
    }

    public void Grow()
    {
        growDay--;

        if(growDay == 0)
        {
            isGrow = true;
            //성장률에 따른 리소스 변화
            sprout.SetActive(false);
            flower.SetActive(true);
        }
    }

    public void Interaction(GameObject user)
    {
        Debug.Log(gameObject.name + " 상호작용");
        if (!isGrow)
            return;

        if(user.GetComponent<PlayerCharacter>())
        {
            
        }

        Invoke("EndInteraction", 0.1f);
    }

    void EndInteraction()
    {
        EventManager.Publish(EventType.EndIteraction);
    }
}
