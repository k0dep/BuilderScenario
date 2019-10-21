namespace BuilderScenario
{
    [TagAlias("Conditional")]
    public class ConditionalJob : IBuildJob
    {
        public IBuildJob Job { get; set; }
        public bool Condition { get; set; }

        public void Run(IJobExecuteService executer)
        {
            if (Condition)
            {
                executer.Execute(Job);
            }
        }
    }
}