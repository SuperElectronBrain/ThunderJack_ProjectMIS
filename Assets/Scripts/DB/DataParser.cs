using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public static List<Dictionary<string, object>> dataParser(string dataName)
    {
        var list = DataBase.Instance.Parser(dataName);

        return list;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}