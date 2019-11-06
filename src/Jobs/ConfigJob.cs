using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("Configurable")]
    public class ConfigurableJob : IBuildJob
    {
        public IConfigMapData ConfigMap { get; set; }
        public IBuildJob Job { get; set; }

        public virtual bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger, IServiceCollection collection)
        {
            logger.BeginScope("set config map");

            var prevVars = configMap.Fill(ConfigMap);
            
            logger.EndScope();

            var result = executeService.Execute(Job);

            configMap.Set(prevVars);
            
            foreach (var prevVar in prevVars)
            {
                configMap.Set(prevVar.Key, prevVar.Value);
            }
            
            return result.IsSucces;
        }
    }
}