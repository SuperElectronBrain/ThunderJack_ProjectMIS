using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAccessory : MonoBehaviour
{
    private Camera cam;
    private PressAccessoryPlate accessoryPlate;
    [SerializeField] private GameObject target;
    [SerializeField] private float zOffset;
    private RaycastHit hit;
    private int itemID;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(int itemID, PressAccessoryPlate accessoryPlate)
    {
        this.itemID = itemID;
        this.accessoryPlate = accessoryPlate;
    }

    public void Init(int itemID)
    {
        this.itemID = itemID;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -Camera.main.transform.position.z + zOffset));

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Guest") | 1 << LayerMask.NameToLayer("Press");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
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
                switch(target.tag)
                {
                    case "Guest":
                        target.GetComponent<Guest>().CheckItem(itemID, 100);
                        break;
                    case "Press":
                        target.GetComponent<PressAccessoryPlate>().SetAccessory(itemID);
                        break;
                }                
            }
            else
            {
                if (accessoryPlate != null)
                {
                    accessoryPlate.RewindPlate();
                    Camera.main.GetComponent<CraftTableCameraController>().GoToCraft();
                }
            }
            Destroy(gameObject);
        }
    }
}
