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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("InvisibleObj"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InvisibleObj"))
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerTransform == null)
            return;
        Vector3 centerPos = transform.position + (playerTransform.position - transform.position) / 2;
        float boxSize = Vector3.Distance(playerTransform.position, transform.position);

        Debug.Log(boxSize);

        Gizmos.DrawWireCube(centerPos, new Vector3(boxSize, boxHeight, boxSize));
    }
}
