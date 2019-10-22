using UnityEditor;
using UnityEngine;


namespace BuilderScenario
{
    public class BuildTests
    {
        [MenuItem("Build/Test/Test 1")]
        public static void Test1()
        {
            var container = new ServiceCollection();
            var executer = new JobExecuter(container);

            container.Register<IServiceCollection>(container);
            container.Register<IJobExecuteService>(executer);
            
            var logger = new XmlBuildLogger();
            container.Register<IBuildLogger>(logger);

            var configMap = new ConfigMap();
            container.Register<IConfigMap>(configMap);

            var jobData = 
@"
ConfigMap:
  BuildPath: Path
  WorkingDir: !Env
    UnixEnv: PWD
  WorkingDir1: !EnvPWD

Job: !Sequence
  Jobs:
    - !debugLog
      Message: 'Hello from job!'
    - !SetVersion
      Version: '1.5.0'
    - !Conditional
      Condition: true
      Job: !Sequence
        Jobs:
          - !debugLog
            Message: 'Hello from job!' 
";

            var rootJob = new JobLoader().Deserialize<RootJob>(jobData);
            var result = executer.Execute(rootJob);

            Debug.Log($"IsSuccess: {result.IsSucces}");
            
            Debug.Log($"Logs:\n{logger}");
        }
    }
}