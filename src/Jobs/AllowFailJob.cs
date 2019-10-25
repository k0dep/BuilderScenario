namespace BuilderScenario
{
    [TagAlias("AllowFail")]
    public class AllowFailJob : IBuildJob
    {
        public IBuildJob Job { get; set; }
        
        public void Run(IJobExecuteService executeService)
        {
            executeService.Execute(Job);
        }
    }
}