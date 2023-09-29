using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    public bool IsUsed { get; set; }

    public void Interaction(GameObject user);
}

