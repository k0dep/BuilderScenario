using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    public class _BuilderWindow  : EditorWindow
    {
        private IBuildJob Config { get; set; }
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

            window.Config = null;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            DrawJobTree();
            
            DrawSelectedJob();
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawJobTree()
        {
            
        }
        
        private void DrawSelectedJob()
        {
        }


        private void DrawJobHeader(IBuildJob job)
        {
        }
    }
}