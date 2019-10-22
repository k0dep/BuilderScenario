using System.Collections.Generic;

namespace BuilderScenario
{
    public class RootJob : IBuildJob
    {
        public IDictionary<string, object> ConfigMap { get; set; }
        public IBuildJob Job { get; set; }

        public bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger)
        {
            foreach (var configEntry in ConfigMap)
            {
                logger.Log($"set config '{configEntry.Key}' with value '{configEntry.Value}'");    
                configMap.Set(configEntry.Key, configEntry.Value);
            }

            var result = executeService.Execute(Job);
            return result.IsSucces;
        }
    }
}