using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowLocationName : MonoBehaviour
{
    [SerializeField]
    AreaSO areaSO;

    private void OnTriggerEnter(Collider other)
    {
        if (areaSO.hasActive)
            return;

        areaSO.text.text = areaSO.areaName;
        areaSO.hasActive = true;
        areaSO.image.gameObject.SetActive(true);
    }
}
