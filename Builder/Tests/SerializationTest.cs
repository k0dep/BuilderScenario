
/*using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

namespace BuilderScenario.TestTools
{
	public class SerializationTest {

		[Test]
		public void SerializationDefaulttest()
		{
			var conf = new BuildScenario();
			
			conf.AbsolutePathForBuilds = "B:/";
			conf.Build = 0;
			conf.Name = "YourExeName";
			conf.ActionsBeforeTargets = new List<IAction>();
			conf.ActionsBeforeTargets.Add(new RunBashScriptAction() { Script = "rm {NAME}", WaitForExit = true });
			conf.ActionsBeforeTargets.Add(new AddFileAction() { Content = "{NAME}.exe -batchmode", Path = "{PATH}/{VER}_b{BUILD}/ServerRun.bat", UseInterpolation = true });

			conf.ActionsAfterTargets = new List<IAction>();
			conf.ActionsAfterTargets.Add(new DeleteFileOrDirAction() { Path = "{PATH}/{VER}_b{NAME}/*.meta" });

			conf.Targets = new List<BuilderTarget>();

			var t1 = new BuilderTarget();
			t1.IsBuilding = false;
			t1.TargetName = "server";
			t1.Actions = new List<IAction>();
			t1.Actions.Add(new BuildAction() { DefinedSymbols = "SERVER", Options = BuildOptions.AllowDebugging, Target = BuildTarget.StandaloneWindows64, Path = "{PATH}/{VER}_b{BUILD}/{TARGET_NAME}_{TARGET_POSTFIX}.exe", Scenes = new List<string>() { "Assets/start.unity" } });

			conf.Targets.Add(t1);

			var tempPath =  Application.dataPath.Replace("Assets", "") + "temp_file_for_test";

			conf.Save(tempPath);

			var resConf = BuildScenario.Load(tempPath);

			//Assert.IsTrue(conf.Equals(resConf));

			Assert.That(conf, Is.EqualTo(resConf));
		}
	}
}*/