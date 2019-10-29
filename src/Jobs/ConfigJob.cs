using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("Configurable")]
    public class ConfigurableJob : IBuildJob
    {
        public IConfigMapData ConfigMap { get; set; }
        public IBuildJob Job { get; set; }

        public bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger, IServiceCollection collection)
        {
            logger.BeginScope("set config map");
            
            collection.Inject(ConfigMap);

            var prevVars = new Dictionary<string, object>();

            foreach (var configEntry in ConfigMap)
            {
                logger.Log($"set config '{configEntry.Key}' with value '{configEntry.Value}'");

                if (configMap.Data.TryGetValue(configEntry.Key, out var existsData))
                {
                    prevVars[configEntry.Key] = existsData;
                }
                
                configMap.Set(configEntry.Key, configEntry.Value);
            }
            logger.EndScope();

            var result = executeService.Execute(Job);

            foreach (var prevVar in prevVars)
            {
                configMap.Set(prevVar.Key, prevVar.Value);
            }
            
            return result.IsSucces;
        }
    }
}