using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class CopyFilesAction : IAction
    {
        public string PathFrom { get; set; }
        public string PathTo { get; set; }
        public bool CanAction { get; set; }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var path = conf.InterpolateString(PathFrom, target);
            var pathto = conf.InterpolateString(PathTo, target);

            log.Line("start copy files at path from: " + path + " to: " + pathto, "copy files", "info");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C xcopy \"" + path + "\" \"" + pathto + "\" /c /i /s /e /t /y";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            log.Line("deleting success. exit code: " + process.ExitCode, "copy files", "info");
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            PathFrom = EditorGUILayout.TextField("From", PathFrom);
            PathTo = EditorGUILayout.TextField("To", PathTo);
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "copy files");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
