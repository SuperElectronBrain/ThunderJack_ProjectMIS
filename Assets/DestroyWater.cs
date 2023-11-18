using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyWater : MonoBehaviour
{
    [SerializeField] private RavenCraftCore.Press press;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Water"))
        {
            Destroy(other.gameObject);
        }
    }
}