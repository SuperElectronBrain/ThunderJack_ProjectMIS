using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.DOTweenEditor;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UI_Sequence), true)]
public class TweenAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();

        bool isPlaying = EditorApplication.isPlaying;


        if (GUILayout.Button("재생"))
        {
            if (!isPlaying)
            {
                //DOTweenEditorPreview.Stop(true);
                (target as UI_Sequence).BakeSeqence();

                var sequence = (Sequence)typeof(UI_Sequence).GetField("_sequence", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default)?.GetValue(target);
                DOTweenEditorPreview.PrepareTweenForPreview(sequence, true, false);
                DOTweenEditorPreview.Start();
            }
            else
            {
                (target as UI_Sequence).ReStart();
            }
        }
        if (GUILayout.Button(isPlaying ? "재시작" : "처음처럼"))
        {
            if (!isPlaying)
            {
                DOTweenEditorPreview.Stop(true);
            }
            else
            {
                (target as UI_Sequence).ReStart();
            }
        }

        EditorGUILayout.EndHorizontal();


        base.OnInspectorGUI();
    }
}
