using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class RunBashScriptAction : IAction
    {

        public string Script { get; set; } = "";
        public bool WaitForExit { get; set; }
        public bool CanAction { get; set; } = true;

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            log.Line("running cmd script: " + conf.InterpolateString(Script, target), "run cmd action", "info");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + conf.InterpolateString(Script, target);
            process.StartInfo = startInfo;
            process.Start();
            if (WaitForExit)
            {
                log.Line("waiting process id: " + process.Id, "run cmd action", "info");
                process.WaitForExit();
                log.Line("process id: " + process.Id + " exit with code: " + process.ExitCode, "run cmd action", "info");
            }
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            EditorGUILayout.LabelField("Script: ");
            Script = EditorGUILayout.TextArea(Script, GUILayout.MaxHeight(200.0f));
            WaitForExit = EditorGUILayout.Toggle("Wait for exit process", WaitForExit);
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "cmd script");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
