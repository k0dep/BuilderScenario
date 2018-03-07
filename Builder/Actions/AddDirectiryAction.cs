using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class AddDirectoryAction : IAction
    {
        public string Path { get; set; }
        public bool CanAction { get; set; }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var path = conf.InterpolateString(Path, target);
            Directory.CreateDirectory(path);
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            Path = EditorGUILayout.TextField("Path", Path);
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "add directory");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
