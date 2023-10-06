using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(WallCollider))]
public class WallColliderEditor : Editor
{
    WallCollider wallCollider;

    private void OnEnable()
    {
        wallCollider = target as WallCollider;
        UnityEditorInternal.ComponentUtility.MoveComponentUp(wallCollider);
        wallCollider.OnEnableFromInspector();
    }

    public override void OnInspectorGUI()
    {
        if (wallCollider == null)
            return;

        EditorGUI.BeginChangeCheck();

        wallCollider.radius = EditorGUILayout.FloatField("Radius", wallCollider.radius);
        wallCollider.height = EditorGUILayout.FloatField("Height", wallCollider.height);

        if (EditorGUI.EndChangeCheck())
            wallCollider.UpdateWalls();

        if (GUILayout.Button("Generate Wall"))
            wallCollider.GenerateWalls();

        if (GUILayout.Button("Remove Wall"))
            wallCollider.RemoveWalls();
    }
}
#endif

[RequireComponent(typeof(LineRenderer))]
public class WallCollider : MonoBehaviour
{
    public float height;
    public float radius;
    float Diameter => radius * 2;

    [SerializeField]
    LineRenderer lineRenderer;

    public void OnEnableFromInspector()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.receiveShadows = false;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = Diameter;
        lineRenderer.endWidth = Diameter;
        lineRenderer.numCornerVertices = 10;
        lineRenderer.numCapVertices = 10;

        if(lineRenderer.sharedMaterial == null)
        {
            var material = new Material(Shader.Find("Sprites/Default"));
            material.color = Color.gray;
            lineRenderer.sharedMaterial = material;
        }
    }

    public void UpdateWalls()
    {
        Array.ForEach(GetComponents<CapsuleCollider>(), x => { x.radius = radius; x.height = height; });

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = new Vector3(Diameter, height - Diameter, transform.GetChild(i).localScale.z);
        }

        lineRenderer.startWidth = Diameter;
        lineRenderer.endWidth = Diameter;
    }

    public void GenerateWalls()
    {
        RemoveWalls();

        int size = lineRenderer.positionCount;
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < size; i++)
        {
            var capsule = gameObject.AddComponent<CapsuleCollider>();
            points.Add(lineRenderer.GetPosition(i));

            capsule.center = lineRenderer.GetPosition(i);
            capsule.height = height;
            capsule.radius = radius;
        }

        for (int i = 0; i < size - 1; i++)
        {
            var box = new GameObject().AddComponent<BoxCollider>().transform;
            box.name = "Wall";
            box.SetParent(transform);

            box.localPosition = (points[i] + points[i + 1]) * 0.5f;
            box.localRotation = Quaternion.LookRotation(points[i + 1] - points[i]);
            box.localScale = new Vector3(Diameter, height - Diameter, Vector3.Distance(points[i], points[i + 1]));
        }
    }

    public void RemoveWalls()
    {
        Array.ForEach(GetComponents<CapsuleCollider>(), x => DestroyImmediate(x));

        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
}