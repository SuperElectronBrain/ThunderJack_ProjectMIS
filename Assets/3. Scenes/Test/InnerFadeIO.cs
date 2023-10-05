using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(InnerFadeIO))]
public class InnerFadeIOEditor : Editor
{
    InnerFadeIO innerFadeIO;

    private void OnEnable()
    {
        innerFadeIO = target as InnerFadeIO;
        UnityEditorInternal.ComponentUtility.MoveComponentUp(innerFadeIO);
    }

    public override void OnInspectorGUI()
    {
        if (innerFadeIO == null)
            return;

        if (GUILayout.Button("Fade In"))
            innerFadeIO.FadeIn();
        if (GUILayout.Button("Fade Out"))
            innerFadeIO.FadeOut();
    }
}

#endif

public class InnerFadeIO : MonoBehaviour
{
    public float minScale;
    public float maxScale;

    public void FadeIn()
    {

    }

    public void FadeOut()
    {

    }

    IEnumerator StartFadeIn()
    {
        float size = transform.localScale.x;

        while(size < maxScale)
        {
            size += Time.deltaTime;
            transform.localScale = new Vector3(size, size, size);
            yield return null;
        }
    }

    IEnumerator StartFadeOut()
    {
        Vector3 scale = gameObject.transform.localScale;
        yield return null;
    }
}


