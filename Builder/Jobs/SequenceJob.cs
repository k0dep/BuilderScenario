using UnityEngine;

namespace BuilderScenario
{
    [TagAlias("Sequence")]
    public class SequenceJob : IBuildJob
    {
        public IBuildJob[] Jobs { get; set; }

        public void Run(IJobExecuteService executer, ILogger logger)
        {
            foreach (var job in Jobs)
            {
                executer.Execute(job);
                logger.Log($"Runned job {job}");
            }
        }
    }
}