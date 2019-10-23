namespace BuilderScenario
{
    [TagAlias("Sequence")]
    public class SequenceJob : IBuildJob
    {
        public IBuildJob[] Jobs { get; set; }
        public IBuildJob JobOnError { get; set; } = null;

        public bool Run(IJobExecuteService executer)
        {
            foreach (var job in Jobs)
            {
                var result = executer.Execute(job);
                if (!result.IsSucces)
                {
                    if (JobOnError != null)
                    {
                        var resultFinalize = executer.Execute(job);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}