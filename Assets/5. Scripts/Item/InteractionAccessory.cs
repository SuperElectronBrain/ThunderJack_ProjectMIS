using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionAccessory : MonoBehaviour
{
    private Camera cam;
    private PressAccessoryPlate accessoryPlate;
    [SerializeField] private GameObject target;
    private RaycastHit hit;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(PressAccessoryPlate accessoryPlate)
    {
        this.accessoryPlate = accessoryPlate;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -Camera.main.transform.position.z));

            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Guest")))
            {
                target = hit.transform.gameObject;
            }
            else
                target = null;
            
            transform.position = mPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                Debug.Log("주기");
            }
            else
            {
                accessoryPlate.RewindPlate();
            }
            Destroy(gameObject);
        }
    }
}
