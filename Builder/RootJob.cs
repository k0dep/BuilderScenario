using UnityEngine;

namespace BuilderScenario
{
    public class RootJob : IBuildJob
    {
        public IBuildJob[] Jobs { get; set; }
        public bool Disabled { get; set; }

        public bool Run(IJobExecuteService executer, ILogger logger)
        {
            foreach (var job in Jobs)
            {
                executer.Execute(job);
                logger.Log($"Runned job {job}");
            }

            return Disabled;
        }
    }
}