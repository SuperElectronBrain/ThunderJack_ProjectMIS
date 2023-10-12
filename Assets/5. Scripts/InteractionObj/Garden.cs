using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : IInteraction
{
    public bool IsUsed { get; set; }

    public void Interaction(GameObject user)
    {
        if (user.GetComponent<PlayerCharacter>())
        {

        }

    }
}
