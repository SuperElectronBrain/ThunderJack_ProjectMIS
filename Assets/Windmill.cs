using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Windmill Data", menuName = "Scriptable Object/Windmill Data", order = int.MaxValue)]
public class Windmill : ScriptableObject
{
    [Range(0f, 1000f)]
    public float smallSpinSpeed;
    [Range(0f, 1000f)]
    public float bigSpinSpeed;
}
