using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAA : MonoBehaviour
{
    [SerializeField]
    List<Vector3> vectors = new List<Vector3>();
    [SerializeField]
    List<Vector3> rotations = new List<Vector3>();
    [SerializeField]
    List<GameObject> objects = new List<GameObject> ();

    bool isRelese;

    [SerializeField]
    GameObject selectObject;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            vectors.Add(transform.GetChild(i).transform.localPosition);
            rotations.Add(transform.GetChild(i).transform.rotation.eulerAngles);
            objects.Add(transform.GetChild(i).gameObject);
        }
    }

    public void SelectObject(GameObject newSelectObject)
    {
        if (selectObject == null)
        {
            selectObject = newSelectObject;
            selectObject.GetComponent<BBB>().Select();
        }            
    }

    public void DropObject()
    {

    }

    [SerializeField]
    float z;

    private void Update()
    {
        if(!isRelese)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + z));

                //mPos.z = z;
                transform.position = mPos;

                for (int i = 0; i < transform.childCount; i++)
                {
                    objects[i].GetComponent<BBB>().ResetObj(vectors[i], rotations[i]);
                }
            }
            if (Input.GetMouseButton(0))
            {
                var mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + z));

                //mPos.z = z;
                transform.position = mPos;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    objects[i].GetComponent<BBB>().ReleseObj();
                }
                isRelese = true;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if(selectObject != null)
                {
                    Debug.Log("drag");
                    var mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + z));

                    selectObject.transform.position = mPos;
                }                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if(selectObject != null)
                {
                    selectObject.GetComponent<BBB>().ReleseObj();
                    selectObject = null;
                }                
            }
        }        
    }
}
