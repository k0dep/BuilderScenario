namespace BuilderScenario
{
    [TagAlias("Sequence")]
    public class SequenceJob : IBuildJob
    {
        public IBuildJob[] Jobs { get; set; }

        public bool Run(IJobExecuteService executer)
        {
            foreach (var job in Jobs)
            {
                var result = executer.Execute(job);
                if (!result.IsSucces)
                {
                    return false;
                }
            }

            return true;
        }
    }
}