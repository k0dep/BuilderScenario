namespace BuilderScenario
{
    [TagAlias("dummy", true)]
    public class DummyJob : IBuildJob
    {
        public void Run(IBuildLogger logger)
        {
            logger.Log("dummy job");
        }
    }
}