using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWaterBall : MonoBehaviour
{
    [SerializeField] private GameObject waterBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject Create(Vector3 createPos)
    {
        return Instantiate(waterBall, createPos, Quaternion.identity);
    }
}
