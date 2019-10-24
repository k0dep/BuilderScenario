using System.Collections.Generic;

namespace BuilderScenario
{
    public class RootJob : IBuildJob
    {
        public IConfigMapData ConfigMap { get; set; }
        public IBuildJob Job { get; set; }

        public bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger, IServiceCollection collection)
        {
            logger.BeginScope("set config map");
            
            collection.Inject(ConfigMap);

            foreach (var configEntry in ConfigMap)
            {
                logger.Log($"set config '{configEntry.Key}' with value '{configEntry.Value}'");    
                configMap.Set(configEntry.Key, configEntry.Value);
            }
            logger.EndScope();

            var result = executeService.Execute(Job);
            return result.IsSucces;
        }
    }
}