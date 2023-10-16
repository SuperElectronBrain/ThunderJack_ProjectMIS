using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillStoneCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BBB material))
        {
            material.InMillstone();
        }
    }
}
