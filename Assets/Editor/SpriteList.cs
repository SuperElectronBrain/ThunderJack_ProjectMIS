using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class SpriteList : EditorWindow
{
    static List<string> list = new List<string>();
    GameObject obj;
    Component selectComponent;
    Component[] components;
    FieldInfo[] fields;
    FieldInfo selectField;
    bool isSetting;
    bool isSelect;

    [MenuItem("SpriteListUp/ListUp")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        SpriteList window = (SpriteList)EditorWindow.GetWindow(typeof(SpriteList));        

        window.Show();
    }

    void Init()
    {
        isSetting = true;
        components = obj.GetComponents<Component>();
    }

    void ShowObjectComponents()
    {
        foreach (var component in components)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(15)))
            {
                selectComponent = component;
                isSelect = true;
            }
            GUILayout.TextField(component.ToString());
            GUILayout.EndHorizontal();
        }        
    }

    void ShowComponentFields()
    {
        fields = selectComponent.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach(var field in fields)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Select", GUILayout.Width(50), GUILayout.Height(15)))
                {
                    selectField = field;
                }
            } 
            GUILayout.TextField(field.ToString());
            GUILayout.EndHorizontal();
            /*if (field.FieldType == typeof(List<Sprite>))
                GUILayout.TextField(field.ToString());*/
        }
    }

    void ShowFieldSprite()
    {
        var field = selectField;
        field.GetValue(obj);
        Type type = selectField.GetType();

        var properties = type.GetProperties();
        GUILayout.TextField(selectField.Name);
        GUILayout.TextField(selectField.FieldType.GetType().ToString());

        /*foreach(var field in values)
        {
            //EditorGUILayout.ObjectField("asdf", field, typeof(Sprite), true);
            GUILayout.TextField(field.Name);
        }*/
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        if(isSetting)
        {
            GUILayout.Label("Components");
            ShowObjectComponents();
        }
        if(isSelect)
        {
            GUILayout.Label("Fields");
            ShowComponentFields();
        }
        if(selectField != null)
        {
            ShowFieldSprite();
        }

        GUILayout.EndVertical();

        obj = EditorGUILayout.ObjectField("Input GameObject", obj, typeof(GameObject), true) as GameObject;

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Init", GUILayout.Width(120), GUILayout.Height(30)))
        {
            Init();
        }
        else if(GUILayout.Button("Reset", GUILayout.Width(120), GUILayout.Height(30)))
        {
            isSetting = false;
        }
        GUILayout.EndHorizontal();
    }
}
