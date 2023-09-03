using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class AutoBuildManager : MonoBehaviour
{
    static int version = 1;
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
        options.locationPathName = "D\\test.exe";
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
