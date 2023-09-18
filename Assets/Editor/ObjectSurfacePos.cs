using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectSurfacePos : EditorWindow
{
    static GameObject obj;

    [MenuItem("GameObject/SetSurface")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        ObjectSurfacePos window = (ObjectSurfacePos)EditorWindow.GetWindow(typeof(ObjectSurfacePos));

        window.Show();
    }

    void GetSurfacePos()
    {
        RaycastHit hit;

        if (Physics.Raycast(obj.transform.position, -obj.transform.up, out hit, 10f))
        {
            obj.transform.position = hit.point;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        obj = EditorGUILayout.ObjectField("Game Object", obj, typeof(GameObject), true) as GameObject;

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("GetSurfacePos", GUILayout.Width(120), GUILayout.Height(30)))
        {
            GetSurfacePos();
        }
        else if (GUILayout.Button("Reset", GUILayout.Width(120), GUILayout.Height(30)))
        {

        }
        GUILayout.EndHorizontal();
    }
}
