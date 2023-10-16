using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{    
    [SerializeField]
    Windmill windmill;

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(1).Rotate(0f, 0f, windmill.smallSpinSpeed * Time.deltaTime, Space.Self);
        transform.GetChild(2).Rotate(0f, 0f, windmill.bigSpinSpeed * Time.deltaTime, Space.Self);
    }
}
