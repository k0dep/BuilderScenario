namespace BuilderScenario
{
    [TagAlias("Conditional")]
    public class ConditionalJob : IBuildJob
    {
        public IBuildJob Job { get; set; }
        public bool Condition { get; set; }

        public bool Run(IJobExecuteService executer)
        {
            if (Condition)
            {
                return executer.Execute(Job).IsSucces;
            }

            return true;
        }
    }
}