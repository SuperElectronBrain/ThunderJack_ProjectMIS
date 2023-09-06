using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;

public class AutoBuildManager : MonoBehaviour
{
    [MenuItem("Build/Build")]
    public static void AutoBuild()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();

        List<string> scenes = new List<string>();

        foreach(var scene in EditorBuildSettings.scenes)
        {
            if(!scene.enabled) continue;
            scenes.Add(scene.path);
        }

        options.scenes = scenes.ToArray();
        //options.locationPathName = "Build/Build_Ver" + version++ + ".exe";

        DateTime date = DateTime.Now;

        options.locationPathName = "../../Build\\MIS_Build\\Build_" + string.Format("{0:D2}", date.Month) + string.Format("{0:D2}", date.Day) + ".exe";
        options.target = BuildTarget.StandaloneWindows64;
        var result = BuildPipeline.BuildPlayer(options);

        if (result.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + result.summary.totalSize + " bytes");
        }
        else if (result.summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
