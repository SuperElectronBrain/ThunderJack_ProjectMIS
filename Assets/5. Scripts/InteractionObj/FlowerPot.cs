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
        Debug.Log(gameObject.name + " ��ȣ�ۿ�");
        if (isPlanted)
            return;

        if (user.GetComponent<PlayerCharacter>())
        {
            // ������ �ִ��� ������ �˻�

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
