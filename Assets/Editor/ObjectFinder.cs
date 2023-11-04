using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectFinder : EditorWindow
{
    [MenuItem("DataTable/LoadDataTable1")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        ObjectFinder window = (ObjectFinder)EditorWindow.GetWindow(typeof(ObjectFinder));
        

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        
        
        GUILayout.EndVertical();
    }
}
