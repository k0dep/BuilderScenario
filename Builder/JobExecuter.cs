using System;

namespace BuilderScenario
{
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
                var invokeResult = runMethod.Invoke(job, parameters);

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