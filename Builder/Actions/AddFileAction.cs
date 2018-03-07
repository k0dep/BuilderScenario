using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class AddFileAction : IAction
    {
        public string Path { get; set; }
        public string Content { get; set; } = "";
        public bool UseInterpolation { get; set; } = false;
        public bool CanAction { get; set; }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var path = conf.InterpolateString(Path, target);

            try
            {
                File.WriteAllText(path, UseInterpolation ? conf.InterpolateString(Content, target) : Content);
                log.Line("create file " + path + " use interpolatio: " + UseInterpolation, "add file action", "info");
            }
            catch { log.Line("dont create file " + path, "add file action", "error"); }
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            Path = EditorGUILayout.TextField("Path", Path);
            UseInterpolation = EditorGUILayout.Toggle("Use interpolation content", UseInterpolation);
            Content = EditorGUILayout.TextArea(Content, GUILayout.MaxHeight(500.0f));
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "Create file " + Path);
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
