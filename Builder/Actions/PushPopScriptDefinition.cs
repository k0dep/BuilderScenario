using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class PushScriptDefinition : IAction
    {
        public bool CanAction { get; set; }

        public PushScriptDefinition()
        {
        }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var stack = "";
            if (EditorPrefs.HasKey("DefineStack"))
            {
                stack = EditorPrefs.GetString("DefineStack");
            }

            var list = stack.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            list.Add(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone));

            EditorPrefs.SetString("DefineStack", string.Join("|", list.ToArray()));
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "push script definition");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }

    [SelfNamedTagAttrubute]
    public class PopScriptDefinition : IAction
    {
        public static Stack<string> DefinesStack { get; set; }

        public bool CanAction { get; set; }

        public PopScriptDefinition()
        {
            DefinesStack = new Stack<string>();
        }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var stack = "";
            if (EditorPrefs.HasKey("DefineStack"))
            {
                stack = EditorPrefs.GetString("DefineStack");
            }

            var list = stack.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var define = "";
            if (list.Count != 0)
                define = list.Last();

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, define);

            if(list.Count != 0)
                list.RemoveAt(list.Count - 1);

            EditorPrefs.SetString("DefineStack", string.Join("|", list.ToArray()));
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "pop script definition");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
