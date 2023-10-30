using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : MonoBehaviour, IInteraction
{
    bool isPlanted;
    public bool IsUsed { get; set; }

    MeshRenderer meshRenderer;
    BoxCollider boxCollider;
    [SerializeField]
    Flower flower;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer.enabled = false;
    }

    public void Init()
    {
        isPlanted = true;
        meshRenderer.enabled = true;
    }

    public void Interaction(GameObject user)
    {
        Debug.Log(gameObject.name + " 상호작용");
        if (isPlanted)
            return;

        if (user.GetComponent<PlayerCharacter>())
        {
            // 씨앗이 있는지 없는지 검사

            meshRenderer.enabled = true;
            flower.Plant();
            boxCollider.enabled = false;   

            isPlanted = true;
        }        
    }

    public void Harvesting()
    {
        Debug.Log("수확");
        gameObject.SetActive(false);
        isPlanted = false;
    }

    public void SaveFlowerData()
    {
        if(isPlanted)
        {
            flower.SaveObjectData();
        }
    }

    public void LoadFlowerData()
    {
        flower.LoadObjectData();
    }
}
