using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectFinder : EditorWindow
{
    private GameObject searchObject;
    private Component[] components;
    private Component searchComponent;
    private GameObject[] searchObjects;
    private bool isSearch;
    
    
    [MenuItem("DataTable/LoadDataTable1")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        ObjectFinder window = (ObjectFinder)EditorWindow.GetWindow(typeof(ObjectFinder));
        

        window.Show();
    }

    void Init()
    {
        components = searchObject.GetComponents<Component>();
        isSearch = false;
    }

    void SearchObjects()
    {
        var type =searchComponent.GetType();
        //searchObjects = (type.GetType())FindObjectsByType<type>()
    }

    private void OnGUI()
    {
        searchObject = (GameObject)EditorGUILayout.ObjectField(searchObject, typeof(GameObject), true);
        
        GUILayout.BeginVertical();

        if (GUILayout.Button("Search Components"))
        {
            Init();
        }

        if (!isSearch)
        {
            for (int i = 0; i < components.Length; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Search Objects"))
                {
                    searchComponent = components[i];
                    isSearch = true;
                }

                EditorGUILayout.TextField(components[i].ToString());
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            
        }
        
        GUILayout.EndVertical();
    }
}
