using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataBase : MonoBehaviour
{
    public Dictionary<string, TextAsset> dataDic;
    public TextAsset data;

    [SerializeField]
    TextAsset[] database;

    DataBase_Character characterDB;

    private void Awake()
    {
        dataDic = new Dictionary<string, TextAsset>();

        database = Resources.LoadAll<TextAsset>("DataSheet");

        foreach (var data in database)
        {
            dataDic.Add(data.name, data);
        }
    }

    public void Start()
    {
        characterDB = GetComponent<DataBase_Character>();
    }

    public List<Dictionary<string, object>> Parser(string dataName)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = dataDic[dataName];

        StringReader reader = new StringReader(data.text);
        string text = reader.ReadLine();

        string[] row = text.Split(',');
        text = reader.ReadLine();

        while (text != null)
        {
            var newDic = new Dictionary<string, object>();
            string[] rowData = text.Split(',');
            for (int i = 0; i < rowData.Length; i++)
            {
                //Debug.Log(rowData[i]);
                newDic.Add(row[i], rowData[i]);                
            }
                
            list.Add(newDic);

            text = reader.ReadLine();
        }

        return list;
    }

    public string GetCharacterName(int characterID)
    {
        return characterDB.GetCharacterName(characterID);
    }
}
