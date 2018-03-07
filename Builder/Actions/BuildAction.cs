using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class BuildAction : IAction
    {
        public string Path { get; set; }
        public List<string> Scenes { get; set; }
        public BuildTarget Target { get; set; }
        public BuildOptions Options { get; set; }
        public string DefinedSymbols { get; set; }
        public bool CanAction { get; set; }

        private ReorderableList _scenes;

        public BuildAction()
        {
            Path = "";
            Scenes = new List<string>();
            Target = BuildTarget.StandaloneWindows64;
            Options = BuildOptions.Development;
            DefinedSymbols = "";
        }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var targetPath = conf.InterpolateString(Path, target);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetPath));
            log.Line("building player to " + targetPath + ", target: " + Target.ToString() + ", options: " + Options.ToString(), "Build Action", "info");

            var targetBuildOption = new BuildPlayerOptions
            {
                target = Target,
                scenes = Scenes.ToArray(),
                options = BuildOptions.Development,
                locationPathName = targetPath
            };


            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, DefinedSymbols);
            var res = BuildPipeline.BuildPlayer(targetBuildOption);
            log.Line("player build successful, errors: " + res, "Build Action", "info");
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            Path = EditorGUILayout.TextField("Path", Path);
            Target = (BuildTarget)EditorGUILayout.EnumPopup("Build target", Target);
            Options = (BuildOptions)EditorGUILayout.EnumMaskField("Build obtions", Options);
            DefinedSymbols = EditorGUILayout.TextField("Defined symbol", DefinedSymbols);

            if(_scenes == null)
            {
                _scenes = new ReorderableList(Scenes, typeof(string), true, true, true, true);
                _scenes.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Scenes", EditorStyles.boldLabel);
                _scenes.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.height -= 2.0f;
                    rect.y += 1.0f;
                    var obj = EditorGUI.ObjectField(rect, AssetDatabase.LoadAssetAtPath(Scenes[index], typeof(SceneAsset)), typeof(SceneAsset), false);
                    Scenes[index] = AssetDatabase.GetAssetPath(obj);
                };
                _scenes.onAddCallback = (ReorderableList l) =>  l.list.Add("");
            }

            _scenes.DoLayoutList();
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "Build action");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
