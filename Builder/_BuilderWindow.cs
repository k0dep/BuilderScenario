using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    public class _BuilderWindow : EditorWindow
    {
        private IBuildJob SelectedJob { get; set; }

        private Dictionary<object, bool> ToggledJobs { get; set; }

        void Init()
        {
            ToggledJobs = new Dictionary<object, bool>();
        }

        [MenuItem("Build/Builder window 2")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<_BuilderWindow>();
            window.titleContent = new GUIContent("Builder scenario");
            window.Init();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space(50, false);
            EditorGUILayout.Space(50, false);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawJobTree(IBuildJob[] job)
        {
            foreach (var j in job)
            {
                DrawJobHeader(j);
            }
        }

        private void DrawSelectedJob()
        {
        }


        private void DrawJobHeader(IBuildJob job)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(job.ToString());
            job.Enabled = EditorGUILayout.Toggle(job.Enabled, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
        }
    }
}