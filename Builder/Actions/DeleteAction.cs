using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class DeleteFileOrDirAction : IAction
    {
        public string Path { get; set; }
        public bool Recursively { get; set; }
        public bool CanAction { get; set; }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var path = conf.InterpolateString(Path, target);
            log.Line("start delete files at path: " + path, "delete files", "info");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C del /F /Q " + (Recursively ? "/S " : "") + path;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            log.Line("deleting success. exit code: " + process.ExitCode, "delete files", "info");
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            Path = EditorGUILayout.TextField("Path", Path);
            Recursively = EditorGUILayout.Toggle("Recursively", Recursively);
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "delete files or dir");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}