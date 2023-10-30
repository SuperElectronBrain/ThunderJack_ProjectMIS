using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flower : SaveObject, IInteraction
{
    public bool IsUsed { get; set; }
    bool isGrow;
    [SerializeField]
    int growDay = 1;

    [SerializeField]
    GameObject sprout;
    [SerializeField]
    GameObject flower;    

    void Awake()
    {
        key = transform.parent.name + "_GrowDay" + GameManager.Instance.GameTime.GetDay();
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Subscribe(EventType.Day, Grow);        
    }

    void Init()
    {
        if(growDay == 0)
        {
            isGrow = true;
            
            transform.parent.GetComponentInChildren<FlowerPot>().Init();
            gameObject.SetActive(true);
            sprout.SetActive(false);
            flower.SetActive(true);
        }
        else if(growDay > 0)
        {
            isGrow = true;

            transform.parent.GetComponentInChildren<FlowerPot>().Init();
            gameObject. SetActive(true);
            sprout.SetActive(true);
            flower.SetActive(false);
        }
        else
        {
            isGrow = false;

            sprout.SetActive(false);
            flower.SetActive(false);
        }
    }

    public void Plant()
    {
        growDay = 1;
        gameObject.SetActive(true);
    }

    public void Grow()
    {
        growDay--;

        Init();
    }

    public void Interaction(GameObject user)
    {
        Debug.Log(gameObject.name + " 상호작용");
        if (!isGrow)
            return;

        if(user.GetComponent<PlayerCharacter>())
        {
            transform.parent.GetComponentInChildren<FlowerPot>().Harvesting();
            isGrow = false;
            gameObject.SetActive(false);
            sprout.SetActive(true);
            flower.SetActive(false);
        }
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe(EventType.Day, Grow);
    }

    public override void SaveObjectData()
    {
        PlayerPrefs.SetInt(key, growDay);
        Debug.Log(transform.parent.name + "Save GrowDay : " + growDay);
    }

    public override void LoadObjectData()
    {
        if (!PlayerPrefs.HasKey(key))
            return;

        growDay = PlayerPrefs.GetInt(key);
        Debug.Log(transform.parent.name + "Load GrowDay : " + growDay);
        Init();
    }

    public override void DeleteObjectData()
    {

    }
}