using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    public class _BuilderWindow : EditorWindow
    {
        private IBuildListJob Config { get; set; }
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

            window.Config = new DummyListJob()
            {
                ChildJobs = new IBuildJob[]
                {
                    new DummyJob(),
                    new DummyListJob()
                    {
                        ChildJobs = new IBuildJob[]
                        {
                            new DummyListJob()
                            {
                                ChildJobs = new IBuildJob[]
                                {
                                    new DummyJob(),
                                    new DummyJob(),
                                    new DummyJob()
                                }
                            }
                        }
                    },
                    new DummyJob(),
                    new DummyJob()
                }
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space(50, false);
            
            EditorGUILayout.BeginVertical();
            DrawJobTree(Config.ChildJobs);
            EditorGUILayout.EndVertical();
            
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
            if (job is IBuildListJob listJob)
            {
                ToggledJobs.TryGetValue(job, out var isToggle);
                EditorGUILayout.BeginHorizontal();
                ToggledJobs[job] = isToggle = EditorGUILayout.Foldout(isToggle, "");
                job.Enabled = EditorGUILayout.Toggle(job.Enabled, GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();

                if (isToggle)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    EditorGUILayout.Space(20, false);
                    
                    EditorGUILayout.BeginVertical();
                    DrawJobTree(listJob.ChildJobs);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(job.ToString());
                job.Enabled = EditorGUILayout.Toggle(job.Enabled, GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}