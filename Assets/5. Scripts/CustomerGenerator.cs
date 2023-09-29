using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateCustomer()
    {
		GameObject t_GameObject = Instantiate(new GameObject(), this.transform);


        return t_GameObject;
    }
}
