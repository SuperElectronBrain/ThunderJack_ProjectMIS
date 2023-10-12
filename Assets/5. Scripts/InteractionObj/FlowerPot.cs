using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : MonoBehaviour, IInteraction
{
    bool isPlanted;
    public bool IsUsed { get; set; }

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void Interaction(GameObject user)
    {
        if (isPlanted)
            return;

        if (user.GetComponent<PlayerCharacter>())
        {
            // ������ �ִ��� ������ �˻�

            meshRenderer.enabled = true;

            isPlanted = true;
        }
    }
}
