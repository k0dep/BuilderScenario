using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    public class BuilderHelper
    {
        private static bool isInited = false;

        public static void BuilderHelperEditorStart()
        {
            if (isInited) return;
            isInited = true;

            var command = Environment.GetCommandLineArgs();
            var target = "";
            var buildConfigPath = "";
            var buildNumber = -1;
            var tcLogging = false;
            var withLogging = false;
            var version = "";

            foreach (var arg in command)
            {
                if (arg.StartsWith("-BuildScenarioTarget="))
                    target = arg.Split('=')[1];
                else if (arg.StartsWith("-BuildScenarioConfig="))
                    buildConfigPath = arg.Split('=')[1];
                else if (arg.StartsWith("-BuildScenarioBuild="))
                    buildNumber = int.Parse(arg.Split('=')[1].Trim());
                else if (arg.StartsWith("-BuildScenarioTCLog"))
                    tcLogging = true;
                else if (arg.StartsWith("-BuildScenarioWithLog"))
                    withLogging = true;
                else if (arg.StartsWith("-BuildScenarioVersion="))
                {
                    version = arg.Split('=')[1];
                    Debug.Log("Version: " + version + " args: " + arg);
                }
            }

            if (buildConfigPath != "" && target != "")
            {
                if(tcLogging)
                    Debug.Log("##teamcity[progressMessage 'BuildScenraio start']");
                BuildTarget(buildConfigPath, target, buildNumber, tcLogging, withLogging, version);
                EditorApplication.Exit(0);
            }
        }

        [MenuItem("Build/Start build")]
        public static void BuildFromConfig()
        {
            if (!EditorPrefs.HasKey("PathToBuildConfig"))
            {
                Debug.Log("Not setted Path to build config key");
                return;
            }

            var scenario = BuildScenario.Load(EditorPrefs.GetString("PathToBuildConfig"));
            scenario.Build++;
            scenario.Save(EditorPrefs.GetString("PathToBuildConfig"));
            scenario.Build--;
            var buildResult = scenario.StartBuild(true);
            File.WriteAllText(scenario.InterpolateString(scenario.BuildResultPath), buildResult.ToString());
        }

        public static void BuildTarget(string configPath, string target, int build, bool tcLog = false, bool withLogging = true, string version = null)
        {
            var scenario = BuildScenario.Load(configPath);

            if (version != null)
                scenario.Version = version;

            Debug.Log("Version: " + scenario.Version);

            scenario.Build = build;

            var buildResult = scenario.StartBuild(true, target, tcLog);
            if(withLogging)
                File.WriteAllText(scenario.InterpolateString(scenario.BuildResultPath), buildResult.ToString());
            //scenario.Save(configPath);
        }
    }
}