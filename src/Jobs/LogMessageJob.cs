namespace BuilderScenario
{
    [TagAlias("debugLog")]
    public class LogMessageJob : IBuildJob
    {
        public string Message { get; set; }
        
        public void Run(IBuildLogger logger)
        {
            logger.Log(Message);
        }
    }
}