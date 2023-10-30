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
        Debug.Log(gameObject.name + " ��ȣ�ۿ�");
        if (isPlanted)
            return;

        if (user.GetComponent<PlayerCharacter>())
        {
            // ������ �ִ��� ������ �˻�

            meshRenderer.enabled = true;
            flower.Plant();
            boxCollider.enabled = false;   

            isPlanted = true;
        }        
    }

    public void Harvesting()
    {
        Debug.Log("��Ȯ");
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
