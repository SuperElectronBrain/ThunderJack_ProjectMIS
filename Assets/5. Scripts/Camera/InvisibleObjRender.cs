using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObjRender : MonoBehaviour
{
    Collider[] invisibleObjs;
    Transform playerTransform;
    
    [SerializeField]
    float boxHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerPos = transform.position + (playerTransform.position - transform.position);
        //invisibleObjs = Physics.OverlapBox()
    }

    private void OnDrawGizmos()
    {
        if (playerTransform == null)
            return;
        Vector3 centerPos = transform.position + (playerTransform.position - transform.position) / 2;
        float boxSize = (playerTransform.position - transform.position).sqrMagnitude;

        Debug.Log(boxSize);

        Gizmos.DrawWireCube(centerPos, new Vector3(boxSize, boxHeight, boxSize));
    }
}
