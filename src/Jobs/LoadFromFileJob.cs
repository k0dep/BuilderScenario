namespace BuilderScenario
{
    [TagAlias("LoadJobFromFile")]
    public class LoadFromFileJob : IBuildJob
    {
        public string FileName { get; set; }
        
        public bool Run(IJobExecuteService executeService, IConfigMap configMap, IBuildLogger logger, IObjectLoader loader)
        {
            var path = configMap.Interpolate(FileName);
            var job = loader.Load<IBuildJob>(path);

            if (job == null)
            {
                logger.Error($"loaded job is null");
                return false;
            }
            
            logger.Log("job loaded");

            var result = executeService.Execute(job);
            return result.IsSucces;
        }
    }
}