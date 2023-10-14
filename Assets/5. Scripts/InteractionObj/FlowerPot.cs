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
    GameObject flower;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer.enabled = false;
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
            flower.SetActive(true);
            boxCollider.enabled = false;   

            isPlanted = true;
        }

        Invoke("EndInteraction", 0.1f);
    }

    void EndInteraction()
    {
        EventManager.Publish(EventType.EndIteraction);
    }
}
