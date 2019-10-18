using System;
using System.Collections.Generic;

namespace BuilderScenario
{
    public class JobExecuteResult
    {
        public bool IsSucces { get; set; }
        public string Status { get; set;  }
        public string Title { get; set;  }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set;  }
        public List<string> Logs { get; set; }

        public JobExecuteResult()
        {
            Logs = new List<string>();
        }
    }

    public interface IJobExecuteService
    {
        JobExecuteResult Execute(IBuildJob job);
    }

    public class JobExecuter : IJobExecuteService
    {
        public readonly IServiceCollection Container;

        public JobExecuter(IServiceCollection container)
        {
            Container = container;
        }
        
        public JobExecuteResult Execute(IBuildJob job)
        {
            var result = new JobExecuteResult
            {
                Start = DateTime.UtcNow,
                Title = $"Execute job {job}"
            };

            try
            {
                var runMethod = job.GetType().GetMethod("Run");
                if (runMethod == null)
                {
                    throw new MissingMethodException($"Cant find method Run in job in type {job.GetType()}");
                }
                
                var parameters = Container.ResolveArguments(runMethod);
                
                result.Logs.Add($"{DateTime.UtcNow:hh:mm:ss:zz} Runner: Run job {job}");
                runMethod.Invoke(job, parameters);

                result.IsSucces = true;
                result.Status = "ok";
            }
            catch (Exception e)
            {
                result.IsSucces = false;
                result.Status = "exception";
                result.Logs.Add($"Exception: {e}");
            }
            
            result.Finish = DateTime.UtcNow;

            return result;
        }
    }
}