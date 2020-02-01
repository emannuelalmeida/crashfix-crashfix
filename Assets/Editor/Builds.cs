using System.Collections.Generic;
using UnityEditor;

namespace Editor
{
    class Builds
    {
        static readonly string APP_NAME = "crashfix-crashfix";
        static readonly string TARGET_DIR = "Build";

        [MenuItem("Custom/CI/Build WebGL")]
        static void PerformWebGLBuild()
        {
            var appName = APP_NAME + "-web";
            GenericBuild(TARGET_DIR + "/" + appName, BuildTarget.WebGL, BuildOptions.None);
        }

        private static string[] FindEnabledEditorScenes()
        {
            var editorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                editorScenes.Add(scene.path);
            }

            return editorScenes.ToArray();
        }

        static void GenericBuild(string target_dir, BuildTarget build_target, BuildOptions build_options)
        { 
            var scenes = FindEnabledEditorScenes();
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            var report = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
//            if (res.Length > 0) {
//                throw new Exception("BuildPlayer failure: " + res);
//            }
        }
    }
}