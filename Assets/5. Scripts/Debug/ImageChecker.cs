using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChecker : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            for (int i = 1; i <= 55; i++)
            {
                var img = Instantiate(image, panel.transform);
                img.sprite = GameManager.Instance.ItemManager.GetItemSprite(i);
            }                
        }
    }
}
