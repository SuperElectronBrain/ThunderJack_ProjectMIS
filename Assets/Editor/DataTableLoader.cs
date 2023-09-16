using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataTableLoader : EditorWindow
{
    static List<TextAsset> dataTableList = new List<TextAsset>();

    [MenuItem("DataTable/LoadDataTable")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        dataTableList.Clear();
        DataTableLoader window = (DataTableLoader)EditorWindow.GetWindow(typeof(DataTableLoader));

        foreach (TextAsset table in Resources.LoadAll<TextAsset>("DataSheet"))
        {
            dataTableList.Add(table);
        }

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        for (int i = 0; i < dataTableList.Count; i++)
        {
            EditorGUILayout.ObjectField(dataTableList[i], typeof(TextAsset), true);
        }
        GUILayout.EndVertical();
    }
}
