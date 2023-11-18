using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWaterBall : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private GameObject waterBall;

    public GameObject Create(Vector3 createPos)
    {
        var go = Instantiate(waterBall, createPos, Quaternion.identity);

        var scale = Random.Range(minScale, maxScale);

        go.transform.localScale = new Vector3(scale, scale, scale);
        return go;
    }

    public void SetWaterColor(Color color)
    {
        waterBall.GetComponent<SpriteRenderer>().color = color;
    }
}
