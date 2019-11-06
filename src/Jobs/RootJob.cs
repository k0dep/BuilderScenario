namespace BuilderScenario
{
    public class RootJob : IBuildJob
    {
        public IConfigMapData ConfigMap { get; set; }
        public IBuildJob Job { get; set; }

        public virtual bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger, IServiceCollection collection, IConfigMapData parameters)
        {
            logger.BeginScope("set config map");
            
            configMap.Fill(ConfigMap);
            configMap.Fill(parameters);
            
            logger.EndScope();

            var result = executeService.Execute(Job);

            return result.IsSucces;
        }
    }
}