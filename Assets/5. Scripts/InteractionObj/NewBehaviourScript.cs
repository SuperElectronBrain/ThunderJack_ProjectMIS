using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField, Range(0, 1f)]
    private float mScale;

    Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = originPos;
        newPos.y -= mScale;
        transform.position = newPos;
    }
}
