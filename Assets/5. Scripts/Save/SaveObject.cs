using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveObject : MonoBehaviour
{
    [SerializeField]
    protected string key;
    public abstract void SaveObjectData();
    public abstract void LoadObjectData();
    public abstract void DeleteObjectData();
}
