using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class SaveVersionAction : IAction
    {
        public bool CanAction { get; set; }

        public SaveVersionAction()
        {
        }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            PlayerSettings.bundleVersion = conf.Version + "#" + conf.Build;
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "save player version");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
