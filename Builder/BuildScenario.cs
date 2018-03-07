using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using YamlDotNet;
using YamlDotNet.Serialization.NamingConventions;

namespace BuilderScenario
{
    public class BuildScenario
    {
        public string Name { get; set; }
        public string AbsolutePathForBuilds { get; set; }
        public string Version { get; set; }
        public Int32 Build { get; set; }
        public string BuildResultPath { get; set; }

        public List<IAction> ActionsBeforeTargets { get; set; }
        public List<IAction> ActionsAfterTargets { get; set; }

        public List<BuilderTarget> Targets { get; set; }

        private int progress = 0;
        private int progressAll = 0;
        private bool showProgress = false;
        private string currentTargetName = "";
        private string currentActionType = "";

        public BuildScenario()
        {
            Name = "";
            AbsolutePathForBuilds = "";
            Version = "";
            Build = 0;
            BuildResultPath = "";
            ActionsAfterTargets = new List<IAction>();
            ActionsBeforeTargets = new List<IAction>();
            Targets = new List<BuilderTarget>();
        }

        public string InterpolateString(string mask, int target = -1)
        {
            var targetObj = target >= 0 ? Targets[target] : null;

            mask = mask.Replace("{NAME}", Name);
            mask = mask.Replace("{PATH}", AbsolutePathForBuilds);
            mask = mask.Replace("{VER}", Version);
            mask = mask.Replace("{BUILD}", Build.ToString());
            mask = mask.Replace("{PROJECT_PATH}", Application.dataPath.Replace("Assets", "").Replace('/', '\\'));
            if (targetObj != null)
            {
                mask = mask.Replace("{TARGET_NAME}", targetObj.TargetName);
            }

            return mask;
        }

        public static BuildScenario Load(string path)
        {
            var yamlBuilder = new YamlDotNet.Serialization.DeserializerBuilder();
            foreach (var type in _GetMappingTypes())            
                yamlBuilder.WithTagMapping("!" + type.Name, type);            
            
            return yamlBuilder.Build().Deserialize<BuildScenario>(File.ReadAllText(path));
        }

        public void Save(string path)
        {
            var yamlBuilder = new YamlDotNet.Serialization.SerializerBuilder();
            yamlBuilder.EmitDefaults();
            foreach(var type in _GetMappingTypes())            
                yamlBuilder.WithTagMapping("!" + type.Name, type);            
            
            File.WriteAllText(path, yamlBuilder.Build().Serialize(this));
        }

        public static BuildScenario Default()
        {
            return null;
        }

        private static List<Type> _GetMappingTypes()
        {         
            var res = new List<Type>();
            var allTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                allTypes.AddRange(assembly.GetTypes());

            foreach(var type in allTypes)
                foreach(var attr in type.GetCustomAttributes(false))
                    if(attr is SelfNamedTagAttrubute)
                    {
                        res.Add(type);
                        break;
                    }

            return res;
        }

        public BuildLog StartBuild(bool drawProgress = false, string targetNeeded = null, bool tcLog = false)
        {
            showProgress = drawProgress;
            var buildLog = !tcLog ? new BuildLog() : new BuildLogTC();

            buildLog.Start();

            buildLog.Line("starting executing action before all targets", "builder", "info");

            ProgressBarStart();

            ProcessActions(ActionsBeforeTargets, -1, buildLog);

            for (int i = 0; i < Targets.Count; i++)
            {
                var target = Targets[i];

                if (!target.IsBuilding || (targetNeeded != null && targetNeeded != target.TargetName))
                {
                    buildLog.Line("passing target " + target.TargetName, "builder", "info");
                    continue;
                }

                currentTargetName = target.TargetName;
                buildLog.Line("starting execution actions from target " + target.TargetName, "builder", "info");
                ProcessActions(target.Actions, i, buildLog);
            }

            buildLog.Line("starting executing action after all targets", "builder", "info");
            ProcessActions(ActionsAfterTargets, -1, buildLog);

            buildLog.End();
            ProgressBarEnd();

            Build++;

            return buildLog;
        }

        void ProcessActions(List<IAction> actions, int target, BuildLog log)
        {
            foreach (var action in actions)
            {
                if (action.CanAction)
                {
                    ProgressBar();
                    log.Line("running action " + action.GetType().Name + " from target " + target, "builder", "info");
                    currentActionType = action.GetType().Name;
                    action.Do(this, target, log);
                }
                else
                    log.Line("passed action " + action.GetType().Name, "builder", "info");
            }
        }


        void ProgressBarStart()
        {
            if (!showProgress) return;

            foreach (var action in ActionsAfterTargets)
                progressAll += action.CanAction ? 1 : 0;

            foreach (var action in ActionsBeforeTargets)
                progressAll += action.CanAction ? 1 : 0;

            foreach (var target in Targets)
                foreach (var action in target.Actions)
                    progressAll += action.CanAction && target.IsBuilding ? 1 : 0;
        }

        public void ProgressBar()
        {
            if (!showProgress) return;

            EditorUtility.DisplayProgressBar("Building", "Building " + currentTargetName + " - " + currentActionType, progress / progressAll);
        }

        void ProgressBarEnd()
        {
            if (!showProgress) return;

            EditorUtility.ClearProgressBar();
        }
    }

}