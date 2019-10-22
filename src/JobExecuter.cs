using System;
using UnityEngine;

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
            var logger = Container.Resolve<IBuildLogger>();
            logger.BeginScope($"execute job: {job}");

            var result = new JobExecuteResult
            {
                Start = DateTime.Now,
                Finish = DateTime.Now,
                IsSucces = false
            };
            
            if (job == null)
            {
                logger.Error("job is null. Check config");
                logger.EndScope();
                return result;
            }

            try
            {
                var runMethod = job.GetType().GetMethod("Run");
                if (runMethod == null)
                {
                    throw new MissingMethodException($"Cant find method Run in job in type {job.GetType()}");
                }
                
                var parameters = Container.ResolveArguments(runMethod);
                var invokeResult = runMethod.Invoke(job, parameters);

                if (invokeResult is bool boolResult)
                {
                    result.IsSucces = boolResult;
                    if (!boolResult)
                    {
                        logger.Log("job return fail result");
                    }
                }
                else
                {
                    result.IsSucces = true;
                }
            }
            catch (Exception e)
            {
                result.IsSucces = false;
                logger.Exception(e);
            }
            
            result.Finish = DateTime.UtcNow;
            
            logger.EndScope();

            return result;
        }
    }
}