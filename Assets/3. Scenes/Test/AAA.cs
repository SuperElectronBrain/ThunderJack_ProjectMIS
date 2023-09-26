using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAA : MonoBehaviour
{
    [SerializeField]
    Vector3[] vectors = new Vector3[3];
    [SerializeField]
    Vector3[] rotations = new Vector3[3];
    [SerializeField]
    GameObject[] objects = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            objects[i] = transform.GetChild(i).gameObject;
            vectors[i] = transform.GetChild(i).transform.localPosition;
            rotations[i] = transform.GetChild(i).transform.rotation.eulerAngles;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            transform.position = mPos;

            for (int i = 0; i < transform.childCount; i++)
            {
                objects[i].GetComponent<BBB>().ResetObj(vectors[i], rotations[i]);                
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                objects[i].GetComponent<BBB>().ReleseObj();
            }
        }
    }

    private void OnMouseDown()
    {
        
    }
}
