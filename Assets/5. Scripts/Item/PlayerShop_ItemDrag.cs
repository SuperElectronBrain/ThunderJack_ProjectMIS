using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop_ItemDrag : MonoBehaviour
{
    [SerializeField]
    bool isMouseOver;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        Vector3 mPos = Input.mousePosition;

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mPos.x, mPos.y, -Camera.main.transform.position.z + 5));
    }
}
