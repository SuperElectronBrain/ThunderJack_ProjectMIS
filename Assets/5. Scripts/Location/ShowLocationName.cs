using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowLocationName : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    string areaName;

    bool hasActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasActive)
            return;

        text.text = areaName;
        hasActive = true;
        image.gameObject.SetActive(true);
    }
}
